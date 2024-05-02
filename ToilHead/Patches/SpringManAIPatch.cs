using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(SpringManAI))]
internal class SpringManAIPatch
{
    [HarmonyPatch("OnCollideWithPlayer")]
    [HarmonyPostfix]
    static void OnCollideWithPlayerPatch(ref SpringManAI __instance, ref Collider other)
    {
        if (!Utils.IsToilHead(__instance)) return;

        if (other.gameObject.TryGetComponent(out PlayerControllerB playerScript))
        {
            if (playerScript != Utils.GetLocalPlayerScript()) return;
            if (!playerScript.isPlayerDead) return;

            Plugin.Instance.SetToilHeadPlayerRagdoll(playerScript);
        }
    }
}
