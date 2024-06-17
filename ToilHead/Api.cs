using com.github.zehsteam.ToilHead.MonoBehaviours;
using com.github.zehsteam.ToilHead.Patches;
using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class Api
{
    /// <summary>
    /// This is for all enemy turret pairs.
    /// </summary>
    public static Dictionary<EnemyAI, ToilHeadTurretBehaviour> EnemyTurretPairs => EnemyAIPatch.EnemyTurretPairs;

    /// <summary>
    /// This is for all player turret pairs.
    /// </summary>
    public static Dictionary<PlayerControllerB, ToilHeadTurretBehaviour> PlayerTurretPairs => PlayerControllerBPatch.PlayerTurretPairs;

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
        return Plugin.Instance.SetToilHeadOnServer(enemyAI, isSlayer: true);
    }
    #endregion

    #region Manti-Slayer
    /// <summary>
    /// Manti-Slayer max spawn count.
    /// </summary>
    public static int MantiSlayerMaxSpawnCount => SpawnDataManager.GetMantiSlayerSpawnDataForCurrentMoon().MaxSpawnCount;

    /// <summary>
    /// Manti-Slayer spawn chance.
    /// </summary>
    public static int MantiSlayerSpawnChance => SpawnDataManager.GetMantiSlayerSpawnDataForCurrentMoon().SpawnChance;

    /// <summary>
    /// Manti-Slayer spawn count.
    /// </summary>
    public static int MantiSlayerSpawnCount => EnemyAIPatch.MantiSlayerSpawnCount;

    /// <summary>
    /// If enabled, will force any spawned Manticoils to become Manti-Slayers.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceMantiSlayerSpawns
    {
        get { return EnemyAIPatch.ForceMantiSlayerSpawns; }
        set { EnemyAIPatch.ForceMantiSlayerSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Manti-Slayer max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceMantiSlayerMaxSpawnCount
    {
        get { return EnemyAIPatch.ForceMantiSlayerMaxSpawnCount; }
        set { EnemyAIPatch.ForceMantiSlayerMaxSpawnCount = value; }
    }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Manticoil".
    /// </summary>
    /// <param name="enemyAI">Manticoil "Manticoil" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetMantiSlayerOnServer(EnemyAI enemyAI)
    {
        return Plugin.Instance.SetMantiToilOnServer(enemyAI, isSlayer: true);
    }
    #endregion

    #region Toil-Player
    /// <summary>
    /// Toil-Player max spawn count.
    /// </summary>
    public static int ToilPlayerMaxSpawnCount => SpawnDataManager.GetToilPlayerSpawnDataForCurrentMoon().MaxSpawnCount;

    /// <summary>
    /// Toil-Player spawn chance.
    /// </summary>
    public static int ToilPlayerdSpawnChance => SpawnDataManager.GetToilPlayerSpawnDataForCurrentMoon().SpawnChance;

    /// <summary>
    /// Toil-Player spawn count.
    /// </summary>
    public static int ToilPlayerSpawnCount => PlayerControllerBPatch.ToilPlayerSpawnCount;

    /// <summary>
    /// If enabled, will force all Players to become Toil-Players when the round starts.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceToilPlayerSpawns
    {
        get { return PlayerControllerBPatch.ForceToilPlayerSpawns; }
        set { PlayerControllerBPatch.ForceToilPlayerSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Toil-Player max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceToilPlayerMaxSpawnCount
    {
        get { return PlayerControllerBPatch.ForceToilPlayerMaxSpawnCount; }
        set { PlayerControllerBPatch.ForceToilPlayerMaxSpawnCount = value; }
    }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// </summary>
    /// <returns>True if successful.</returns>
    public static bool SetToilPlayerOnServer(PlayerControllerB playerScript, bool isSlayer = false)
    {
        return Plugin.Instance.SetToilPlayerOnServer(playerScript, isSlayer);
    }
    #endregion



    #region Deprecated
    [Obsolete("enemyTurretPairs is deprecated, please use EnemyTurretPairs instead.", true)]
    public static Dictionary<NetworkObject, NetworkObject> enemyTurretPairs => [];

    #region Toil-Head
    [Obsolete("MaxSpawnCount is deprecated, please use ToilHeadMaxSpawnCount instead.", true)]
    public static int MaxSpawnCount => SpawnDataManager.GetToilHeadSpawnDataForCurrentMoon().MaxSpawnCount;

    [Obsolete("SpawnChance is deprecated, please use ToilHeadSpawnChance instead.", true)]
    public static int SpawnChance => SpawnDataManager.GetToilHeadSpawnDataForCurrentMoon().SpawnChance;

    [Obsolete("spawnCount is deprecated, please use ToilHeadSpawnCount instead.", true)]
    public static int spawnCount => EnemyAIPatch.ToilHeadSpawnCount;

    [Obsolete("forceSpawns is deprecated, please use ForceToilHeadSpawns instead.", true)]
    public static bool forceSpawns { get { return EnemyAIPatch.ForceToilHeadSpawns; } set { EnemyAIPatch.ForceToilHeadSpawns = value; } }

    [Obsolete("forceMaxSpawnCount is deprecated, please use ForceToilHeadMaxSpawnCount instead.", true)]
    public static int forceMaxSpawnCount { get { return EnemyAIPatch.ForceToilHeadMaxSpawnCount; } set { EnemyAIPatch.ForceToilHeadMaxSpawnCount = value; } }
    #endregion

    #region Manti-Toil
    [Obsolete("mantiToilSpawnCount is deprecated, please use MantiToilSpawnCount instead.", true)]
    public static int mantiToilSpawnCount => EnemyAIPatch.MantiToilSpawnCount;

    [Obsolete("forceMantiToilSpawns is deprecated, please use ForceMantiToilSpawns instead.", true)]
    public static bool forceMantiToilSpawns { get { return EnemyAIPatch.ForceMantiToilSpawns; } set { EnemyAIPatch.ForceMantiToilSpawns = value; } }

    [Obsolete("forceMantiToilMaxSpawnCount is deprecated, please use ForceMantiToilMaxSpawnCount instead.", true)]
    public static int forceMantiToilMaxSpawnCount { get { return EnemyAIPatch.ForceMantiToilMaxSpawnCount; } set { EnemyAIPatch.ForceMantiToilMaxSpawnCount = value; } }
    #endregion

    #region Toil-Slayer
    [Obsolete("toilSlayerSpawnCount is deprecated, please use ToilSlayerSpawnCount instead.", true)]
    public static int toilSlayerSpawnCount => EnemyAIPatch.ToilSlayerSpawnCount;

    [Obsolete("forceToilSlayerSpawns is deprecated, please use ForceToilSlayerSpawns instead.", true)]
    public static bool forceToilSlayerSpawns { get { return EnemyAIPatch.ForceToilSlayerSpawns; } set { EnemyAIPatch.ForceToilSlayerSpawns = value; } }

    [Obsolete("forceToilSlayerMaxSpawnCount is deprecated, please use ForceToilSlayerMaxSpawnCount instead.", true)]
    public static int forceToilSlayerMaxSpawnCount { get { return EnemyAIPatch.ForceToilSlayerMaxSpawnCount; } set { EnemyAIPatch.ForceToilSlayerMaxSpawnCount = value; } }
    #endregion
    #endregion
}
