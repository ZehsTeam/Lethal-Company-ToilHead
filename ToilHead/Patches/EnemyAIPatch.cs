using com.github.zehsteam.ToilHead.MonoBehaviours;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal class EnemyAIPatch
{
    private static Dictionary<NetworkObject, NetworkObject> enemyTurretPairs = [];
    private static int springSpawnCount = 0;

    public static void Reset()
    {
        enemyTurretPairs.Clear();
        springSpawnCount = 0;
    }

    public static void AddEnemyTurretPair(NetworkObject enemyNetworkObject, NetworkObject turretNetworkObject)
    {
        enemyTurretPairs.Add(enemyNetworkObject, turretNetworkObject);
    }

    public static void DespawnAllTurrets()
    {
        if (!Plugin.IsHostOrServer) return;

        NetworkObject[] turretNetworkObjects = enemyTurretPairs.Values.ToArray();

        foreach (var networkObject in turretNetworkObjects)
        {
            if (!networkObject.IsSpawned) continue;

            networkObject.Despawn();

            Plugin.logger.LogInfo($"Despawned Toil-Head turret (NetworkObjectId: {networkObject.NetworkObjectId}).");
        }

        Plugin.logger.LogInfo($"Finished despawning all Toil-Head turrets.");
    }

    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch(ref EnemyAI __instance)
    {
        if (IsSpring(__instance))
        {
            SpringStart(__instance);
        }
    }

    private static void SpringStart(EnemyAI enemyAI)
    {
        if (!Plugin.IsHostOrServer) return;

        if (Random.Range(1f, 100f) > Plugin.Instance.ConfigManager.SpawnChance) return;
        if (springSpawnCount >= Plugin.Instance.ConfigManager.MaxSpawnCount) return;

        Plugin.Instance.SetToilHeadOnServer(enemyAI);

        springSpawnCount++;
    }

    [HarmonyPatch("HitEnemyServerRpc")]
    [HarmonyPostfix]
    static void HitEnemyServerRpcPatch(ref EnemyAI __instance, int playerWhoHit)
    {
        if (playerWhoHit == -1) return;

        if (IsToilHead(__instance))
        {
            ToilHeadOnHitEnemyOnServer(__instance);
        }
    }

    private static void ToilHeadOnHitEnemyOnServer(EnemyAI enemyAI)
    {
        ToilHeadTurretBehaviour behaviour = GetToilHeadTurretBehaviour(enemyAI);
        if (behaviour == null) return;

        if (!behaviour.turretActive) return;

        behaviour.EnterBerserkModeClientRpc();
    }

    [HarmonyPatch("OnDestroy")]
    [HarmonyPrefix]
    static void OnDestroyPatch(ref EnemyAI __instance)
    {
        if (IsSpring(__instance))
        {
            SpringOnDestroy(__instance);
        }
    }

    private static void SpringOnDestroy(EnemyAI enemyAI)
    {
        if (!Plugin.IsHostOrServer) return;

        NetworkObject enemyNetworkObject = enemyAI.gameObject.GetComponent<NetworkObject>();
        if (enemyNetworkObject == null) return;

        DespawnTurret(enemyNetworkObject);
    }

    private static bool IsSpring(EnemyAI enemyAI)
    {
        return enemyAI.enemyType.enemyName == "Spring";
    }

    private static bool IsToilHead(EnemyAI enemyAI)
    {
        if (!IsSpring(enemyAI)) return false;

        return enemyAI.transform.Find("ToilHeadTurretContainer(Clone)") != null;
    }

    private static ToilHeadTurretBehaviour GetToilHeadTurretBehaviour(EnemyAI enemyAI)
    {
        Transform toilHeadTurretTransform = enemyAI.transform.Find("ToilHeadTurretContainer(Clone)");
        if (toilHeadTurretTransform == null) return null;

        return toilHeadTurretTransform.gameObject.GetComponentInChildren<ToilHeadTurretBehaviour>();
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
