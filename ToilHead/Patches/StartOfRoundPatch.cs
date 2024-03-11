using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal class StartOfRoundPatch
{
    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    static void AwakePatch()
    {
        SpawnNetworkHandler();
    }

    private static void SpawnNetworkHandler()
    {
        if (!ToilHeadBase.IsHostOrServer) return;

        var networkHandlerHost = Object.Instantiate(GameNetworkManagerPatch.networkPrefab, Vector3.zero, Quaternion.identity);
        networkHandlerHost.GetComponent<NetworkObject>().Spawn();
    }

    [HarmonyPatch("ShipHasLeft")]
    [HarmonyPostfix]
    static void ShipHasLeftPatch()
    {
        ToilHeadBase.Instance.OnShipHasLeft();
    }
}
