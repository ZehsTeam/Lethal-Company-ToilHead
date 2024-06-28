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
        if (!Utils.IsValidEnemy(__instance)) return;

        if (!TurretHeadManager.TrySetEnemyTurretHeadOnServer(__instance, isSlayer: true))
        {
            TurretHeadManager.TrySetEnemyTurretHeadOnServer(__instance, isSlayer: false);
        }
    }

    [HarmonyPatch("HitEnemyServerRpc")]
    [HarmonyPostfix]
    static void HitEnemyServerRpcPatch(ref EnemyAI __instance, int playerWhoHit)
    {
        if (__instance.isEnemyDead) return;

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
        if (!destroy) return;

        if (TurretHeadManager.IsEnemyTurretHead(__instance))
        {
            TurretHeadManager.DespawnEnemyControllerOnServer(__instance);
        }
    }
}
