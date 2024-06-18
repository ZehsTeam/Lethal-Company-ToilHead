using com.github.zehsteam.ToilHead.MonoBehaviours;
using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerBPatch
{
    public static Dictionary<PlayerControllerB, ToilHeadTurretBehaviour> PlayerTurretPairs { get; private set; } = [];
    public static List<ToilHeadTurretBehaviour> DeadBodyTurrets { get; private set; } = [];

    public static int ToilPlayerSpawnCount = 0;
    public static bool ForceToilPlayerSpawns = false;
    public static int ForceToilPlayerMaxSpawnCount = -1;

    public static void Reset()
    {
        PlayerTurretPairs = [];
        DeadBodyTurrets = [];

        ToilPlayerSpawnCount = 0;
        ForceToilPlayerSpawns = false;
        ForceToilPlayerMaxSpawnCount = -1;
    }

    public static void AddPlayerTurretPair(PlayerControllerB playerScript, ToilHeadTurretBehaviour turretScript)
    {
        PlayerTurretPairs.Add(playerScript, turretScript);
    }

    public static void AddDeadBodyTurret(ToilHeadTurretBehaviour turretScript)
    {
        DeadBodyTurrets.Add(turretScript);
    }

    public static void DespawnAllTurretsOnServer()
    {
        if (!Plugin.IsHostOrServer) return;

        List<ToilHeadTurretBehaviour> turretScripts = [
            .. PlayerTurretPairs.Values.ToList(),
            .. DeadBodyTurrets
        ];

        foreach (var turretScript in turretScripts)
        {
            DespawnTurret(turretScript);
        }

        PlayerTurretPairs.Clear();
        DeadBodyTurrets.Clear();

        Plugin.Instance.LogInfoExtended($"Finished despawning all Toil-Player/Toiled turrets.");
    }

    public static void TrySpawnToilPlayersOnServer()
    {
        if (!Plugin.IsHostOrServer) return;
        if (GameNetworkManager.Instance.connectedPlayers == 1) return;
        if (StartOfRound.Instance == null) return;
        if (!StartOfRound.Instance.currentLevel.spawnEnemiesAndScrap) return;

        Plugin.Instance.LogInfoExtended("Trying to spawn Toil-Players.");

        foreach (var playerScript in StartOfRound.Instance.allPlayerScripts)
        {
            if (playerScript == null) continue;

            TrySpawnToilPlayer(playerScript);
        }
    }

    private static bool TrySpawnToilPlayer(PlayerControllerB playerScript)
    {
        SpawnData spawnData = SpawnDataManager.GetToilPlayerSpawnDataForCurrentMoon();
        int maxSpawnCount = spawnData.MaxSpawnCount;
        int spawnChance = spawnData.SpawnChance;

        if (ForceToilPlayerMaxSpawnCount > -1)
        {
            maxSpawnCount = ForceToilPlayerMaxSpawnCount;
        }

        if (!ForceToilPlayerSpawns)
        {
            if (ToilPlayerSpawnCount >= maxSpawnCount) return false;
            if (!Utils.RandomPercent(spawnChance)) return false;
        }

        bool isSlayer = Utils.RandomPercent(5);

        return Plugin.Instance.SetToilPlayerOnServer(playerScript, isSlayer);
    }

    [HarmonyPatch("KillPlayerServerRpc")]
    [HarmonyPrefix]
    static void KillPlayerServerRpcPatch(ref PlayerControllerB __instance)
    {
        DespawnTurret(__instance);
    }

    [HarmonyPatch("OnDestroy")]
    [HarmonyPrefix]
    static void OnDestroyPatch(ref PlayerControllerB __instance)
    {
        if (!Plugin.IsHostOrServer) return;

        DespawnTurret(__instance);
    }

    private static void DespawnTurret(PlayerControllerB playerScript)
    {
        if (!Plugin.IsHostOrServer) return;

        if (PlayerTurretPairs.TryGetValue(playerScript, out ToilHeadTurretBehaviour turretScript))
        {
            try
            {
                DespawnTurret(turretScript);
                PlayerTurretPairs.Remove(playerScript);

                Plugin.Instance.LogInfoExtended($"Despawned \"{playerScript.playerUsername}\" Toil-Player turret.");
            }
            catch (System.Exception e)
            {
                Plugin.logger.LogError($"Error: Failed to despawn \"{playerScript.playerUsername}\" Toil-Player turret.\n\n{e}");
            }
        }
    }

    private static void DespawnTurret(ToilHeadTurretBehaviour turretScript)
    {
        if (!Plugin.IsHostOrServer) return;

        NetworkObject turretNetworkObject = turretScript.transform.parent.GetComponent<NetworkObject>();

        if (turretNetworkObject == null)
        {
            Plugin.logger.LogError($"Error: Failed to despawn Toil-Player/Toiled turret. NetworkObject is null.");
            return;
        }

        if (!turretNetworkObject.IsSpawned)
        {
            Plugin.logger.LogError($"Error: Failed to despawn Toil-Player/Toiled turret. NetworkObject is not spawned.");
            return;
        }

        turretNetworkObject.Despawn();
    }

    public static bool IsToilPlayer(PlayerControllerB playerScript)
    {
        if (playerScript == null) return false;

        return PlayerTurretPairs.ContainsKey(playerScript);
    }
}
