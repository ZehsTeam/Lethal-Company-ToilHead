using GameNetcodeStuff;
using HarmonyLib;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(Turret))]
internal class TurretPatch
{
    [HarmonyPatch("CheckForPlayersInLineOfSight")]
    [HarmonyPostfix]
    static void CheckForPlayersInLineOfSightPatch(ref PlayerControllerB __result)
    {
        if (__result != null && TurretHeadManager.IsPlayerTurretHead(__result))
        {
            __result = null;
        }
    }
}
