using com.github.zehsteam.ToilHead.MonoBehaviours;
using com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;
using GameNetcodeStuff;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class TurretHeadManager
{
    public static List<TurretHeadData> TurretHeadDataList { get; private set; } = [];
    public static TurretHeadData PlayerTurretHeadData { get; private set; }

    public static Dictionary<EnemyAI, TurretHeadControllerBehaviour> EnemyTurretHeadControllerPairs { get; private set; } = [];
    public static Dictionary<PlayerControllerB, TurretHeadControllerBehaviour> PlayerTurretHeadControllerPairs { get; private set; } = [];
    public static Dictionary<PlayerControllerB, TurretHeadControllerBehaviour> DeadBodyTurretHeadControllerPairs { get; private set; } = [];

    private static Coroutine _setDeadBodyTurretHeadOnServerCoroutine;

    internal static void Initialize()
    {
        TurretHeadDataList = [
            new TurretHeadData(enemyName: "Spring",    isSlayer: false, controllerPrefab: Content.ToilHeadControllerPrefab,     new MoonSpawnDataList(Plugin.ConfigManager.ToilHeadSpawnSettingsMoonList.Value,     defaultSpawnData: new SpawnData(Plugin.ConfigManager.ToilHeadDefaultSpawnSettings.Value)),     toilationSpawnData: new SpawnData(Plugin.ConfigManager.ToilationToilHeadSpawnSettings.Value)),
            new TurretHeadData(enemyName: "Spring",    isSlayer: true,  controllerPrefab: Content.ToilSlayerControllerPrefab,   new MoonSpawnDataList(Plugin.ConfigManager.ToilSlayerSpawnSettingsMoonList.Value,   defaultSpawnData: new SpawnData(Plugin.ConfigManager.ToilSlayerDefaultSpawnSettings.Value)),   toilationSpawnData: new SpawnData(Plugin.ConfigManager.ToilationToilSlayerSpawnSettings.Value)),
            new TurretHeadData(enemyName: "Manticoil", isSlayer: false, controllerPrefab: Content.MantiToilControllerPrefab,    new MoonSpawnDataList(Plugin.ConfigManager.MantiToilSpawnSettingsMoonList.Value,    defaultSpawnData: new SpawnData(Plugin.ConfigManager.MantiToilDefaultSpawnSettings.Value)),    toilationSpawnData: new SpawnData(Plugin.ConfigManager.ToilationMantiToilSpawnSettings.Value)),
            new TurretHeadData(enemyName: "Manticoil", isSlayer: true,  controllerPrefab: Content.MantiSlayerControllerPrefab,  new MoonSpawnDataList(Plugin.ConfigManager.MantiSlayerSpawnSettingsMoonList.Value,  defaultSpawnData: new SpawnData(Plugin.ConfigManager.MantiSlayerDefaultSpawnSettings.Value)),  toilationSpawnData: new SpawnData(Plugin.ConfigManager.ToilationMantiSlayerSpawnSettings.Value)),
            new TurretHeadData(enemyName: "Masked",    isSlayer: false, controllerPrefab: Content.ToilMaskedControllerPrefab,   new MoonSpawnDataList(Plugin.ConfigManager.ToilMaskedSpawnSettingsMoonList.Value,   defaultSpawnData: new SpawnData(Plugin.ConfigManager.ToilMaskedDefaultSpawnSettings.Value)),   toilationSpawnData: new SpawnData(Plugin.ConfigManager.ToilationToilMaskedSpawnSettings.Value)),
            new TurretHeadData(enemyName: "Masked",    isSlayer: true,  controllerPrefab: Content.SlayerMaskedControllerPrefab, new MoonSpawnDataList(Plugin.ConfigManager.SlayerMaskedSpawnSettingsMoonList.Value, defaultSpawnData: new SpawnData(Plugin.ConfigManager.SlayerMaskedDefaultSpawnSettings.Value)), toilationSpawnData: new SpawnData(Plugin.ConfigManager.ToilationSlayerMaskedSpawnSettings.Value)),
        ];

        PlayerTurretHeadData = new TurretHeadData(string.Empty, false, null, new MoonSpawnDataList(Plugin.ConfigManager.ToilPlayerSpawnSettingsMoonList.Value, defaultSpawnData: new SpawnData(Plugin.ConfigManager.ToilPlayerDefaultSpawnSettings.Value)), toilationSpawnData: new SpawnData(Plugin.ConfigManager.ToilationToilPlayerSpawnSettings.Value));

        EnemyTurretHeadControllerPairs = [];
        PlayerTurretHeadControllerPairs = [];
        DeadBodyTurretHeadControllerPairs = [];

        _setDeadBodyTurretHeadOnServerCoroutine = null;
    }

    internal static void Reset()
    {
        TurretHeadDataList.ForEach(_ => _.Reset());
        PlayerTurretHeadData.Reset();

        EnemyTurretHeadControllerPairs.Clear();
        PlayerTurretHeadControllerPairs.Clear();
        DeadBodyTurretHeadControllerPairs.Clear();

        _setDeadBodyTurretHeadOnServerCoroutine = null;

        DespawnAllControllersOnServer();
    }

    #region Try Set Turret-Head
    internal static bool TrySetEnemyTurretHeadOnServer(EnemyAI enemyScript, bool isSlayer)
    {
        if (!Plugin.IsHostOrServer) return false;

        string enemyName = enemyScript.enemyType.enemyName;
        TurretHeadData turretHeadData = GetEnemyTurretHeadData(enemyName, isSlayer);

        if (turretHeadData == null)
        {
            Plugin.logger.LogError($"Error: Failed to try set \"{enemyName}\" Turret-Head on server. TurretHeadData is null.");
            return false;
        }

        SpawnData spawnData = turretHeadData.GetSpawnDataForCurrentMoon();
        int maxSpawnCount = spawnData.MaxSpawnCount;

        if (turretHeadData.ForceMaxSpawnCount > -1)
        {
            maxSpawnCount = turretHeadData.ForceMaxSpawnCount;
        }

        if (!turretHeadData.ForceSpawns)
        {
            if (turretHeadData.SpawnCount >= maxSpawnCount) return false;
            if (!Utils.RandomPercent(spawnData.SpawnChance)) return false;
        }

        return SetEnemyTurretHeadOnServer(enemyScript, isSlayer);
    }

    internal static void TrySetPlayerTurretHeadsOnServer()
    {
        if (!Plugin.IsHostOrServer) return;
        if (!StartOfRound.Instance.currentLevel.spawnEnemiesAndScrap) return;
        if (GameNetworkManager.Instance.connectedPlayers == 1) return;

        List<PlayerControllerB> playerScripts = StartOfRound.Instance.allPlayerScripts.ToList();

        for (int i = playerScripts.Count - 1; i >= 0; i--)
        {
            int index = Random.Range(0, i);
            bool isSlayer = Utils.RandomPercent(Plugin.ConfigManager.ToilPlayerSlayerChance.Value);

            PlayerControllerB playerScript = playerScripts[index];
            if (!playerScript.gameObject.activeSelf) continue;
            if (!playerScript.isPlayerControlled) continue;

            TrySetPlayerTurretHeadOnServer(playerScript, isSlayer);

            playerScripts.RemoveAt(index);
        }
    }

    internal static bool TrySetPlayerTurretHeadOnServer(PlayerControllerB playerScript, bool isSlayer)
    {
        if (!Plugin.IsHostOrServer) return false;

        TurretHeadData turretHeadData = PlayerTurretHeadData;

        SpawnData spawnData = turretHeadData.GetSpawnDataForCurrentMoon();
        int maxSpawnCount = spawnData.MaxSpawnCount;

        if (turretHeadData.ForceMaxSpawnCount > -1)
        {
            maxSpawnCount = turretHeadData.ForceMaxSpawnCount;
        }

        if (!turretHeadData.ForceSpawns)
        {
            if (turretHeadData.SpawnCount >= maxSpawnCount) return false;
            if (!Utils.RandomPercent(spawnData.SpawnChance)) return false;
        }

        return SetPlayerTurretHeadOnServer(playerScript, isSlayer);
    }
    #endregion

    #region Set Turret-Head
    public static bool SetEnemyTurretHeadOnServer(EnemyAI enemyScript, bool isSlayer)
    {
        if (!Plugin.IsHostOrServer) return false;

        if (enemyScript == null)
        {
            Plugin.logger.LogError($"Error: Failed to set enemy Turret-Head (isSlayer? {isSlayer}) on server. EnemyAI is null.");
            return false;
        }

        string enemyName = enemyScript.enemyType.enemyName;

        if (enemyScript.isEnemyDead)
        {
            Plugin.logger.LogError($"Error: Failed to set enemy \"{enemyName}\" Turret-Head (isSlayer? {isSlayer}) on server. Enemy is already dead.");
            return false;
        }

        if (Utils.IsTurretHead(enemyScript))
        {
            Plugin.logger.LogError($"Error: Failed to set enemy \"{enemyName}\" Turret-Head (isSlayer? {isSlayer}) on server. Enemy is already a Turret-Head.");
            return false;
        }

        TurretHeadData turretHeadData = GetEnemyTurretHeadData(enemyName, isSlayer);

        if (turretHeadData == null)
        {
            Plugin.logger.LogError($"Error: Failed to set enemy \"{enemyName}\" Turret-Head (isSlayer? {isSlayer}) on server. TurretHeadData is null.");
            return false;
        }

        SpawnTurretHeadControllerOnServer(turretHeadData.ControllerPrefab, enemyScript.transform);

        Plugin.Instance.LogInfoExtended($"Set enemy \"{enemyName}\" Turret-Head (isSlayer? {isSlayer}) on server.");

        return true;
    }

    public static bool SetPlayerTurretHeadOnServer(PlayerControllerB playerScript, bool isSlayer)
    {
        if (!Plugin.IsHostOrServer) return false;

        string playerUsername = playerScript.playerUsername;

        if (playerScript == null)
        {
            Plugin.logger.LogError($"Error: Failed to set player \"{playerUsername}\" Turret-Head (isSlayer? {isSlayer}) on server. PlayerControllerB is null.");
            return false;
        }

        if (playerScript.isPlayerDead)
        {
            Plugin.logger.LogError($"Error: Failed to set player \"{playerUsername}\" Turret-Head (isSlayer? {isSlayer}) on server. Player is already dead.");
            return false;
        }

        if (Utils.IsTurretHead(playerScript))
        {
            Plugin.logger.LogError($"Error: Failed to set player \"{playerUsername}\" Turret-Head (isSlayer? {isSlayer}) on server. Player is already a Turret-Head.");
            return false;
        }

        GameObject controllerPrefab = isSlayer ? Content.SlayerPlayerControllerPrefab : Content.ToilPlayerControllerPrefab;
        SpawnTurretHeadControllerOnServer(controllerPrefab, playerScript.transform);

        Plugin.Instance.LogInfoExtended($"Set player \"{playerUsername}\" Turret-Head (isSlayer? {isSlayer}) on server.");

        return true;
    }

    public static void SetDeadBodyTurretHead(PlayerControllerB playerScript, bool isSlayer)
    {
        if (Plugin.IsHostOrServer)
        {
            SetDeadBodyTurretHeadOnServer(playerScript, isSlayer);
        }
        else
        {
            PluginNetworkBehaviour.Instance.SetToilHeadPlayerRagdollServerRpc(PlayerUtils.GetPlayerId(playerScript), isSlayer);
        }
    }

    public static void SetDeadBodyTurretHeadOnServer(PlayerControllerB playerScript, bool isSlayer)
    {
        if (!Plugin.IsHostOrServer) return;

        bool isReal = Plugin.ConfigManager.SpawnRealToiledPlayerRagdolls.Value;

        if (!Plugin.ConfigManager.SpawnToiledPlayerRagdolls.Value)
        {
            Plugin.Instance.LogErrorExtended($"Error: Failed to set player ragdoll Turret-Head (isSlayer? {isSlayer}, isReal? {isReal}) on server. Spawning player ragdoll Turret-Heads is disabled in the config settings.");
            return;
        }

        if (playerScript == null)
        {
            Plugin.logger.LogError($"Error: Failed to set player ragdoll Turret-Head (isSlayer? {isSlayer}, isReal? {isReal}) on server. PlayerControllerB is null.");
            return;
        }
        
        if (_setDeadBodyTurretHeadOnServerCoroutine != null)
        {
            StartOfRound.Instance.StopCoroutine(_setDeadBodyTurretHeadOnServerCoroutine);
        }

        _setDeadBodyTurretHeadOnServerCoroutine = StartOfRound.Instance.StartCoroutine(SetDeadBodyTurretHeadOnServerCO(playerScript, isSlayer, isReal));
    }

    private static IEnumerator SetDeadBodyTurretHeadOnServerCO(PlayerControllerB playerScript, bool isSlayer, bool isReal)
    {
        if (!Plugin.IsHostOrServer) yield break;

        string playerUsername = playerScript.playerUsername;

        yield return Utils.WaitUntil(() =>
        {
            return playerScript.deadBody != null;
        });

        DeadBodyInfo deadBodyScript = playerScript.deadBody;

        if (deadBodyScript == null)
        {
            Plugin.logger.LogError($"Error: Failed to set player \"{playerUsername}\" ragdoll Turret-Head (isSlayer? {isSlayer}, isReal? {isReal}) on server. DeadBodyInfo is null.");
            yield break;
        }

        GameObject ragdollObject = deadBodyScript.gameObject;

        if (ragdollObject.name != "PlayerRagdollSpring Variant(Clone)")
        {
            Plugin.logger.LogError($"Error: Failed to set player \"{playerUsername}\" ragdoll Turret-Head (isSlayer? {isSlayer}, isReal? {isReal}) on server. Player ragdoll is not of type \"PlayerRagdollSpring Variant\".");
            yield break;
        }

        yield return Utils.WaitUntil(() =>
        {
            return ragdollObject.GetComponentInChildren<NetworkObject>() != null;
        });

        NetworkObject ragdollNetworkObject = ragdollObject.GetComponentInChildren<NetworkObject>();

        if (ragdollNetworkObject == null)
        {
            Plugin.logger.LogError($"Error: Failed to set player \"{playerUsername}\" ragdoll Turret-Head (isSlayer? {isSlayer}, isReal? {isReal}) on server. NetworkObject is null.");
            yield break;
        }

        if (Utils.IsTurretHead(deadBodyScript))
        {
            Plugin.logger.LogError($"Error: Failed to set player \"{playerUsername}\" ragdoll Turret-Head (isSlayer? {isSlayer}, isReal? {isReal}) on server. Player is already a player ragdoll Turret-Head.");
            yield break;
        }

        GameObject controllerPrefab = isSlayer ? Content.SlayedDeadBodyControllerPrefab : Content.ToiledDeadBodyControllerPrefab;
        SpawnTurretHeadControllerOnServer(controllerPrefab, ragdollNetworkObject.transform);

        Plugin.Instance.LogInfoExtended($"Set player \"{playerUsername}\" ragdoll Turret-Head (isSlayer? {isSlayer}, isReal? {isReal}) on server.");
    }

    private static void SpawnTurretHeadControllerOnServer(GameObject controllerPrefab, Transform parentTransform)
    {
        if (!Plugin.IsHostOrServer) return;

        GameObject controllerObject = Object.Instantiate(controllerPrefab, parentTransform);
        controllerObject.GetComponent<NetworkObject>().Spawn();
        controllerObject.transform.SetParent(parentTransform);
        controllerObject.GetComponent<TurretHeadControllerBehaviour>().SetupTurret();
    }
    #endregion

    #region Add Turret-Head Controller Pair
    public static void AddEnemyTurretHeadControllerPair(EnemyAI enemyScript, TurretHeadControllerBehaviour behaviour)
    {
        EnemyTurretHeadControllerPairs.Add(enemyScript, behaviour);
    }

    public static void AddPlayerTurretHeadControllerPair(PlayerControllerB playerScript, TurretHeadControllerBehaviour behaviour)
    {
        PlayerTurretHeadControllerPairs.Add(playerScript, behaviour);
    }

    public static void AddDeadBodyTurretHeadControllerPair(PlayerControllerB playerScript, TurretHeadControllerBehaviour behaviour)
    {
        DeadBodyTurretHeadControllerPairs.Add(playerScript, behaviour);
    }
    #endregion

    #region Add SpawnCount
    internal static void AddToEnemySpawnCount(EnemyAI enemyScript, bool isSlayer)
    {
        string enemyName = enemyScript.enemyType.enemyName;
        TurretHeadData turretHeadData = GetEnemyTurretHeadData(enemyName, isSlayer);

        if (turretHeadData == null)
        {
            Plugin.logger.LogError($"Error: Failed to add to spawn count for enemy \"{enemyName}\". TurretHeadData is null.");
            return;
        }

        turretHeadData.AddToSpawnCount();

        Plugin.Instance.LogInfoExtended($"AddToEnemySpawnCount(); Enemy \"{enemyName}\" SpawnCount: {turretHeadData.SpawnCount}, MaxSpawnCount: {turretHeadData.GetSpawnDataForCurrentMoon().MaxSpawnCount}, SpawnChance: {turretHeadData.GetSpawnDataForCurrentMoon().SpawnChance}");
    }

    internal static void AddToPlayerSpawnCount()
    {
        TurretHeadData turretHeadData = PlayerTurretHeadData;

        turretHeadData.AddToSpawnCount();
    }
    #endregion

    public static TurretHeadData GetEnemyTurretHeadData(string enemyName, bool isSlayer)
    {
        foreach (var turretHeadData in TurretHeadDataList)
        {
            if (!turretHeadData.EnemyName.Equals(enemyName, System.StringComparison.OrdinalIgnoreCase)) continue;
            if (turretHeadData.IsSlayer != isSlayer) continue;

            return turretHeadData;
        }
        
        return null;
    }

    public static void DespawnEnemyControllerOnServer(EnemyAI enemyScript)
    {
        if (!Plugin.IsHostOrServer) return;

        string enemyName = enemyScript.enemyType.enemyName;

        if (EnemyTurretHeadControllerPairs.TryGetValue(enemyScript, out TurretHeadControllerBehaviour behaviour))
        {
            if (behaviour.TryGetComponent(out NetworkObject networkObject))
            {
                networkObject.Despawn();

                Plugin.Instance.LogInfoExtended($"Despawned enemy \"{enemyName}\" Turret-Head controller.");
            }
            else
            {
                Plugin.logger.LogError($"Error: Failed to despawn enemy \"{enemyName}\" Turret-Head controller. NetworkObject is null.");
            }

            EnemyTurretHeadControllerPairs.Remove(enemyScript);
        }
        else
        {
            Plugin.logger.LogError($"Error: Failed to despawn enemy \"{enemyName}\" Turret-Head controller. Could not find value from key.");
        }
    }

    public static void DespawnPlayerControllerOnServer(PlayerControllerB playerScript)
    {
        if (!Plugin.IsHostOrServer) return;

        string playerUsername = playerScript.playerUsername;

        if (PlayerTurretHeadControllerPairs.TryGetValue(playerScript, out TurretHeadControllerBehaviour behaviour))
        {
            if (behaviour.TryGetComponent(out NetworkObject networkObject))
            {
                networkObject.Despawn();

                Plugin.Instance.LogInfoExtended($"Despawned player \"{playerUsername}\" Turret-Head controller.");
            }
            else
            {
                Plugin.logger.LogError($"Error: Failed to despawn player \"{playerUsername}\" Turret-Head controller. NetworkObject is null.");
            }

            PlayerTurretHeadControllerPairs.Remove(playerScript);
        }
        else
        {
            Plugin.logger.LogError($"Error: Failed to despawn player \"{playerUsername}\" Turret-Head controller. Could not find value from key.");
        }
    }

    public static void DespawnDeadBodyControllerOnServer(PlayerControllerB playerScript)
    {
        if (!Plugin.IsHostOrServer) return;

        string playerUsername = playerScript.playerUsername;

        if (DeadBodyTurretHeadControllerPairs.TryGetValue(playerScript, out TurretHeadControllerBehaviour behaviour))
        {
            if (behaviour.TryGetComponent(out NetworkObject networkObject))
            {
                networkObject.Despawn();

                Plugin.Instance.LogInfoExtended($"Despawned player \"{playerUsername}\" ragdoll Turret-Head controller.");
            }
            else
            {
                Plugin.logger.LogError($"Error: Failed to despawn player \"{playerUsername}\" ragdoll Turret-Head controller. NetworkObject is null.");
            }

            DeadBodyTurretHeadControllerPairs.Remove(playerScript);
        }
        else
        {
            Plugin.logger.LogError($"Error: Failed to despawn player \"{playerUsername}\" ragdoll Turret-Head controller. Could not find value from key.");
        }
    }

    private static void DespawnAllControllersOnServer()
    {
        if (!Plugin.IsHostOrServer) return;

        try
        {
            foreach (var turretHeadBehaviour in Object.FindObjectsByType<TurretHeadControllerBehaviour>(FindObjectsSortMode.None))
            {
                if (!turretHeadBehaviour.TryGetComponent(out NetworkObject networkObject))
                {
                    Plugin.logger.LogError("Error: Failed to despawn TurretHeadBehaviour. NetworkObject is null.");
                    continue;
                }

                networkObject.Despawn();
            }

            Plugin.Instance.LogInfoExtended("Finished despawning all TurretHeadBehaviour(s).");
        }
        catch (System.Exception e)
        {
            Plugin.logger.LogError($"Error: Failed to despawn all TurretHeadBehaviour(s).\n\n{e}");
        }
    }

    public static bool IsEnemyTurretHead(EnemyAI enemyScript)
    {
        return EnemyTurretHeadControllerPairs.ContainsKey(enemyScript);
    }

    public static bool IsPlayerTurretHead(PlayerControllerB playerScript)
    {
        return PlayerTurretHeadControllerPairs.ContainsKey(playerScript);
    }

    public static bool IsDeadBodyTurretHead(PlayerControllerB playerScript)
    {
        return DeadBodyTurretHeadControllerPairs.ContainsKey(playerScript);
    }
}
