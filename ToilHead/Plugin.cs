using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.ToilHead.MonoBehaviours;
using com.github.zehsteam.ToilHead.Patches;
using GameNetcodeStuff;
using HarmonyLib;
using System.Collections;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(LethalLib.Plugin.ModGUID, BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(ScrapHelper.MonsterPlushiesGUID, BepInDependency.DependencyFlags.SoftDependency)]
internal class Plugin : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static Plugin Instance;
    internal static ManualLogSource logger;

    internal static SyncedConfigManager ConfigManager;

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
        harmony.PatchAll(typeof(SpringManAIPatch));

        ConfigManager = new SyncedConfigManager();

        ToilHeadDataManager.Initialize();
        Content.Load();
        EnemyAIPatch.Reset();

        RegisterScrapItems();
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
        Secret.SpawnSecrets();
    }

    public void OnShipHasLeft()
    {
        EnemyAIPatch.DespawnAllTurrets();
        EnemyAIPatch.Reset();
    }

    private void RegisterScrapItems()
    {
        int iRarity = ConfigManager.PlushieSpawnWeight;
        bool spawnAllMoons = ConfigManager.PlushieSpawnAllMoons;
        string moonSpawnList = ConfigManager.PlushieMoonSpawnList;
        int carryWeight = ConfigManager.PlushieCarryWeight;
        int minValue = ConfigManager.PlushieMinValue;
        int maxValue = ConfigManager.PlushieMaxValue;

        try
        {
            ScrapHelper.RegisterScrap(Content.toilHeadPlush, iRarity, spawnAllMoons, moonSpawnList, twoHanded: false, carryWeight,  minValue, maxValue);
        }
        catch (System.Exception e)
        {
            logger.LogWarning($"Warning: Failed to register scrap items.\n\n{e}");
        }
    }
    
    #region Coil-Head Enemy
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

        LogInfoExtended($"Spawned Toil-Head. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");

        return true;
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
    #endregion

    #region Player Ragdoll
    public void SetToilHeadPlayerRagdoll(PlayerControllerB playerScript)
    {
        SetToilHeadPlayerRagdoll((int)playerScript.playerClientId);
    }

    public void SetToilHeadPlayerRagdoll(int playerId)
    {
        if (!IsHostOrServer)
        {
            PluginNetworkBehaviour.Instance.SetToilHeadPlayerRagdollServerRpc(playerId);
            return;
        }

        if (!ConfigManager.SpawnToilHeadPlayerRagdolls) return;

        StartOfRound.Instance.StartCoroutine(SetToilHeadPlayerRagdollOnServer(playerId));
    }

    private IEnumerator SetToilHeadPlayerRagdollOnServer(int playerId)
    {
        if (!IsHostOrServer) yield break;

        PlayerControllerB playerScript = Utils.GetPlayerScript(playerId);

        if (playerScript == null)
        {
            logger.LogError("Error: Failed to set Toil-Head on player ragdoll on server. Could not find PlayerControllerB.");
            yield break;
        }

        yield return Utils.WaitUntil(() =>
        {
            return playerScript.deadBody != null;
        });

        if (playerScript.deadBody == null)
        {
            logger.LogError("Error: Failed to set Toil-Head on player ragdoll on server. Could not find DeadBodyInfo.");
            yield break;
        }

        GameObject ragdollObject = playerScript.deadBody.gameObject;

        if (ragdollObject.name != "PlayerRagdollSpring Variant(Clone)")
        {
            logger.LogError("Error: Failed to set Toil-Head on player ragdoll on server. Player ragdoll is not of type \"PlayerRagdollSpring Variant\".");
            yield break;
        }

        yield return Utils.WaitUntil(() =>
        {
            return ragdollObject.GetComponentInChildren<NetworkObject>() != null;
        });

        NetworkObject ragdollNetworkObject = ragdollObject.GetComponentInChildren<NetworkObject>();

        if (ragdollNetworkObject == null)
        {
            logger.LogError($"Error: Failed to set Toil-Head on player ragdoll on server. Could not find NetworkObject.");
            yield break;
        }

        if (Utils.IsToilHeadPlayerRagdoll(ragdollNetworkObject.gameObject))
        {
            logger.LogWarning($"Warning: Failed to set Toil-Head on player ragdoll on server. Player ragdoll is already a Toil-Head player ragdoll. (NetworkObject: {ragdollNetworkObject.NetworkObjectId})");
            yield break;
        }

        bool realTurret = ConfigManager.RealToilHeadPlayerRagdolls;

        if (realTurret)
        {
            SpawnTurretOnServer(ragdollNetworkObject.transform);
        }
        
        SetToilHeadPlayerRagdollOnLocalClient(ragdollNetworkObject, realTurret);
        PluginNetworkBehaviour.Instance.SetToilHeadPlayerRagdollClientRpc(ragdollNetworkObject, realTurret);

        LogInfoExtended($"Spawned Toil-Head on player ragdoll. (NetworkObject: {ragdollNetworkObject.NetworkObjectId})");
    }

    public void SetToilHeadPlayerRagdollOnLocalClient(NetworkObject ragdollNetworkObject, bool realTurret)
    {
        if (ragdollNetworkObject == null)
        {
            logger.LogError($"Error: Failed to set Toil-Head on player ragdoll on local client. Player ragdoll NetworkObject is null.");
            return;
        }

        if (realTurret)
        {
            SetRealToilHeadPlayerRagdollOnLocalClient(ragdollNetworkObject);
        }
        else
        {
            SetFakeToilHeadPlayerRagdollOnLocalClient(ragdollNetworkObject);
        }
    }

    private void SetRealToilHeadPlayerRagdollOnLocalClient(NetworkObject ragdollNetworkObject)
    {
        try
        {
            Transform turretTransform = ragdollNetworkObject.transform.GetChild(0);
            Transform syncToHeadTransform = turretTransform.Find("SyncToHead");
            Transform springTransform = ragdollNetworkObject.transform.parent.GetChild(0).Find("spine.004").Find("SpringContainer").Find("SpringMetarig").GetChild(0).GetChild(0).GetChild(0).GetChild(0);

            turretTransform.localPosition = Vector3.zero;
            turretTransform.localRotation = Quaternion.identity;
            turretTransform.localScale = new Vector3(1.949195f, 1.949194f, 1.949195f);

            ParentToTransformBehaviour behaviour = springTransform.gameObject.AddComponent<ParentToTransformBehaviour>();
            behaviour.SetTargetAndParent(syncToHeadTransform, springTransform);
            behaviour.SetPositionOffset(new Vector3(0f, 0.0213f, 0.006f));
            behaviour.SetRotationOffset(new Vector3(0f, 90f, 0f));

            EnemyAIPatch.AddEnemyTurretPair(ragdollNetworkObject, turretTransform.GetComponent<NetworkObject>());
        }
        catch (System.Exception e)
        {
            logger.LogError($"Error: Failed to set Toil-Head on player ragdoll on local client. (NetworkObject: {ragdollNetworkObject.NetworkObjectId})\n\n{e}");
        }
    }

    private void SetFakeToilHeadPlayerRagdollOnLocalClient(NetworkObject ragdollNetworkObject)
    {
        try
        {
            Transform turretTransform = Object.Instantiate(Content.turretPropPrefab, Vector3.zero, Quaternion.identity, ragdollNetworkObject.transform).transform;
            Transform springTransform = ragdollNetworkObject.transform.parent.GetChild(0).Find("spine.004").Find("SpringContainer").Find("SpringMetarig").GetChild(0).GetChild(0).GetChild(0).GetChild(0);

            turretTransform.localPosition = Vector3.zero;
            turretTransform.localRotation = Quaternion.identity;
            turretTransform.localScale = new Vector3(1.949195f, 1.949194f, 1.949195f);

            ParentToTransformBehaviour behaviour = springTransform.gameObject.AddComponent<ParentToTransformBehaviour>();
            behaviour.SetTargetAndParent(turretTransform, springTransform);
            behaviour.SetPositionOffset(new Vector3(0f, 0.0213f, 0.006f));
            behaviour.SetRotationOffset(new Vector3(0f, 90f, 0f));
        }
        catch (System.Exception e)
        {
            logger.LogError($"Error: Failed to set Toil-Head on player ragdoll on local client. (NetworkObject: {ragdollNetworkObject.NetworkObjectId})\n\n{e}");
        }
    }
    #endregion

    private NetworkObject SpawnTurretOnServer(Transform parentTransform)
    {
        if (!IsHostOrServer) return null;

        GameObject turretObject = Object.Instantiate(Content.turretPrefab, Vector3.zero, Quaternion.identity, parentTransform);
        NetworkObject enemyNetworkObject = turretObject.GetComponent<NetworkObject>();
        enemyNetworkObject.Spawn(destroyWithScene: true);
        turretObject.transform.SetParent(parentTransform);

        return enemyNetworkObject;
    }

    public void LogInfoExtended(object data)
    {
        if (ConfigManager.ExtendedLogging)
        {
            logger.LogInfo(data);
        }
    }

    public void LogWarningExtended(object data)
    {
        if (ConfigManager.ExtendedLogging)
        {
            logger.LogWarning(data);
        }
    }
}
