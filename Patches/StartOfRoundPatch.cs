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
        bool isHostOrServer = NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer;

        if (isHostOrServer)
        {
            SpawnNetworkHandler();
        }
    }

    private static void SpawnNetworkHandler()
    {
        var networkHandlerHost = Object.Instantiate(GameNetworkManagerPatch.networkPrefab, Vector3.zero, Quaternion.identity);
        networkHandlerHost.GetComponent<NetworkObject>().Spawn();
    }
}
