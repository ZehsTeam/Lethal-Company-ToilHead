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
        if (TurretHeadManager.IsPlayerTurretHead(__instance))
        {
            TurretHeadManager.DespawnPlayerControllerOnServer(__instance);
        }
    }

    [HarmonyPatch("OnDestroy")]
    [HarmonyPrefix]
    static void OnDestroyPatch(ref PlayerControllerB __instance)
    {
        if (!Plugin.IsHostOrServer) return;

        if (TurretHeadManager.IsPlayerTurretHead(__instance))
        {
            TurretHeadManager.DespawnPlayerControllerOnServer(__instance);
        }
    }
}
