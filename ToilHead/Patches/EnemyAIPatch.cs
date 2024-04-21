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
    public static int spawnCount = 0;

    public static void Reset()
    {
        enemyTurretPairs = [];
        spawnCount = 0;
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

        Plugin.logger.LogInfo($"Finished despawning all Toil-Head turrets.");
    }

    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch(ref EnemyAI __instance)
    {
        if (Utils.IsSpring(__instance))
        {
            SpringStart(__instance);
        }
    }

    private static void SpringStart(EnemyAI enemyAI)
    {
        if (!Plugin.IsHostOrServer) return;

        if (!Utils.RandomPercent(Plugin.Instance.ConfigManager.SpawnChance)) return;
        if (spawnCount >= Plugin.Instance.ConfigManager.MaxSpawnCount) return;

        Plugin.Instance.SetToilHeadOnServer(enemyAI);
    }

    [HarmonyPatch("HitEnemyServerRpc")]
    [HarmonyPostfix]
    static void HitEnemyServerRpcPatch(ref EnemyAI __instance, int playerWhoHit)
    {
        if (playerWhoHit == -1) return;

        if (Utils.IsToilHead(__instance))
        {
            ToilHeadOnHitEnemyOnServer(__instance);
        }
    }

    private static void ToilHeadOnHitEnemyOnServer(EnemyAI enemyAI)
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
            SpringOnDestroy(__instance);
        }
    }

    private static void SpringOnDestroy(EnemyAI enemyAI)
    {
        if (!Plugin.IsHostOrServer) return;

        if (enemyAI.TryGetComponent<NetworkObject>(out NetworkObject enemyNetworkObject))
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

            Plugin.logger.LogInfo($"Despawned Toil-Head turret (NetworkObjectId: {turretNetworkObject.NetworkObjectId}).");
        }
    }
}
