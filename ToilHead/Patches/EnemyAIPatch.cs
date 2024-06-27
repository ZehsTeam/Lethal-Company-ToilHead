using com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;
using HarmonyLib;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal class EnemyAIPatch
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch(ref EnemyAI __instance)
    {
        if (!Utils.IsSpring(__instance) && !Utils.IsManticoil(__instance))
        {
            return;
        }

        if (!TurretHeadManager.TrySetTurretHeadOnServer(__instance, isSlayer: true))
        {
            TurretHeadManager.TrySetTurretHeadOnServer(__instance, isSlayer: false);
        }
    }

    [HarmonyPatch("HitEnemyServerRpc")]
    [HarmonyPostfix]
    static void HitEnemyServerRpcPatch(ref EnemyAI __instance, int playerWhoHit)
    {
        if (playerWhoHit == -1) return;

        if (TurretHeadManager.EnemyTurretHeadControllerPairs.TryGetValue(__instance, out TurretHeadControllerBehaviour behaviour))
        {
            if (!behaviour.TurretBehaviour.turretActive) return;

            behaviour.TurretBehaviour.EnterBerserkModeClientRpc();
        }
    }

    [HarmonyPatch("KillEnemy")]
    [HarmonyPrefix]
    static void KillEnemyPatch(ref EnemyAI __instance, bool destroy)
    {
        if (!Plugin.IsHostOrServer) return;
        if (!Utils.IsTurretHead(__instance)) return;
        if (!destroy) return;

        TurretHeadManager.DespawnControllerOnServer(__instance);
    }
}
