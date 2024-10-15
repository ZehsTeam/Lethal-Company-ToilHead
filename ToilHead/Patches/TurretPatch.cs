using GameNetcodeStuff;
using HarmonyLib;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(Turret))]
internal static class TurretPatch
{
    [HarmonyPatch(nameof(Turret.CheckForPlayersInLineOfSight))]
    [HarmonyPostfix]
    private static void CheckForPlayersInLineOfSightPatch(ref PlayerControllerB __result)
    {
        if (__result != null && TurretHeadManager.IsPlayerTurretHead(__result))
        {
            __result = null;
        }
    }
}
