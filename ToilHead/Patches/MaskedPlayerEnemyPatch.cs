using HarmonyLib;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(MaskedPlayerEnemy))]
internal static class MaskedPlayerEnemyPatch
{
    [HarmonyPatch(nameof(MaskedPlayerEnemy.Start))]
    [HarmonyPostfix]
    private static void StartPatch(ref EnemyAI __instance)
    {
        if (!TurretHeadManager.TrySetEnemyTurretHeadOnServer(__instance, isSlayer: true))
        {
            TurretHeadManager.TrySetEnemyTurretHeadOnServer(__instance, isSlayer: false);
        }
    }
}
