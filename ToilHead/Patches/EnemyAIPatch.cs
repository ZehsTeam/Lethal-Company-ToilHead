using com.github.zehsteam.ToilHead.MonoBehaviours;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal class EnemyAIPatch
{
    public static Dictionary<NetworkObject, NetworkObject> enemyTurretPairs = [];

    public static int toilHeadSpawnCount = 0;
    public static bool forceToilHeadSpawns = false;
    public static int forceToilHeadMaxSpawnCount = -1;

    public static int mantiToilSpawnCount = 0;
    public static bool forceMantiToilSpawns = false;
    public static int forceMantiToilMaxSpawnCount = -1;

    public static int toilSlayerSpawnCount = 0;
    public static bool forceToilSlayerSpawns = false;
    public static int forceToilSlayerMaxSpawnCount = -1;

    public static ToilHeadConfigData currentToilHeadConfigData => ToilHeadDataManager.GetDataForCurrentLevel().configData;

    public static void Reset()
    {
        enemyTurretPairs = [];

        toilHeadSpawnCount = 0;
        forceToilHeadSpawns = false;
        forceToilHeadMaxSpawnCount = -1;

        mantiToilSpawnCount = 0;
        forceMantiToilSpawns = false;
        forceMantiToilMaxSpawnCount = -1;

        toilSlayerSpawnCount = 0;
        forceToilSlayerSpawns = false;
        forceToilSlayerMaxSpawnCount = -1;
    }

    public static void AddEnemyTurretPair(NetworkObject enemyNetworkObject, NetworkObject turretNetworkObject)
    {
        enemyTurretPairs.Add(enemyNetworkObject, turretNetworkObject);
    }

    public static void DespawnAllTurrets()
    {
        if (!Plugin.IsHostOrServer) return;

        NetworkObject[] enemyNetworkObjects = enemyTurretPairs.Keys.ToArray();

        foreach (var enemyNetworkObject in enemyNetworkObjects)
        {
            DespawnTurret(enemyNetworkObject);
        }

        enemyTurretPairs.Clear();

        Plugin.logger.LogInfo($"Finished despawning all Turret-Head turrets.");
    }

    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch(ref EnemyAI __instance)
    {
        if (Utils.IsSpring(__instance))
        {
            SpringStart(__instance);
            return;
        }

        if (Utils.IsManticoil(__instance))
        {
            ManticoilStart(__instance);
            return;
        }
    }

    private static void SpringStart(EnemyAI enemyAI)
    {
        if (!Plugin.IsHostOrServer) return;

        if (!TrySpawnToilSlayer(enemyAI))
        {
            TrySpawnToilHead(enemyAI);
        }
    }

    private static bool TrySpawnToilHead(EnemyAI enemyAI)
    {
        int maxSpawnCount = currentToilHeadConfigData.maxSpawnCount;
        int spawnChance = currentToilHeadConfigData.spawnChance;

        if (Utils.IsOnToilation())
        {
            maxSpawnCount = 8;
            spawnChance = 80;
        }

        if (forceToilHeadMaxSpawnCount > -1)
        {
            maxSpawnCount = forceToilHeadMaxSpawnCount;
        }

        if (!forceToilHeadSpawns)
        {
            if (toilHeadSpawnCount >= maxSpawnCount) return false;
            if (!Utils.RandomPercent(spawnChance)) return false;
        }

        Plugin.Instance.SetToilHeadOnServer(enemyAI);
        return true;
    }

    private static bool TrySpawnToilSlayer(EnemyAI enemyAI)
    {
        int maxSpawnCount = Plugin.ConfigManager.ToilSlayerMaxSpawnCount.Value;
        int spawnChance = Plugin.ConfigManager.ToilSlayerSpawnChance.Value;

        if (Utils.IsOnToilation())
        {
            maxSpawnCount = 2;
            spawnChance = 10;
        }

        if (forceToilSlayerMaxSpawnCount > -1)
        {
            maxSpawnCount = forceToilSlayerMaxSpawnCount;
        }

        if (!forceToilSlayerSpawns)
        {
            if (toilSlayerSpawnCount >= maxSpawnCount) return false;
            if (!Utils.RandomPercent(spawnChance)) return false;
        }

        Plugin.Instance.SetToilSlayerOnServer(enemyAI);
        return true;
    }

    private static void ManticoilStart(EnemyAI enemyAI)
    {
        if (!Plugin.IsHostOrServer) return;

        TrySpawnMantiToil(enemyAI);
    }

    private static bool TrySpawnMantiToil(EnemyAI enemyAI)
    {
        int maxSpawnCount = Plugin.ConfigManager.MantiToilMaxSpawnCount.Value;
        int spawnChance = Plugin.ConfigManager.MantiToilSpawnChance.Value;

        if (Utils.IsOnToilation())
        {
            maxSpawnCount = 50;
            spawnChance = 90;
        }

        if (forceMantiToilMaxSpawnCount > -1)
        {
            maxSpawnCount = forceMantiToilMaxSpawnCount;
        }

        if (!forceMantiToilSpawns)
        {
            if (mantiToilSpawnCount >= maxSpawnCount) return false;
            if (!Utils.RandomPercent(spawnChance)) return false;
        }

        Plugin.Instance.SetMantiToilOnServer(enemyAI);
        return true;
    }

    [HarmonyPatch("HitEnemyServerRpc")]
    [HarmonyPostfix]
    static void HitEnemyServerRpcPatch(ref EnemyAI __instance, int playerWhoHit)
    {
        if (playerWhoHit == -1) return;

        if (Utils.IsTurretHead(__instance))
        {
            TurretHeadOnHitEnemyOnServer(__instance);
        }
    }

    private static void TurretHeadOnHitEnemyOnServer(EnemyAI enemyAI)
    {
        ToilHeadTurretBehaviour behaviour = Utils.GetToilHeadTurretBehaviour(enemyAI);
        if (behaviour == null) return;

        if (!behaviour.turretActive) return;

        behaviour.EnterBerserkModeClientRpc();
    }

    [HarmonyPatch("KillEnemy")]
    [HarmonyPrefix]
    static void KillEnemyPatch(ref EnemyAI __instance, bool destroy)
    {
        if (!Plugin.IsHostOrServer) return;
        if (!Utils.IsTurretHead(__instance)) return;
        if (!destroy) return;

        DespawnTurret(__instance);
    }

    private static void DespawnTurret(EnemyAI enemyAI)
    {
        if (!Plugin.IsHostOrServer) return;

        if (enemyAI.TryGetComponent(out NetworkObject enemyNetworkObject))
        {
            DespawnTurret(enemyNetworkObject);
        }
    }

    private static void DespawnTurret(NetworkObject enemyNetworkObject)
    {
        if (!Plugin.IsHostOrServer) return;

        if (enemyTurretPairs.TryGetValue(enemyNetworkObject, out NetworkObject turretNetworkObject))
        {
            if (!turretNetworkObject.IsSpawned) return;

            turretNetworkObject.Despawn();
            enemyTurretPairs.Remove(enemyNetworkObject);

            Plugin.logger.LogInfo($"Despawned Turret-Head turret (NetworkObjectId: {turretNetworkObject.NetworkObjectId}).");
        }
    }
}
