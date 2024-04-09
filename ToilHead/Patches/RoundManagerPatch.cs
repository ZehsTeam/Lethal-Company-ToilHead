using HarmonyLib;
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
        SetTurretPropPrefab();
    }

    [HarmonyPatch("LoadNewLevel")]
    [HarmonyPostfix]
    static void LoadNewLevelPatch(int randomSeed)
    {
        // Call on Host or Server
        Plugin.Instance.OnNewLevelLoaded(randomSeed);
    }

    [HarmonyPatch("GenerateNewLevelClientRpc")]
    [HarmonyPrefix]
    static void GenerateNewLevelClientRpcPatch(int randomSeed)
    {
        if (!Plugin.IsHostOrServer)
        {
            // Call on Client
            Plugin.Instance.OnNewLevelLoaded(randomSeed);
        }
    }

    private static void SetTurretPrefab()
    {
        if (Content.turretPrefab != null) return;

        GameObject turretPrefab = GetHazardPrefab("TurretContainer");
        Content.turretPrefab = turretPrefab;
    }

    private static void SetTurretPropPrefab()
    {
        if (Content.turretPropPrefab != null) return;
        if (Content.turretPrefab == null) return;

        Content.turretPropPrefab = Content.turretPrefab.transform.GetChild(1).gameObject;
    }

    private static GameObject GetHazardPrefab(string name)
    {
        SpawnableMapObject[] spawnableMapObjects = RoundManager.Instance.spawnableMapObjects;

        foreach (var spawnableMapObject in spawnableMapObjects)
        {
            if (spawnableMapObject.prefabToSpawn.name == name)
            {
                return spawnableMapObject.prefabToSpawn;
            }
        }

        Plugin.logger.LogError($"Error: could not find hazard \"{name}\"");

        return null;
    }
}
