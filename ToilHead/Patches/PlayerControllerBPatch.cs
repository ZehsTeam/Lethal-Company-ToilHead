﻿using com.github.zehsteam.ToilHead.MonoBehaviours;
using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerBPatch
{
    public static Dictionary<PlayerControllerB, ToilHeadTurretBehaviour> PlayerTurretPairs = [];

    public static int ToilPlayerSpawnCount = 0;
    public static bool ForceToilPlayerSpawns = false;
    public static int ForceToilPlayerMaxSpawnCount = -1;

    public static void Reset()
    {
        PlayerTurretPairs = [];

        ToilPlayerSpawnCount = 0;
        ForceToilPlayerSpawns = false;
        ForceToilPlayerMaxSpawnCount = -1;
    }

    public static void AddPlayerTurretPair(PlayerControllerB playerScript, ToilHeadTurretBehaviour turretScript)
    {
        PlayerTurretPairs.Add(playerScript, turretScript);
    }

    public static void DespawnAllTurrets()
    {
        if (!Plugin.IsHostOrServer) return;

        ToilHeadTurretBehaviour[] turretScripts = PlayerTurretPairs.Values.ToArray();

        foreach (var turretScript in turretScripts)
        {
            DespawnTurret(turretScript);
        }

        PlayerTurretPairs.Clear();

        Plugin.logger.LogInfo($"Finished despawning all Toil-Player turrets.");
    }

    public static void TrySpawnToilPlayersOnServer()
    {
        if (!Plugin.IsHostOrServer) return;

        if (GameNetworkManager.Instance.connectedPlayers == 1) return;

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

        Plugin.Instance.LogInfoExtended($"\n\n\n\n\n\nSetToilPlayerOnServer({playerScript.playerUsername});\n\n\n\n\n");

        return Plugin.Instance.SetToilPlayerOnServer(playerScript);
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

                Plugin.logger.LogInfo($"Despawned \"{playerScript.playerUsername}\" Toil-Player turret.");
            }
            catch (System.Exception e)
            {
                Plugin.logger.LogInfo($"Error: Failed to despawn \"{playerScript.playerUsername}\" Toil-Player turret.\n\n{e}");
            }
        }
    }

    private static void DespawnTurret(ToilHeadTurretBehaviour turretScript)
    {
        if (!Plugin.IsHostOrServer) return;

        NetworkObject turretNetworkObject = turretScript.transform.parent.GetComponent<NetworkObject>();

        if (turretNetworkObject == null)
        {
            Plugin.logger.LogInfo($"Error: Failed to despawn Toil-Player turret. ToilHeadTurretBehaviour NetworkObject is null.");
            return;
        }

        if (!turretNetworkObject.IsSpawned)
        {
            Plugin.logger.LogInfo($"Error: Failed to despawn Toil-Player turret. ToilHeadTurretBehaviour NetworkObject is not spawned.");
            return;
        }

        turretNetworkObject.Despawn();
    }
}
