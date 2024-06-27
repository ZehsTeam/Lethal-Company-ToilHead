using com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;
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
        if (!TurretHeadManager.EnemyTurretHeadControllerPairs.TryGetValue(__instance, out TurretHeadControllerBehaviour behaviour))
        {
            return;
        }

        if (!other.gameObject.TryGetComponent(out PlayerControllerB playerScript))
        {
            return;
        }

        if (playerScript != PlayerUtils.GetLocalPlayerScript()) return;
        if (!playerScript.AllowPlayerDeath()) return;

        TurretHeadManager.SetDeadBodyTurretHead(playerScript, isSlayer: behaviour.TurretBehaviour.IsMinigun);
    }
}
