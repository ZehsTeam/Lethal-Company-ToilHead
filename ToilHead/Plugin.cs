using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.ToilHead.MonoBehaviours;
using com.github.zehsteam.ToilHead.Patches;
using HarmonyLib;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
internal class Plugin : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static Plugin Instance;
    internal static ManualLogSource logger;

    internal SyncedConfigManager ConfigManager;

    public static bool IsHostOrServer => NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        logger = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        harmony.PatchAll(typeof(GameNetworkManagerPatch));
        harmony.PatchAll(typeof(StartOfRoundPatch));
        harmony.PatchAll(typeof(RoundManagerPatch));
        harmony.PatchAll(typeof(TerminalPatch));
        harmony.PatchAll(typeof(EnemyAIPatch));

        ConfigManager = new SyncedConfigManager();

        Content.Load();
        EnemyAIPatch.Reset();

        NetcodePatcherAwake();
    }

    private void NetcodePatcherAwake()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();

        foreach (var type in types)
        {
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);

                if (attributes.Length > 0)
                {
                    method.Invoke(null, null);
                }
            }
        }
    }

    public void OnLocalDisconnect()
    {
        logger.LogInfo($"Local player disconnected. Removing hostConfigData.");
        ConfigManager.SetHostConfigData(null);
    }

    public void OnNewLevelLoaded()
    {
        // I will enabled this when Wesley's Asteroid13 moons is working in v50 (This includes LLL working in v50)
        //Secret.SpawnSecrets();
    }

    public void OnShipHasLeft()
    {
        EnemyAIPatch.DespawnAllTurrets();
        EnemyAIPatch.Reset();
    }

    public bool SetToilHeadOnServer(EnemyAI enemyAI)
    {
        if (!IsHostOrServer) return false;

        if (enemyAI == null)
        {
            logger.LogError("Error: failed to set Toil-Head on server. EnemyAI could not be found.");
            return false;
        }

        if (!Utils.IsSpring(enemyAI))
        {
            logger.LogError("Error: failed to set Toil-Head on server. EnemyAI is not a Coil-Head.");
            return false;
        }

        if (Utils.IsToilHead(enemyAI))
        {
            logger.LogError("Error: failed to set Toil-Head on server. EnemyAI is already a Toil-Head. Skipping.");
            return false;
        }

        if (Content.turretPrefab == null)
        {
            logger.LogError("Error: failed to set Toil-Head on server. Turret prefab could not be found.");
            return false;
        }

        NetworkObject enemyNetworkObject = enemyAI.gameObject.GetComponent<NetworkObject>();
        NetworkObject turretNetworkObject = SpawnTurretOnServer(enemyAI.gameObject.transform);
        EnemyAIPatch.AddEnemyTurretPair(enemyNetworkObject, turretNetworkObject);

        PluginNetworkBehaviour.Instance.SetToilHeadClientRpc(NetworkUtils.GetNetworkObjectId(enemyNetworkObject));
        SetToilHeadOnLocalClient(enemyAI.gameObject);

        EnemyAIPatch.spawnCount++;

        return true;
    }

    private NetworkObject SpawnTurretOnServer(Transform parent)
    {
        if (!IsHostOrServer) return null;

        GameObject turretObject = Object.Instantiate(Content.turretPrefab, Vector3.zero, Quaternion.identity, parent);

        NetworkObject turretNetworkObject = turretObject.GetComponent<NetworkObject>();
        turretNetworkObject.Spawn(destroyWithScene: true);

        turretObject.transform.SetParent(parent);

        return turretNetworkObject;
    }

    public void SetToilHeadOnLocalClient(GameObject enemyObject)
    {
        Transform enemyHeadTransform = enemyObject.transform.GetChild(0).Find("Head");
        if (enemyHeadTransform == null) return;

        Transform turretTransform = enemyObject.transform.Find("ToilHeadTurretContainer(Clone)");
        if (turretTransform == null) return;

        turretTransform.localPosition = Vector3.zero;
        turretTransform.localRotation = Quaternion.identity;

        Utils.DisableColliders(turretTransform.gameObject, keepScanNodeEnabled: true);

        Transform syncToHeadTransform = turretTransform.Find("SyncToHead");
        if (syncToHeadTransform == null) return;

        ParentToTransformBehaviour behaviour = enemyHeadTransform.gameObject.AddComponent<ParentToTransformBehaviour>();
        behaviour.SetTargetAndParent(syncToHeadTransform, enemyHeadTransform);
        behaviour.SetPositionOffset(new Vector3(0f, 0.425f, -0.02f));
        behaviour.SetRotationOffset(new Vector3(90f, 0f, 0f));

        logger.LogInfo("Spawned Toil-Head, Good luck :3");
    }
}
