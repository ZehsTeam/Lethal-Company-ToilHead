using com.github.zehsteam.ToilHead.Patches;
using System.Collections.Generic;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead;

public class Api
{
    /// <summary>
    /// Toil-Head data.
    /// </summary>
    public static ToilHeadData ToilHeadData => ToilHeadDataManager.GetDataForCurrentLevel();

    /// <summary>
    /// Toil-Head max spawn count.
    /// </summary>
    public static int MaxSpawnCount => ToilHeadData.configData.maxSpawnCount;

    /// <summary>
    /// Toil-Head spawn chance.
    /// </summary>
    public static int SpawnChance => ToilHeadData.configData.spawnChance;

    /// <summary>
    /// This is for all enemy turret pairs.
    /// </summary>
    public static Dictionary<NetworkObject, NetworkObject> enemyTurretPairs => EnemyAIPatch.enemyTurretPairs;

    /// <summary>
    /// Toil-Head spawn count.
    /// </summary>
    public static int spawnCount => EnemyAIPatch.toilHeadSpawnCount;

    /// <summary>
    /// If enabled, will force any spawned Coil-Heads to become Toil-Heads.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool forceSpawns { get { return EnemyAIPatch.forceToilHeadSpawns; } set { EnemyAIPatch.forceToilHeadSpawns = value; } }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Toil-Head max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int forceMaxSpawnCount { get { return EnemyAIPatch.forceToilHeadMaxSpawnCount; } set { EnemyAIPatch.forceToilHeadMaxSpawnCount = value; } }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Spring".
    /// </summary>
    /// <param name="enemyAI">Coil-Head "Spring" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetToilHeadOnServer(EnemyAI enemyAI)
    {
        return Plugin.Instance.SetToilHeadOnServer(enemyAI);
    }

    /// <summary>
    /// Manti-Toil max spawn count.
    /// </summary>
    public static int MantiToilMaxSpawnCount => Plugin.ConfigManager.MantiToilMaxSpawnCount.Value;

    /// <summary>
    /// Manti-Toil spawn chance.
    /// </summary>
    public static int MantiToilSpawnChance => Plugin.ConfigManager.MantiToilSpawnChance.Value;

    /// <summary>
    /// Manti-Toil spawn count.
    /// </summary>
    public static int mantiToilSpawnCount => EnemyAIPatch.mantiToilSpawnCount;

    /// <summary>
    /// If enabled, will force any spawned Manticoils to become Manti-Toils.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool forceMantiToilSpawns { get { return EnemyAIPatch.forceMantiToilSpawns; } set { EnemyAIPatch.forceMantiToilSpawns = value; } }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Manti-Toil max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int forceMantiToilMaxSpawnCount { get { return EnemyAIPatch.forceMantiToilMaxSpawnCount; } set { EnemyAIPatch.forceMantiToilMaxSpawnCount = value; } }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Manticoil".
    /// </summary>
    /// <param name="enemyAI">Manticoil "Manticoil" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetMantiToilOnServer(EnemyAI enemyAI)
    {
        return Plugin.Instance.SetMantiToilOnServer(enemyAI);
    }
}
