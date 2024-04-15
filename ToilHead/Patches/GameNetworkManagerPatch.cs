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
        AddNetworkPrefab(Content.networkHandlerPrefab);
        AddNetworkPrefab(Content.turretPrefab);
    }

    private static void AddNetworkPrefab(GameObject prefab)
    {
        if (prefab == null) return;

        NetworkManager.Singleton.AddNetworkPrefab(prefab);

        Plugin.logger.LogInfo($"Registered \"{prefab.name}\" network prefab.");
    }
}
