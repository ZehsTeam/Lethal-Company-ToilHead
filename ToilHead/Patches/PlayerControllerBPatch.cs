using GameNetcodeStuff;
using HarmonyLib;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerBPatch
{
    [HarmonyPatch("KillPlayerServerRpc")]
    [HarmonyPrefix]
    static void KillPlayerServerRpcPatch(ref PlayerControllerB __instance)
    {
        if (!TurretHeadManager.PlayerTurretHeadControllerPairs.ContainsKey(__instance)) return;

        TurretHeadManager.DespawnControllerOnServer(__instance);
    }

    [HarmonyPatch("OnDestroy")]
    [HarmonyPrefix]
    static void OnDestroyPatch(ref PlayerControllerB __instance)
    {
        if (!Plugin.IsHostOrServer) return;
        if (!TurretHeadManager.PlayerTurretHeadControllerPairs.ContainsKey(__instance)) return;

        TurretHeadManager.DespawnControllerOnServer(__instance);
    }
}
