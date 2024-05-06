using com.github.zehsteam.ToilHead.Patches;
using System.Collections.Generic;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead;

public class Api
{
    public static ToilHeadData ToilHeadData => ToilHeadDataManager.GetDataForCurrentLevel();
    public static int MaxSpawnCount => ToilHeadData.configData.maxSpawnCount;
    public static int SpawnChance => ToilHeadData.configData.spawnChance;

    public static Dictionary<NetworkObject, NetworkObject> enemyTurretPairs => EnemyAIPatch.enemyTurretPairs;
    public static int spawnCount => EnemyAIPatch.spawnCount;

    /// <summary>
    /// If enabled, will force any spawned Coil-Heads to become Toil-Heads.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool forceSpawns { get { return EnemyAIPatch.forceSpawns; } set { EnemyAIPatch.forceSpawns = value; } }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Toil-Head max spawn count for the day.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int forceMaxSpawnCount { get { return EnemyAIPatch.forceMaxSpawnCount; } set { EnemyAIPatch.forceMaxSpawnCount = value; } }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// Only accepts an EnemyAI instance of type SpringManAI.
    /// </summary>
    /// <param name="enemyAI">Coil-Head "Spring" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetToilHeadOnServer(EnemyAI enemyAI)
    {
        return Plugin.Instance.SetToilHeadOnServer(enemyAI);
    }
}
