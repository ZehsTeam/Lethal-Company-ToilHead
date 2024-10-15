using com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;
using HarmonyLib;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal static class EnemyAIPatch
{
    [HarmonyPatch(nameof(EnemyAI.Start))]
    [HarmonyPostfix]
    private static void StartPatch(ref EnemyAI __instance)
    {
        if (!Utils.IsValidEnemy(__instance)) return;

        if (!TurretHeadManager.TrySetEnemyTurretHeadOnServer(__instance, isSlayer: true))
        {
            TurretHeadManager.TrySetEnemyTurretHeadOnServer(__instance, isSlayer: false);
        }
    }

    [HarmonyPatch(nameof(EnemyAI.HitEnemyServerRpc))]
    [HarmonyPostfix]
    private static void HitEnemyServerRpcPatch(ref EnemyAI __instance, int playerWhoHit)
    {
        if (__instance.isEnemyDead) return;

        if (TurretHeadManager.EnemyTurretHeadControllerPairs.TryGetValue(__instance, out TurretHeadControllerBehaviour behaviour))
        {
            if (!behaviour.TurretBehaviour.turretActive) return;

            behaviour.TurretBehaviour.EnterBerserkModeClientRpc();
        }
    }

    [HarmonyPatch(nameof(EnemyAI.KillEnemy))]
    [HarmonyPrefix]
    private static void KillEnemyPatch(ref EnemyAI __instance, bool destroy)
    {
        if (!NetworkUtils.IsServer) return;
        if (!destroy) return;

        if (TurretHeadManager.IsEnemyTurretHead(__instance))
        {
            TurretHeadManager.DespawnEnemyControllerOnServer(__instance);
        }
    }
}
