using HarmonyLib;
using System;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal class EnemyAIPatch
{
    public static Random random;
    public static int toilHeadSpawns;

    public static void Initialize(int randomMapSeed)
    {
        random = new Random(randomMapSeed + 420);
        toilHeadSpawns = 0;
    }

    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch(ref EnemyAI __instance)
    {
        bool isHostOrServer = NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer;

        if (isHostOrServer)
        {
            CheckEnemy(__instance);
        }
    }

    private static void CheckEnemy(EnemyAI enemyAI)
    {
        string enemyName = enemyAI.enemyType.enemyName;

        if (enemyName == "Spring")
        {
            EnemyIsCoilHead(enemyAI);
        }
    }

    private static void EnemyIsCoilHead(EnemyAI enemyAI)
    {
        if (random.Next(0, 100) > ToilHeadBase.Instance.configManager.SpawnChance) return;
        if (toilHeadSpawns >= ToilHeadBase.Instance.configManager.MaxSpawns) return;

        ToilHeadBase.Instance.SetToilHeadOnServer(enemyAI);
        toilHeadSpawns++;
    }
}
