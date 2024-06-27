using GameNetcodeStuff;
using HarmonyLib;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(RagdollGrabbableObject))]
internal class RagdollGrabbableObjectPatch
{
    [HarmonyPatch("OnDestroy")]
    [HarmonyPrefix]
    static void OnDestroyPatch(ref RagdollGrabbableObject __instance)
    {
        if (!Plugin.IsHostOrServer) return;

        PlayerControllerB playerScript = __instance.ragdoll.playerScript;

        if (TurretHeadManager.IsDeadBodyTurretHead(playerScript))
        {
            TurretHeadManager.DespawnDeadBodyControllerOnServer(playerScript);
        }
    }
}
