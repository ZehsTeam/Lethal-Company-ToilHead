using GameNetcodeStuff;
using HarmonyLib;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerBPatch
{
    [HarmonyPatch(nameof(PlayerControllerB.KillPlayerServerRpc))]
    [HarmonyPrefix]
    private static void KillPlayerServerRpcPatch(ref PlayerControllerB __instance)
    {
        if (TurretHeadManager.IsPlayerTurretHead(__instance))
        {
            TurretHeadManager.DespawnPlayerControllerOnServer(__instance);
        }
    }

    [HarmonyPatch(nameof(PlayerControllerB.OnDestroy))]
    [HarmonyPrefix]
    private static void OnDestroyPatch(ref PlayerControllerB __instance)
    {
        if (!NetworkUtils.IsServer) return;

        if (TurretHeadManager.IsPlayerTurretHead(__instance))
        {
            TurretHeadManager.DespawnPlayerControllerOnServer(__instance);
        }
    }
}
