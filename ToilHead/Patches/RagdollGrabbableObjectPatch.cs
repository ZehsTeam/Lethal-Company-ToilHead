using GameNetcodeStuff;
using HarmonyLib;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(RagdollGrabbableObject))]
internal static class RagdollGrabbableObjectPatch
{
    [HarmonyPatch(nameof(RagdollGrabbableObject.OnDestroy))]
    [HarmonyPrefix]
    private static void OnDestroyPatch(ref RagdollGrabbableObject __instance)
    {
        if (!NetworkUtils.IsServer) return;

        if (__instance.ragdoll == null || __instance.ragdoll.playerScript == null)
        {
            return;
        }

        PlayerControllerB playerScript = __instance.ragdoll.playerScript;

        if (TurretHeadManager.IsDeadBodyTurretHead(playerScript))
        {
            TurretHeadManager.DespawnDeadBodyControllerOnServer(playerScript);
        }
    }
}
