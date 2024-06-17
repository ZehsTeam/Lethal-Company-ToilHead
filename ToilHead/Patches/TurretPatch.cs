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
        if (__result != null && PlayerControllerBPatch.IsToilPlayer(__result))
        {
            __result = null;
        }
    }
}
