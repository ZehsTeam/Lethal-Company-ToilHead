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

        EnemyAIPatch.Reset();
    }

    public void OnNewLevelLoaded()
    {
        // I will enabled this when Wesley's Asteroid13 moon is working in v50 (This includes LLL working in v50)
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
            logger.LogError("Error: Failed to set Toil-Head on server. EnemyAI is null.");
            return false;
        }

        NetworkObject enemyNetworkObject = enemyAI.GetComponent<NetworkObject>();

        if (!Utils.IsSpring(enemyAI))
        {
            logger.LogError($"Error: Failed to set Toil-Head on server. Enemy is not a Coil-Head. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
            return false;
        }

        if (Utils.IsToilHead(enemyAI))
        {
            logger.LogWarning($"Warning: Failed to set Toil-Head on server. Enemy is already a Toil-Head. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
            return false;
        }
        
        SpawnTurretOnServer(enemyAI.transform);
        PluginNetworkBehaviour.Instance.SetToilHeadClientRpc(enemyNetworkObject);
        SetToilHeadOnLocalClient(enemyNetworkObject);

        return true;
    }

    private NetworkObject SpawnTurretOnServer(Transform parentTransform)
    {
        if (!IsHostOrServer) return null;

        GameObject turretObject = Object.Instantiate(Content.turretPrefab, Vector3.zero, Quaternion.identity, parentTransform);
        NetworkObject enemyNetworkObject = turretObject.GetComponent<NetworkObject>();
        enemyNetworkObject.Spawn(destroyWithScene: true);
        turretObject.transform.SetParent(parentTransform);

        return enemyNetworkObject;
    }

    public void SetToilHeadOnLocalClient(NetworkObject enemyNetworkObject)
    {
        if (enemyNetworkObject == null)
        {
            logger.LogError($"Error: Failed to set Toil-Head on local client. Enemy NetworkObject is null.");
            return;
        }

        if (enemyNetworkObject.TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
        {
            if (!Utils.IsSpring(enemyAI))
            {
                logger.LogError($"Error: Failed to set Toil-Head on local client. Enemy \"{enemyNetworkObject.gameObject.name}\" is not a Coil-Head. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
                return;
            }

            if (EnemyAIPatch.enemyTurretPairs.ContainsKey(enemyNetworkObject))
            {
                logger.LogWarning($"Warning: Failed to set Toil-Head on local client. Enemy is already a Toil-Head. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
                return;
            }
        }
        else
        {
            logger.LogError($"Error: Failed to set Toil-Head on local client. Could not find EnemyAI. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
            return;
        }

        try
        {
            Transform turretTransform = enemyNetworkObject.transform.Find("ToilHeadTurretContainer(Clone)");
            Transform syncToHeadTransform = turretTransform.Find("SyncToHead");
            Transform enemyHeadTransform = enemyNetworkObject.transform.GetChild(0).Find("Head");

            turretTransform.localPosition = Vector3.zero;
            turretTransform.localRotation = Quaternion.identity;

            Utils.DisableColliders(turretTransform.gameObject, keepScanNodeEnabled: true);

            ParentToTransformBehaviour behaviour = enemyHeadTransform.gameObject.AddComponent<ParentToTransformBehaviour>();
            behaviour.SetTargetAndParent(syncToHeadTransform, enemyHeadTransform);
            behaviour.SetPositionOffset(new Vector3(0f, 0.425f, -0.02f));
            behaviour.SetRotationOffset(new Vector3(90f, 0f, 0f));

            EnemyAIPatch.AddEnemyTurretPair(enemyNetworkObject, turretTransform.GetComponent<NetworkObject>());
            EnemyAIPatch.spawnCount++;
        }
        catch (System.Exception e)
        {
            logger.LogError($"Error: Failed to set Toil-Head on local client. (NetworkObject: {enemyNetworkObject.NetworkObjectId})\n\n{e}");
        }
    }
}
