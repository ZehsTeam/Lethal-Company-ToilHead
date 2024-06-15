using com.github.zehsteam.ToilHead.Patches;
using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead;

/// <summary>
/// 
/// </summary>
public class Api
{
    /// <summary>
    /// This is for all enemy turret pairs.
    /// </summary>
    public static Dictionary<NetworkObject, NetworkObject> EnemyTurretPairs => EnemyAIPatch.EnemyTurretPairs;

    #region Toil-Head
    /// <summary>
    /// Toil-Head max spawn count.
    /// </summary>
    public static int ToilHeadMaxSpawnCount => SpawnDataManager.GetToilHeadSpawnDataForCurrentMoon().MaxSpawnCount;

    /// <summary>
    /// Toil-Head spawn chance.
    /// </summary>
    public static int ToilHeadSpawnChance => SpawnDataManager.GetToilHeadSpawnDataForCurrentMoon().SpawnChance;

    /// <summary>
    /// Toil-Head spawn count.
    /// </summary>
    public static int ToilHeadSpawnCount => EnemyAIPatch.ToilHeadSpawnCount;

    /// <summary>
    /// If enabled, will force any spawned Coil-Heads to become Toil-Heads.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceToilHeadSpawns
    {
        get { return EnemyAIPatch.ForceToilHeadSpawns; }
        set { EnemyAIPatch.ForceToilHeadSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Toil-Head max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceToilHeadMaxSpawnCount
    {
        get { return EnemyAIPatch.ForceToilHeadMaxSpawnCount; }
        set { EnemyAIPatch.ForceToilHeadMaxSpawnCount = value; }
    }

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
    #endregion

    #region Manti-Toil
    /// <summary>
    /// Manti-Toil max spawn count.
    /// </summary>
    public static int MantiToilMaxSpawnCount => SpawnDataManager.GetMantiToilSpawnDataForCurrentMoon().MaxSpawnCount;

    /// <summary>
    /// Manti-Toil spawn chance.
    /// </summary>
    public static int MantiToilSpawnChance => SpawnDataManager.GetMantiToilSpawnDataForCurrentMoon().SpawnChance;

    /// <summary>
    /// Manti-Toil spawn count.
    /// </summary>
    public static int MantiToilSpawnCount => EnemyAIPatch.MantiToilSpawnCount;

    /// <summary>
    /// If enabled, will force any spawned Manticoils to become Manti-Toils.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceMantiToilSpawns
    {
        get { return EnemyAIPatch.ForceMantiToilSpawns; }
        set { EnemyAIPatch.ForceMantiToilSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Manti-Toil max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceMantiToilMaxSpawnCount
    {
        get { return EnemyAIPatch.ForceMantiToilMaxSpawnCount; }
        set { EnemyAIPatch.ForceMantiToilMaxSpawnCount = value; }
    }

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
    #endregion

    #region Toil-Slayer
    /// <summary>
    /// Toil-Slayer max spawn count.
    /// </summary>
    public static int ToilSlayerMaxSpawnCount => SpawnDataManager.GetToilSlayerSpawnDataForCurrentMoon().MaxSpawnCount;

    /// <summary>
    /// Toil-Slayer spawn chance.
    /// </summary>
    public static int ToilSlayerSpawnChance => SpawnDataManager.GetToilSlayerSpawnDataForCurrentMoon().SpawnChance;

    /// <summary>
    /// Toil-Slayer spawn count.
    /// </summary>
    public static int ToilSlayerSpawnCount => EnemyAIPatch.ToilSlayerSpawnCount;

    /// <summary>
    /// If enabled, will force any spawned Coil-Heads to become Toil-Slayers.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceToilSlayerSpawns
    {
        get { return EnemyAIPatch.ForceToilSlayerSpawns; }
        set { EnemyAIPatch.ForceToilSlayerSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Toil-Slayer max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceToilSlayerMaxSpawnCount
    {
        get { return EnemyAIPatch.ForceToilSlayerMaxSpawnCount; }
        set { EnemyAIPatch.ForceToilSlayerMaxSpawnCount = value; }
    }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Spring".
    /// </summary>
    /// <param name="enemyAI">Coil-Head "Spring" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetToilSlayerOnServer(EnemyAI enemyAI)
    {
        return Plugin.Instance.SetToilSlayerOnServer(enemyAI);
    }
    #endregion



    #region Deprecated
    /// <summary>
    /// This is for all enemy turret pairs.
    /// </summary>
    [Obsolete("enemyTurretPairs is deprecated, please use EnemyTurretPairs instead.", true)]
    public static Dictionary<NetworkObject, NetworkObject> enemyTurretPairs => EnemyAIPatch.EnemyTurretPairs;

    #region Toil-Head
    /// <summary>
    /// Toil-Head max spawn count.
    /// </summary>
    [Obsolete("MaxSpawnCount is deprecated, please use ToilHeadMaxSpawnCount instead.", true)]
    public static int MaxSpawnCount => SpawnDataManager.GetToilHeadSpawnDataForCurrentMoon().MaxSpawnCount;

    /// <summary>
    /// Toil-Head spawn chance.
    /// </summary>
    [Obsolete("SpawnChance is deprecated, please use ToilHeadSpawnChance instead.", true)]
    public static int SpawnChance => SpawnDataManager.GetToilHeadSpawnDataForCurrentMoon().SpawnChance;

    /// <summary>
    /// Toil-Head spawn count.
    /// </summary>
    [Obsolete("spawnCount is deprecated, please use ToilHeadSpawnCount instead.", true)]
    public static int spawnCount => EnemyAIPatch.ToilHeadSpawnCount;

    /// <summary>
    /// If enabled, will force any spawned Coil-Heads to become Toil-Heads.
    /// This will get reset automatically when the day ends.
    /// </summary>
    [Obsolete("forceSpawns is deprecated, please use ForceToilHeadSpawns instead.", true)]
    public static bool forceSpawns { get { return EnemyAIPatch.ForceToilHeadSpawns; } set { EnemyAIPatch.ForceToilHeadSpawns = value; } }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Toil-Head max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    [Obsolete("forceMaxSpawnCount is deprecated, please use ForceToilHeadMaxSpawnCount instead.", true)]
    public static int forceMaxSpawnCount { get { return EnemyAIPatch.ForceToilHeadMaxSpawnCount; } set { EnemyAIPatch.ForceToilHeadMaxSpawnCount = value; } }
    #endregion

    #region Manti-Toil
    /// <summary>
    /// Manti-Toil spawn count.
    /// </summary>
    [Obsolete("mantiToilSpawnCount is deprecated, please use MantiToilSpawnCount instead.", true)]
    public static int mantiToilSpawnCount => EnemyAIPatch.MantiToilSpawnCount;

    /// <summary>
    /// If enabled, will force any spawned Manticoils to become Manti-Toils.
    /// This will get reset automatically when the day ends.
    /// </summary>
    [Obsolete("forceMantiToilSpawns is deprecated, please use ForceMantiToilSpawns instead.", true)]
    public static bool forceMantiToilSpawns { get { return EnemyAIPatch.ForceMantiToilSpawns; } set { EnemyAIPatch.ForceMantiToilSpawns = value; } }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Manti-Toil max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    [Obsolete("forceMantiToilMaxSpawnCount is deprecated, please use ForceMantiToilMaxSpawnCount instead.", true)]
    public static int forceMantiToilMaxSpawnCount { get { return EnemyAIPatch.ForceMantiToilMaxSpawnCount; } set { EnemyAIPatch.ForceMantiToilMaxSpawnCount = value; } }
    #endregion

    #region Toil-Slayer
    /// <summary>
    /// Toil-Slayer spawn count.
    /// </summary>
    [Obsolete("toilSlayerSpawnCount is deprecated, please use ToilSlayerSpawnCount instead.", true)]
    public static int toilSlayerSpawnCount => EnemyAIPatch.ToilSlayerSpawnCount;

    /// <summary>
    /// If enabled, will force any spawned Coil-Heads to become Toil-Slayers.
    /// This will get reset automatically when the day ends.
    /// </summary>
    [Obsolete("forceToilSlayerSpawns is deprecated, please use ForceToilSlayerSpawns instead.", true)]
    public static bool forceToilSlayerSpawns { get { return EnemyAIPatch.ForceToilSlayerSpawns; } set { EnemyAIPatch.ForceToilSlayerSpawns = value; } }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Toil-Slayer max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    [Obsolete("forceToilSlayerMaxSpawnCount is deprecated, please use ForceToilSlayerMaxSpawnCount instead.", true)]
    public static int forceToilSlayerMaxSpawnCount { get { return EnemyAIPatch.ForceToilSlayerMaxSpawnCount; } set { EnemyAIPatch.ForceToilSlayerMaxSpawnCount = value; } }
    #endregion
    #endregion
}
