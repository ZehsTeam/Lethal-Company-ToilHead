using com.github.zehsteam.ToilHead.MonoBehaviours;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal class EnemyAIPatch
{
    public static Dictionary<NetworkObject, NetworkObject> EnemyTurretPairs = [];

    public static int ToilHeadSpawnCount = 0;
    public static bool ForceToilHeadSpawns = false;
    public static int ForceToilHeadMaxSpawnCount = -1;

    public static int MantiToilSpawnCount = 0;
    public static bool ForceMantiToilSpawns = false;
    public static int ForceMantiToilMaxSpawnCount = -1;

    public static int ToilSlayerSpawnCount = 0;
    public static bool ForceToilSlayerSpawns = false;
    public static int ForceToilSlayerMaxSpawnCount = -1;

    public static void Reset()
    {
        EnemyTurretPairs = [];

        ToilHeadSpawnCount = 0;
        ForceToilHeadSpawns = false;
        ForceToilHeadMaxSpawnCount = -1;

        MantiToilSpawnCount = 0;
        ForceMantiToilSpawns = false;
        ForceMantiToilMaxSpawnCount = -1;

        ToilSlayerSpawnCount = 0;
        ForceToilSlayerSpawns = false;
        ForceToilSlayerMaxSpawnCount = -1;
    }

    public static void AddEnemyTurretPair(NetworkObject enemyNetworkObject, NetworkObject turretNetworkObject)
    {
        EnemyTurretPairs.Add(enemyNetworkObject, turretNetworkObject);
    }

    public static void DespawnAllTurrets()
    {
        if (!Plugin.IsHostOrServer) return;

        NetworkObject[] enemyNetworkObjects = EnemyTurretPairs.Keys.ToArray();

        foreach (var enemyNetworkObject in enemyNetworkObjects)
        {
            DespawnTurret(enemyNetworkObject);
        }

        EnemyTurretPairs.Clear();

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
        SpawnData spawnData = SpawnDataManager.GetToilHeadSpawnDataForCurrentMoon();
        int maxSpawnCount = spawnData.MaxSpawnCount;
        int spawnChance = spawnData.SpawnChance;

        if (ForceToilHeadMaxSpawnCount > -1)
        {
            maxSpawnCount = ForceToilHeadMaxSpawnCount;
        }

        if (!ForceToilHeadSpawns)
        {
            if (ToilHeadSpawnCount >= maxSpawnCount) return false;
            if (!Utils.RandomPercent(spawnChance)) return false;
        }

        Plugin.Instance.SetToilHeadOnServer(enemyAI);
        return true;
    }

    private static bool TrySpawnToilSlayer(EnemyAI enemyAI)
    {
        SpawnData spawnData = SpawnDataManager.GetToilSlayerSpawnDataForCurrentMoon();
        int maxSpawnCount = spawnData.MaxSpawnCount;
        int spawnChance = spawnData.SpawnChance;

        if (Utils.IsCurrentMoonToilation())
        {
            maxSpawnCount = 2;
            spawnChance = 10;
        }

        if (ForceToilSlayerMaxSpawnCount > -1)
        {
            maxSpawnCount = ForceToilSlayerMaxSpawnCount;
        }

        if (!ForceToilSlayerSpawns)
        {
            if (ToilSlayerSpawnCount >= maxSpawnCount) return false;
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
        SpawnData spawnData = SpawnDataManager.GetMantiToilSpawnDataForCurrentMoon();
        int maxSpawnCount = spawnData.MaxSpawnCount;
        int spawnChance = spawnData.SpawnChance;

        if (ForceMantiToilMaxSpawnCount > -1)
        {
            maxSpawnCount = ForceMantiToilMaxSpawnCount;
        }

        if (!ForceMantiToilSpawns)
        {
            if (MantiToilSpawnCount >= maxSpawnCount) return false;
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

        if (EnemyTurretPairs.TryGetValue(enemyNetworkObject, out NetworkObject turretNetworkObject))
        {
            if (!turretNetworkObject.IsSpawned) return;

            turretNetworkObject.Despawn();
            EnemyTurretPairs.Remove(enemyNetworkObject);

            Plugin.logger.LogInfo($"Despawned Turret-Head turret (NetworkObjectId: {turretNetworkObject.NetworkObjectId}).");
        }
    }
}
