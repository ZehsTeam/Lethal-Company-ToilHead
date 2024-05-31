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

        Plugin.logger.LogInfo($"Finished despawning all Toil-Head/Manti-Toil turrets.");
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

        int maxSpawnCount = currentToilHeadConfigData.maxSpawnCount;

        if (forceToilHeadMaxSpawnCount > -1)
        {
            maxSpawnCount = forceToilHeadMaxSpawnCount;
        }
 
        if (!forceToilHeadSpawns)
        {
            if (toilHeadSpawnCount >= maxSpawnCount) return;

            if (!Utils.RandomPercent(currentToilHeadConfigData.spawnChance))
            {
                return;
            }
        }

        Plugin.Instance.SetToilHeadOnServer(enemyAI);
    }

    private static void ManticoilStart(EnemyAI enemyAI)
    {
        if (!Plugin.IsHostOrServer) return;

        int maxSpawnCount = Plugin.ConfigManager.MantiToilMaxSpawnCount.Value;

        if (forceMantiToilMaxSpawnCount > -1)
        {
            maxSpawnCount = forceMantiToilMaxSpawnCount;
        }

        if (!forceMantiToilSpawns)
        {
            if (mantiToilSpawnCount >= maxSpawnCount) return;

            if (!Utils.RandomPercent(Plugin.ConfigManager.MantiToilSpawnChance.Value))
            {
                return;
            }
        }

        Plugin.Instance.SetMantiToilOnServer(enemyAI);
    }

    [HarmonyPatch("HitEnemyServerRpc")]
    [HarmonyPostfix]
    static void HitEnemyServerRpcPatch(ref EnemyAI __instance, int playerWhoHit)
    {
        if (playerWhoHit == -1) return;

        if (Utils.IsToilHead(__instance))
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

    [HarmonyPatch("OnDestroy")]
    [HarmonyPrefix]
    static void OnDestroyPatch(ref EnemyAI __instance)
    {
        if (Utils.IsSpring(__instance))
        {
            TurretHeadOnDestroy(__instance);
        }
    }

    private static void TurretHeadOnDestroy(EnemyAI enemyAI)
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

            Plugin.logger.LogInfo($"Despawned Toil-Head/Manti-Toil turret (NetworkObjectId: {turretNetworkObject.NetworkObjectId}).");
        }
    }
}
