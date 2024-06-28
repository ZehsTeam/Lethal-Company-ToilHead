using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(GameNetworkManager))]
internal class GameNetworkManagerPatch
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch()
    {
        AddNetworkPrefabs();
    }

    private static void AddNetworkPrefabs()
    {
        // Network Handler
        AddNetworkPrefab(Content.NetworkHandlerPrefab);

        // Turret-Head Controllers
        AddNetworkPrefab(Content.ToilHeadControllerPrefab);
        AddNetworkPrefab(Content.ToilSlayerControllerPrefab);
        AddNetworkPrefab(Content.MantiToilControllerPrefab);
        AddNetworkPrefab(Content.MantiSlayerControllerPrefab);
        AddNetworkPrefab(Content.ToilPlayerControllerPrefab);
        AddNetworkPrefab(Content.SlayerPlayerControllerPrefab);
        AddNetworkPrefab(Content.ToiledDeadBodyControllerPrefab);
        AddNetworkPrefab(Content.SlayedDeadBodyControllerPrefab);
        AddNetworkPrefab(Content.ToilMaskedControllerPrefab);
        AddNetworkPrefab(Content.SlayerMaskedControllerPrefab);
    }

    private static void AddNetworkPrefab(GameObject prefab)
    {
        if (prefab == null)
        {
            Plugin.logger.LogError("Error: Failed to register network prefab. Prefab is null.");
            return;
        }

        NetworkManager.Singleton.AddNetworkPrefab(prefab);

        Plugin.logger.LogInfo($"Registered \"{prefab.name}\" network prefab.");
    }
}
