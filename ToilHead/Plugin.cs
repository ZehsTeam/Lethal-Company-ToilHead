using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.ToilHead.MonoBehaviours;
using com.github.zehsteam.ToilHead.Patches;
using GameNetcodeStuff;
using HarmonyLib;
using System.Collections;
using System.Linq;
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
        harmony.PatchAll(typeof(PlayerControllerBPatch));
        harmony.PatchAll(typeof(TurretPatch));

        ConfigManager = new SyncedConfigManager();

        SpawnDataManager.Initialize();
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
        PlayerControllerBPatch.Reset();
    }

    public void OnNewLevelLoaded()
    {
        Secret.SpawnSecrets();

        PlayerControllerBPatch.TrySpawnToilPlayersOnServer();
    }

    public void OnShipHasLeft()
    {
        EnemyAIPatch.DespawnAllTurretsOnServer();
        EnemyAIPatch.Reset();

        PlayerControllerBPatch.DespawnAllTurretsOnServer();
        PlayerControllerBPatch.Reset();

        ForceDespawnAllTurretHeadTurretsOnServer();
    }

    private void RegisterScrapItems()
    {
        try
        {
            ScrapHelper.RegisterScrap(Content.ToilHeadPlush, ConfigManager.ToilHeadPlushieSpawnWeight.Value, ConfigManager.ToilHeadPlushieSpawnAllMoons.Value, ConfigManager.ToilHeadPlushieMoonSpawnList.Value, twoHanded: false, ConfigManager.ToilHeadPlushieCarryWeight.Value, ConfigManager.ToilHeadPlushieMinValue.Value, ConfigManager.ToilHeadPlushieMaxValue.Value);
            ScrapHelper.RegisterScrap(Content.ToilSlayerPlush, ConfigManager.ToilSlayerPlushieSpawnWeight.Value, ConfigManager.ToilSlayerPlushieSpawnAllMoons.Value, ConfigManager.ToilSlayerPlushieMoonSpawnList.Value, twoHanded: false, ConfigManager.ToilSlayerPlushieCarryWeight.Value, ConfigManager.ToilSlayerPlushieMinValue.Value, ConfigManager.ToilSlayerPlushieMaxValue.Value);
        }
        catch (System.Exception e)
        {
            logger.LogWarning($"Warning: Failed to register scrap items.\n\n{e}");
        }
    }
    
    #region Toil-Head
    public bool SetToilHeadOnServer(EnemyAI enemyAI, bool isSlayer = false)
    {
        if (!IsHostOrServer) return false;

        string enemyName = isSlayer ? "Toil-Slayer" : "Toil-Head";

        if (enemyAI == null)
        {
            logger.LogError($"Error: Failed to set {enemyName} on server. EnemyAI is null.");
            return false;
        }

        NetworkObject enemyNetworkObject = enemyAI.GetComponent<NetworkObject>();

        if (!Utils.IsSpring(enemyAI))
        {
            logger.LogError($"Error: Failed to set {enemyName} on server. Enemy is not a Coil-Head. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
            return false;
        }

        if (Utils.IsToilHead(enemyAI))
        {
            logger.LogWarning($"Warning: Failed to set {enemyName} on server. Enemy is already a Toil-Head/Toil-Slayer. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
            return false;
        }

        SpawnTurretOnServer(enemyAI.transform, isSlayer);
        PluginNetworkBehaviour.Instance.SetToilHeadClientRpc(enemyNetworkObject, isSlayer);
        SetToilHeadOnLocalClient(enemyNetworkObject, isSlayer);

        LogInfoExtended($"Spawned {enemyName}. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");

        return true;
    }

    public void SetToilHeadOnLocalClient(NetworkObject enemyNetworkObject, bool isSlayer)
    {
        string enemyName = isSlayer ? "Toil-Slayer" : "Toil-Head";

        if (enemyNetworkObject == null)
        {
            logger.LogError($"Error: Failed to set {enemyName} on local client. Enemy NetworkObject is null.");
            return;
        }

        if (enemyNetworkObject.TryGetComponent(out EnemyAI enemyAI))
        {
            if (!Utils.IsSpring(enemyAI))
            {
                logger.LogError($"Error: Failed to set {enemyName} on local client. Enemy \"{enemyNetworkObject.gameObject.name}\" is not a Coil-Head. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
                return;
            }

            if (EnemyAIPatch.EnemyTurretPairs.ContainsKey(enemyAI))
            {
                logger.LogWarning($"Warning: Failed to set {enemyName} on local client. Enemy is already a Toil-Head/Toil-Slayer. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
                return;
            }
        }
        else
        {
            logger.LogError($"Error: Failed to set {enemyName} on local client. Could not find EnemyAI. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
            return;
        }

        try
        {
            enemyNetworkObject.GetComponentInChildren<ScanNodeProperties>().headerText = isSlayer ? "Toil-slayer" : "Toil-head";

            Transform turretTransform = enemyNetworkObject.transform.Find($"{GetTurretPrefabName(isSlayer)}(Clone)");
            ToilHeadTurretBehaviour turretScript = turretTransform.GetComponentInChildren<ToilHeadTurretBehaviour>();
            Transform syncToHeadTransform = turretTransform.Find("SyncToHead");
            Transform enemyHeadTransform = enemyNetworkObject.transform.GetChild(0).Find("Head");

            turretTransform.localPosition = Vector3.zero;
            turretTransform.localRotation = Quaternion.identity;

            Utils.DisableColliders(turretTransform.gameObject, keepScanNodeEnabled: true);

            ParentToTransformBehaviour behaviour = enemyHeadTransform.gameObject.AddComponent<ParentToTransformBehaviour>();
            behaviour.SetTargetAndParent(syncToHeadTransform, enemyHeadTransform);
            behaviour.SetPositionOffset(new Vector3(0f, 0.425f, -0.02f));
            behaviour.SetRotationOffset(new Vector3(90f, 0f, 0f));

            EnemyAIPatch.AddEnemyTurretPair(enemyAI, turretScript);

            if (isSlayer)
            {
                EnemyAIPatch.ToilSlayerSpawnCount++;
            }
            else
            {
                EnemyAIPatch.ToilHeadSpawnCount++;
            }

            LogInfoExtended($"Initialized {enemyName} on local client. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
        }
        catch (System.Exception e)
        {
            logger.LogError($"Error: Failed to set {enemyName} on local client. (NetworkObject: {enemyNetworkObject.NetworkObjectId})\n\n{e}");
        }
    }
    #endregion

    #region Manti-Toil
    public bool SetMantiToilOnServer(EnemyAI enemyAI, bool isSlayer = false)
    {
        if (!IsHostOrServer) return false;

        string enemyName = isSlayer ? "Manti-Slayer" : "Manti-Toil";

        if (enemyAI == null)
        {
            logger.LogError($"Error: Failed to set {enemyName} on server. EnemyAI is null.");
            return false;
        }

        NetworkObject enemyNetworkObject = enemyAI.GetComponent<NetworkObject>();

        if (!Utils.IsManticoil(enemyAI))
        {
            logger.LogError($"Error: Failed to set {enemyName} on server. Enemy is not a Manticoil. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
            return false;
        }

        if (Utils.IsMantiToil(enemyAI))
        {
            logger.LogWarning($"Warning: Failed to set {enemyName} on server. Enemy is already a Manti-Toil/Manti-Slayer. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
            return false;
        }

        SpawnTurretOnServer(enemyAI.transform, isSlayer);
        PluginNetworkBehaviour.Instance.SetMantiToilClientRpc(enemyNetworkObject, isSlayer);
        SetMantiToilOnLocalClient(enemyNetworkObject, isSlayer);

        LogInfoExtended($"Spawned {enemyName}. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");

        return true;
    }

    public void SetMantiToilOnLocalClient(NetworkObject enemyNetworkObject, bool isSlayer)
    {
        string enemyName = isSlayer ? "Manti-Slayer" : "Manti-Toil";

        if (enemyNetworkObject == null)
        {
            logger.LogError($"Error: Failed to set {enemyName} on local client. Enemy NetworkObject is null.");
            return;
        }

        if (enemyNetworkObject.TryGetComponent(out EnemyAI enemyAI))
        {
            if (!Utils.IsManticoil(enemyAI))
            {
                logger.LogError($"Error: Failed to set {enemyName} on local client. Enemy \"{enemyNetworkObject.gameObject.name}\" is not a Manticoil. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
                return;
            }

            if (EnemyAIPatch.EnemyTurretPairs.ContainsKey(enemyAI))
            {
                logger.LogWarning($"Warning: Failed to set {enemyName} on local client. Enemy is already a Manti-Toil/Manti-Slayer. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
                return;
            }
        }
        else
        {
            logger.LogError($"Error: Failed to set {enemyName} on local client. Could not find EnemyAI. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
            return;
        }

        try
        {
            enemyNetworkObject.GetComponentInChildren<ScanNodeProperties>().headerText = isSlayer ? "Manti-slayer" : "Manti-toil";

            Transform turretTransform = enemyNetworkObject.transform.Find($"{GetTurretPrefabName(isSlayer)}(Clone)");
            ToilHeadTurretBehaviour turretScript = turretTransform.GetComponentInChildren<ToilHeadTurretBehaviour>();
            Transform syncToHeadTransform = turretTransform.Find("SyncToHead");
            Transform enemyHeadTransform = enemyNetworkObject.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0);

            turretTransform.localScale = new Vector3(2.051871f, 2.051871f, 2.051871f);
            turretTransform.localPosition = Vector3.zero;
            turretTransform.localRotation = Quaternion.identity;

            Utils.DisableColliders(turretTransform.gameObject, keepScanNodeEnabled: true);

            ParentToTransformBehaviour behaviour = enemyHeadTransform.gameObject.AddComponent<ParentToTransformBehaviour>();
            behaviour.SetTargetAndParent(syncToHeadTransform, enemyHeadTransform);
            behaviour.SetPositionOffset(new Vector3(0f, 0.12f, -0.025f));
            behaviour.SetRotationOffset(new Vector3(0f, 0f, 0f));

            ToilHeadTurretBehaviour toilHeadTurretBehaviour = turretTransform.GetComponentInChildren<ToilHeadTurretBehaviour>();
            toilHeadTurretBehaviour.useMantiToilSettings = true;

            EnemyAIPatch.AddEnemyTurretPair(enemyAI, turretScript);

            if (isSlayer)
            {
                EnemyAIPatch.MantiSlayerSpawnCount++;
            }
            else
            {
                EnemyAIPatch.MantiToilSpawnCount++;
            }
            
            LogInfoExtended($"Initialized {enemyName} on local client. (NetworkObject: {enemyNetworkObject.NetworkObjectId})");
        }
        catch (System.Exception e)
        {
            logger.LogError($"Error: Failed to set {enemyName} on local client. (NetworkObject: {enemyNetworkObject.NetworkObjectId})\n\n{e}");
        }
    }
    #endregion

    #region Toil-Player
    public bool SetToilPlayerOnServer(PlayerControllerB playerScript, bool isSlayer = false)
    {
        if (!IsHostOrServer) return false;

        if (playerScript == null)
        {
            logger.LogError("Error: Failed to set Toil-Player on server. PlayerControllerB is null.");
            return false;
        }

        if (playerScript.isPlayerDead)
        {
            logger.LogError($"Error: Failed to set Toil-Player on server. Player is already dead. (Player: {playerScript.playerUsername})");
            return false;
        }

        NetworkObject playerNetworkObject = playerScript.GetComponent<NetworkObject>();

        if (Utils.IsToilPlayer(playerScript))
        {
            logger.LogWarning($"Warning: Failed to set Toil-Player on server. Player is already a Toil-Player. (Player: {playerScript.playerUsername})");
            return false;
        }

        SpawnTurretOnServer(playerScript.transform, isSlayer);
        PluginNetworkBehaviour.Instance.SetToilPlayerClientRpc(playerNetworkObject, isSlayer);
        SetToilPlayerOnLocalClient(playerNetworkObject, isSlayer);

        LogInfoExtended($"Spawned Toil-Player (Player: {playerScript.playerUsername})");

        return true;
    }

    public void SetToilPlayerOnLocalClient(NetworkObject playerNetworkObject, bool isSlayer)
    {
        if (playerNetworkObject == null)
        {
            logger.LogError($"Error: Failed to set Toil-Player on local client. Player NetworkObject is null.");
            return;
        }

        if (playerNetworkObject.TryGetComponent(out PlayerControllerB playerScript))
        {
            if (PlayerControllerBPatch.PlayerTurretPairs.ContainsKey(playerScript))
            {
                logger.LogWarning($"Warning: Failed to set Toil-Player on local client. Player is already a Toil-Player. (Player: {playerScript.playerUsername})");
                return;
            }
        }
        else
        {
            logger.LogError($"Error: Failed to set Toil-Player on local client. Could not find PlayerControllerB. (NetworkObject: {playerNetworkObject.NetworkObjectId})");
            return;
        }

        try
        {
            Transform turretTransform = playerNetworkObject.transform.Find($"{GetTurretPrefabName(isSlayer)}(Clone)");
            ToilHeadTurretBehaviour turretScript = turretTransform.GetComponentInChildren<ToilHeadTurretBehaviour>();
            Transform syncToHeadTransform = turretTransform.Find("SyncToHead");
            Transform playerHeadTransform = playerNetworkObject.transform.Find("ScavengerModel").Find("metarig").GetChild(0).GetChild(0).GetChild(0).GetChild(0).Find("spine.004").Find("HeadPoint");

            turretTransform.localPosition = Vector3.zero;
            turretTransform.localRotation = Quaternion.identity;

            Utils.DisableColliders(turretTransform.gameObject, keepScanNodeEnabled: true);

            if (PlayerUtils.IsLocalPlayer(playerScript))
            {
                Utils.DisableRenderers(turretTransform.gameObject);
            }

            ParentToTransformBehaviour behaviour = playerHeadTransform.gameObject.AddComponent<ParentToTransformBehaviour>();
            behaviour.SetTargetAndParent(syncToHeadTransform, playerHeadTransform);
            behaviour.SetPositionOffset(new Vector3(0f, 0.375f, 0f));
            behaviour.SetRotationOffset(new Vector3(0f, 0f, 0f));

            PlayerControllerBPatch.AddPlayerTurretPair(playerScript, turretScript);
            PlayerControllerBPatch.ToilPlayerSpawnCount++;

            LogInfoExtended($"Initialized Toil-Player on local client. isSlayer? {isSlayer} (Player: {playerScript.playerUsername})");
        }
        catch (System.Exception e)
        {
            logger.LogError($"Error: Failed to set Toil-Player on local client. (Player: {playerScript.playerUsername})\n\n{e}");
        }
    }
    #endregion

    #region Toiled Player Ragdoll
    public void SetToilHeadPlayerRagdoll(PlayerControllerB playerScript, bool isSlayer = false)
    {
        SetToilHeadPlayerRagdoll((int)playerScript.playerClientId, isSlayer);
    }

    public void SetToilHeadPlayerRagdoll(int playerId, bool isSlayer = false)
    {
        if (!IsHostOrServer)
        {
            PluginNetworkBehaviour.Instance.SetToilHeadPlayerRagdollServerRpc(playerId, isSlayer);
            return;
        }

        if (!ConfigManager.SpawnToiledPlayerRagdolls.Value) return;

        StartOfRound.Instance.StartCoroutine(SetToilHeadPlayerRagdollOnServer(playerId, isSlayer));
    }

    private IEnumerator SetToilHeadPlayerRagdollOnServer(int playerId, bool isSlayer = false)
    {
        if (!IsHostOrServer) yield break;

        PlayerControllerB playerScript = PlayerUtils.GetPlayerScript(playerId);

        if (playerScript == null)
        {
            logger.LogError("Error: Failed to set Toiled player ragdoll on server. Could not find PlayerControllerB.");
            yield break;
        }

        yield return Utils.WaitUntil(() =>
        {
            return playerScript.deadBody != null;
        });

        if (playerScript.deadBody == null)
        {
            logger.LogError("Error: Failed to set Toiled player ragdoll on server. Could not find DeadBodyInfo.");
            yield break;
        }

        GameObject ragdollObject = playerScript.deadBody.gameObject;

        if (ragdollObject.name != "PlayerRagdollSpring Variant(Clone)")
        {
            logger.LogError("Error: Failed to set Toiled player ragdoll on server. Player ragdoll is not of type \"PlayerRagdollSpring Variant\".");
            yield break;
        }

        yield return Utils.WaitUntil(() =>
        {
            return ragdollObject.GetComponentInChildren<NetworkObject>() != null;
        });

        NetworkObject ragdollNetworkObject = ragdollObject.GetComponentInChildren<NetworkObject>();

        if (ragdollNetworkObject == null)
        {
            logger.LogError($"Error: Failed to set Toiled player ragdoll on server. Could not find NetworkObject.");
            yield break;
        }

        if (Utils.IsToilHeadPlayerRagdoll(ragdollNetworkObject.gameObject))
        {
            logger.LogWarning($"Warning: Failed to set Toiled player ragdoll on server. Player ragdoll is already a Toiled player ragdoll. (NetworkObject: {ragdollNetworkObject.NetworkObjectId})");
            yield break;
        }

        bool realTurret = ConfigManager.SpawnRealToiledPlayerRagdolls.Value;

        if (realTurret)
        {
            SpawnTurretOnServer(ragdollNetworkObject.transform, isSlayer);
        }

        SetToilHeadPlayerRagdollOnLocalClient(ragdollNetworkObject, realTurret, isSlayer);
        PluginNetworkBehaviour.Instance.SetToilHeadPlayerRagdollClientRpc(ragdollNetworkObject, realTurret, isSlayer);

        LogInfoExtended($"Spawned Toiled player ragdoll. isSlayer? {isSlayer} (NetworkObject: {ragdollNetworkObject.NetworkObjectId})");
    }

    public void SetToilHeadPlayerRagdollOnLocalClient(NetworkObject ragdollNetworkObject, bool realTurret, bool isSlayer)
    {
        if (ragdollNetworkObject == null)
        {
            logger.LogError($"Error: Failed to set Toiled player ragdoll on local client. Player ragdoll NetworkObject is null.");
            return;
        }

        if (realTurret)
        {
            SetRealToilHeadPlayerRagdollOnLocalClient(ragdollNetworkObject, isSlayer);
        }
        else
        {
            SetFakeToilHeadPlayerRagdollOnLocalClient(ragdollNetworkObject, isSlayer);
        }
    }

    private void SetRealToilHeadPlayerRagdollOnLocalClient(NetworkObject ragdollNetworkObject, bool isSlayer)
    {
        try
        {
            Transform turretTransform = ragdollNetworkObject.transform.GetChild(0);
            ToilHeadTurretBehaviour turretScript = turretTransform.GetComponentInChildren<ToilHeadTurretBehaviour>();
            Transform syncToHeadTransform = turretTransform.Find("SyncToHead");
            Transform springTransform = ragdollNetworkObject.transform.parent.GetChild(0).Find("spine.004").Find("SpringContainer").Find("SpringMetarig").GetChild(0).GetChild(0).GetChild(0).GetChild(0);

            turretTransform.localPosition = Vector3.zero;
            turretTransform.localRotation = Quaternion.identity;
            turretTransform.localScale = new Vector3(1.949195f, 1.949194f, 1.949195f);

            ParentToTransformBehaviour behaviour = springTransform.gameObject.AddComponent<ParentToTransformBehaviour>();
            behaviour.SetTargetAndParent(syncToHeadTransform, springTransform);
            behaviour.SetPositionOffset(new Vector3(0f, 0.0213f, 0.006f));
            behaviour.SetRotationOffset(new Vector3(0f, 90f, 0f));

            PlayerControllerBPatch.AddDeadBodyTurret(turretScript);

            LogInfoExtended($"Initialized real Toiled player ragdoll on local client. isSlayer? {isSlayer} (NetworkObject: {ragdollNetworkObject.NetworkObjectId})");
        }
        catch (System.Exception e)
        {
            logger.LogError($"Error: Failed to set Toiled player ragdoll on local client. (NetworkObject: {ragdollNetworkObject.NetworkObjectId})\n\n{e}");
        }
    }

    private void SetFakeToilHeadPlayerRagdollOnLocalClient(NetworkObject ragdollNetworkObject, bool isSlayer)
    {
        try
        {
            GameObject turretPropPrefab = isSlayer ? Content.MinigunTurretPropPrefab : Content.TurretPropPrefab;
            Transform turretTransform = Object.Instantiate(turretPropPrefab, Vector3.zero, Quaternion.identity, ragdollNetworkObject.transform).transform;
            Transform springTransform = ragdollNetworkObject.transform.parent.GetChild(0).Find("spine.004").Find("SpringContainer").Find("SpringMetarig").GetChild(0).GetChild(0).GetChild(0).GetChild(0);

            turretTransform.localPosition = Vector3.zero;
            turretTransform.localRotation = Quaternion.identity;
            turretTransform.localScale = new Vector3(1.949195f, 1.949194f, 1.949195f);

            ParentToTransformBehaviour behaviour = springTransform.gameObject.AddComponent<ParentToTransformBehaviour>();
            behaviour.SetTargetAndParent(turretTransform, springTransform);
            behaviour.SetPositionOffset(new Vector3(0f, 0.0213f, 0.006f));
            behaviour.SetRotationOffset(new Vector3(0f, 90f, 0f));

            LogInfoExtended($"Initialized fake Toiled player ragdoll on local client. isSlayer? {isSlayer} (NetworkObject: {ragdollNetworkObject.NetworkObjectId})");
        }
        catch (System.Exception e)
        {
            logger.LogError($"Error: Failed to set Toiled player ragdoll on local client. (NetworkObject: {ragdollNetworkObject.NetworkObjectId})\n\n{e}");
        }
    }
    #endregion

    private NetworkObject SpawnTurretOnServer(Transform parentTransform, bool isSlayer)
    {
        if (!IsHostOrServer) return null;

        GameObject turretPrefab = isSlayer ? Content.MinigunTurretPrefab : Content.TurretPrefab;
        GameObject turretObject = Object.Instantiate(turretPrefab, Vector3.zero, Quaternion.identity, parentTransform);
        NetworkObject enemyNetworkObject = turretObject.GetComponent<NetworkObject>();
        enemyNetworkObject.Spawn(destroyWithScene: true);
        turretObject.transform.SetParent(parentTransform);

        return enemyNetworkObject;
    }

    private string GetTurretPrefabName(bool isSlayer)
    {
        return isSlayer ? Content.MinigunTurretPrefab.name : Content.TurretPrefab.name;
    }

    private void ForceDespawnAllTurretHeadTurretsOnServer()
    {
        if (!IsHostOrServer) return;

        try
        {
            foreach (var obj in FindObjectsByType<ToilHeadTurretBehaviour>(FindObjectsSortMode.None).Select(_ => _.transform.parent))
            {
                if (obj.TryGetComponent(out NetworkObject turretNetworkObject))
                {
                    if (!turretNetworkObject.IsSpawned) return;

                    turretNetworkObject.Despawn();

                    LogInfoExtended("Force despawned Turret-Head turret.");
                }
                else
                {
                    logger.LogError($"Error: Failed to force despawn Turret-Head turret. NetworkObject is null.");
                }
            }
        }
        catch (System.Exception e)
        {
            logger.LogError($"Error: Failed to force despawn all Turret-Head turrets.\n\n{e}");
        }
    }

    public void LogInfoExtended(object data)
    {
        if (ConfigManager.ExtendedLogging.Value)
        {
            logger.LogInfo(data);
        }
    }

    public void LogWarningExtended(object data)
    {
        if (ConfigManager.ExtendedLogging.Value)
        {
            logger.LogWarning(data);
        }
    }
}
