using com.github.zehsteam.ToilHead.MonoBehaviours;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal class EnemyAIPatch
{
    public static Dictionary<EnemyAI, ToilHeadTurretBehaviour> EnemyTurretPairs = [];

    public static int ToilHeadSpawnCount = 0;
    public static bool ForceToilHeadSpawns = false;
    public static int ForceToilHeadMaxSpawnCount = -1;

    public static int MantiToilSpawnCount = 0;
    public static bool ForceMantiToilSpawns = false;
    public static int ForceMantiToilMaxSpawnCount = -1;

    public static int ToilSlayerSpawnCount = 0;
    public static bool ForceToilSlayerSpawns = false;
    public static int ForceToilSlayerMaxSpawnCount = -1;

    public static int MantiSlayerSpawnCount = 0;
    public static bool ForceMantiSlayerSpawns = false;
    public static int ForceMantiSlayerMaxSpawnCount = -1;

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

        MantiSlayerSpawnCount = 0;
        ForceMantiSlayerSpawns = false;
        ForceMantiSlayerMaxSpawnCount = -1;
    }

    public static void AddEnemyTurretPair(EnemyAI enemyAI, ToilHeadTurretBehaviour turretScript)
    {
        EnemyTurretPairs.Add(enemyAI, turretScript);
    }

    public static void DespawnAllTurrets()
    {
        if (!Plugin.IsHostOrServer) return;

        ToilHeadTurretBehaviour[] turretScripts = EnemyTurretPairs.Values.ToArray();

        foreach (var turretScript in turretScripts)
        {
            DespawnTurret(turretScript);
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

        return Plugin.Instance.SetToilHeadOnServer(enemyAI);
    }

    private static bool TrySpawnToilSlayer(EnemyAI enemyAI)
    {
        SpawnData spawnData = SpawnDataManager.GetToilSlayerSpawnDataForCurrentMoon();
        int maxSpawnCount = spawnData.MaxSpawnCount;
        int spawnChance = spawnData.SpawnChance;

        if (ForceToilSlayerMaxSpawnCount > -1)
        {
            maxSpawnCount = ForceToilSlayerMaxSpawnCount;
        }

        if (!ForceToilSlayerSpawns)
        {
            if (ToilSlayerSpawnCount >= maxSpawnCount) return false;
            if (!Utils.RandomPercent(spawnChance)) return false;
        }

        return Plugin.Instance.SetToilSlayerOnServer(enemyAI);
    }

    private static void ManticoilStart(EnemyAI enemyAI)
    {
        if (!Plugin.IsHostOrServer) return;

        if (!TrySpawnMantiSlayer(enemyAI))
        {
            TrySpawnMantiToil(enemyAI);
        }
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

        return Plugin.Instance.SetMantiToilOnServer(enemyAI);
    }

    private static bool TrySpawnMantiSlayer(EnemyAI enemyAI)
    {
        SpawnData spawnData = SpawnDataManager.GetMantiSlayerSpawnDataForCurrentMoon();
        int maxSpawnCount = spawnData.MaxSpawnCount;
        int spawnChance = spawnData.SpawnChance;

        if (ForceMantiSlayerMaxSpawnCount > -1)
        {
            maxSpawnCount = ForceMantiSlayerMaxSpawnCount;
        }

        if (!ForceMantiSlayerSpawns)
        {
            if (MantiSlayerSpawnCount >= maxSpawnCount) return false;
            if (!Utils.RandomPercent(spawnChance)) return false;
        }

        return Plugin.Instance.SetMantiSlayerOnServer(enemyAI);
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
        ToilHeadTurretBehaviour behaviour = Utils.GetTurretHeadTurretBehaviour(enemyAI);
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

        NetworkObject enemyNetworkObject = enemyAI.GetComponent<NetworkObject>();

        if (enemyNetworkObject == null)
        {
            Plugin.logger.LogInfo($"Error: Failed to despawn Turret-Head turret. Enemy NetworkObject is null.");
            return;
        }

        if (EnemyTurretPairs.TryGetValue(enemyAI, out ToilHeadTurretBehaviour turretScript))
        {
            try
            {
                DespawnTurret(turretScript);
                EnemyTurretPairs.Remove(enemyAI);

                Plugin.logger.LogInfo($"Despawned Turret-Head turret (NetworkObjectId: {enemyNetworkObject.NetworkObjectId}).");
            }
            catch (System.Exception e)
            {
                Plugin.logger.LogInfo($"Error: Failed to despawn Turret-Head turret. (NetworkObjectId: {enemyNetworkObject.NetworkObjectId}).\n\n{e}");
            }
        }
    }

    private static void DespawnTurret(ToilHeadTurretBehaviour turretScript)
    {
        if (!Plugin.IsHostOrServer) return;

        NetworkObject turretNetworkObject = turretScript.transform.parent.GetComponent<NetworkObject>();

        if (turretNetworkObject == null)
        {
            Plugin.logger.LogInfo($"Error: Failed to despawn Turret-Head turret. ToilHeadTurretBehaviour NetworkObject is null.");
            return;
        }

        if (!turretNetworkObject.IsSpawned)
        {
            Plugin.logger.LogInfo($"Error: Failed to despawn Turret-Head turret. ToilHeadTurretBehaviour NetworkObject is not spawned.");
            return;
        }

        turretNetworkObject.Despawn();
    }
}
