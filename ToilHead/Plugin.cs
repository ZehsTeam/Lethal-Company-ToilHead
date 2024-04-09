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
public class Plugin : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static Plugin Instance;
    internal static ManualLogSource logger;

    internal ConfigManager ConfigManager;

    public static bool IsHostOrServer => NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer;

    void Awake()
    {
        if (Instance == null) Instance = this;

        logger = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        harmony.PatchAll(typeof(GameNetworkManagerPatch));
        harmony.PatchAll(typeof(StartOfRoundPatch));
        harmony.PatchAll(typeof(RoundManagerPatch));
        harmony.PatchAll(typeof(EnemyAIPatch));

        ConfigManager = new ConfigManager();

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

    public void OnNewLevelLoaded(int randomSeed)
    {
        Secret.SpawnSecrets();
    }

    public void OnShipHasLeft()
    {
        EnemyAIPatch.DespawnAllTurrets();
        EnemyAIPatch.Reset();
    }

    public void SetToilHeadOnServer(EnemyAI enemyAI)
    {
        if (!IsHostOrServer) return;

        if (enemyAI == null)
        {
            logger.LogError("Error: failed to set Toil-Head on server. Enemy could not be found.");
            return;
        }

        if (Content.turretPrefab == null)
        {
            logger.LogError("Error: failed to set Toil-Head on server. Turret prefab could not be found.");
            return;
        }

        float forwardWeight = ConfigManager.SpawnTurretFacingForwardWeight * 100f;
        float backwardWeight = ConfigManager.SpawnTurretFacingBackwardWeight * 100f;
        bool turretFacingForward = Random.Range(1f, forwardWeight + backwardWeight) <= forwardWeight;
        bool hideTurretBody = ConfigManager.HideTurretBody;

        if (hideTurretBody)
        {
            turretFacingForward = true;
        }

        NetworkObject enemyNetworkObject = enemyAI.gameObject.GetComponent<NetworkObject>();
        NetworkObject turretNetworkObject = SpawnTurretOnServer(enemyAI.gameObject.transform);
        EnemyAIPatch.AddEnemyTurretPair(enemyNetworkObject, turretNetworkObject);

        PluginNetworkBehaviour.Instance.SetToilHeadClientRpc(NetworkUtils.GetNetworkObjectId(enemyNetworkObject), turretFacingForward, hideTurretBody);
        SetToilHeadOnLocalClient(enemyAI.gameObject, turretFacingForward, hideTurretBody);
    }

    private NetworkObject SpawnTurretOnServer(Transform parent)
    {
        if (!IsHostOrServer) return null;

        GameObject turretObject = Object.Instantiate(Content.turretPrefab, parent.localPosition, Quaternion.identity, parent);

        NetworkObject turretNetworkObject = turretObject.GetComponent<NetworkObject>();
        turretNetworkObject.Spawn(destroyWithScene: true);

        turretObject.transform.SetParent(parent);

        return turretNetworkObject;
    }

    public void SetToilHeadOnLocalClient(GameObject enemyObject, bool turretFacingForward, bool hideTurretBody)
    {
        Transform head = enemyObject.transform.GetChild(0).Find("Head");

        Transform turretTransform = enemyObject.transform.GetChild(1);
        GameObject mountObject = turretTransform.transform.GetChild(1).GetChild(0).gameObject;

        ParentToTransformBehaviour behaviour = head.gameObject.AddComponent<ParentToTransformBehaviour>();
        behaviour.SetTargetAndParent(turretTransform, head);

        if (hideTurretBody)
        {
            mountObject.SetActive(false);
            behaviour.SetPositionOffset(new Vector3(0f, -0.9f, -0.05f));
        }

        if (turretFacingForward)
        {
            behaviour.SetRotationOffset(new Vector3(90f, 0f, 0f));
        }
        else
        {
            behaviour.SetRotationOffset(new Vector3(-20f, 180f, 0f));
        }

        Utils.DisableColliders(turretTransform.gameObject, keepScanNodeEnabled: true);

        logger.LogInfo("Spawned Toil-Head, Good luck :3");
    }
}
