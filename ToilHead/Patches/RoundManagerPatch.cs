using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatch
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    [HarmonyPriority(int.MinValue)]
    static void StartPatch()
    {
        SetTurretPrefab();
    }

    [HarmonyPatch("LoadNewLevel")]
    [HarmonyPostfix]
    static void LoadNewLevelPatch(int randomSeed)
    {
        // Call on Host or Server
        ToilHeadBase.Instance.OnNewLevelLoaded(randomSeed);
    }

    [HarmonyPatch("GenerateNewLevelClientRpc")]
    [HarmonyPrefix]
    static void GenerateNewLevelClientRpcPatch(int randomSeed)
    {
        if (!ToilHeadBase.IsHostOrServer)
        {
            // Call on Client
            ToilHeadBase.Instance.OnNewLevelLoaded(randomSeed);
        }
    }

    private static void SetTurretPrefab()
    {
        if (ToilHeadBase.Instance.turretPrefab != null) return;

        GameObject turretPrefab = GetHazardPrefab("TurretContainer");
        ToilHeadBase.Instance.turretPrefab = turretPrefab;
        SetTurretPropPrefab(turretPrefab);
    }

    private static void SetTurretPropPrefab(GameObject turretPrefab)
    {
        if (turretPrefab == null) return;

        Secret.turretPropPrefab = turretPrefab.transform.GetChild(1).gameObject;
    }

    private static GameObject GetHazardPrefab(string name)
    {
        List<SpawnableMapObject> spawnableMapObjects = RoundManager.Instance.spawnableMapObjects.ToList();

        GameObject prefab = null;

        spawnableMapObjects.ForEach(spawnableMapObject =>
        {
            if (spawnableMapObject.prefabToSpawn.name != name) return;

            prefab = spawnableMapObject.prefabToSpawn;
        });

        if (prefab == null)
        {
            ToilHeadBase.mls.LogError($"Error: could not find hazard \"{name}\"");
        }

        return prefab;
    }
}
