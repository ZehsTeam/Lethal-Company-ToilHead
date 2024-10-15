using com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(SpringManAI))]
internal static class SpringManAIPatch
{
    [HarmonyPatch(nameof(SpringManAI.OnCollideWithPlayer))]
    [HarmonyPostfix]
    private static void OnCollideWithPlayerPatch(ref SpringManAI __instance, ref Collider other)
    {
        TurretHeadControllerBehaviour controllerBehaviour = __instance.GetComponentInChildren<TurretHeadControllerBehaviour>();
        if (controllerBehaviour == null) return;

        PlayerControllerB playerScript = other.gameObject.GetComponent<PlayerControllerB>();
        if (playerScript == null) return;

        if (!PlayerUtils.IsLocalPlayer(playerScript)) return;
        if (!playerScript.AllowPlayerDeath()) return;

        bool isSlayer = controllerBehaviour.TurretBehaviour.IsMinigun;

        Plugin.Instance.LogInfoExtended($"SpringManAI OnCollideWithPlayer \"{playerScript.playerUsername}\" isSlayer? {isSlayer}");

        TurretHeadManager.SetDeadBodyTurretHead(playerScript, isSlayer);
    }
}
