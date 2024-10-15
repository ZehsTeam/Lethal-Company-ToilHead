using com.github.zehsteam.ToilHead.MonoBehaviours;
using com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;
using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public static class Api
{
    /// <summary>
    /// This is for all enemy turret pairs.
    /// </summary>
    public static Dictionary<EnemyAI, TurretHeadControllerBehaviour> EnemyTurretHeadControllerPairs => TurretHeadManager.EnemyTurretHeadControllerPairs;

    /// <summary>
    /// This is for all player turret pairs.
    /// </summary>
    public static Dictionary<PlayerControllerB, TurretHeadControllerBehaviour> PlayerTurretHeadControllerPairs => TurretHeadManager.PlayerTurretHeadControllerPairs;

    /// <summary>
    /// This is for all player ragdoll turret pairs.
    /// </summary>
    public static Dictionary<PlayerControllerB, TurretHeadControllerBehaviour> DeadBodyTurretHeadControllerPairs => TurretHeadManager.DeadBodyTurretHeadControllerPairs;

    #region Toil-Head
    /// <summary>
    /// Toil-Head data.
    /// </summary>
    public static TurretHeadData ToilHeadData => TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: false);

    /// <summary>
    /// If enabled, will force any spawned Coil-Heads to become Toil-Heads.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceToilHeadSpawns
    {
        get { return ToilHeadData.ForceSpawns; }
        set { ToilHeadData.ForceSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Toil-Head max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceToilHeadMaxSpawnCount
    {
        get { return ToilHeadData.ForceMaxSpawnCount; }
        set { ToilHeadData.ForceMaxSpawnCount = value; }
    }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Spring".
    /// </summary>
    /// <param name="enemyScript">Coil-Head "Spring" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetToilHeadOnServer(EnemyAI enemyScript)
    {
        return TurretHeadManager.SetEnemyTurretHeadOnServer(enemyScript, isSlayer: false);
    }
    #endregion

    #region Manti-Toil
    /// <summary>
    /// Manti-Toil data.
    /// </summary>
    public static TurretHeadData MantiToilData => TurretHeadManager.GetEnemyTurretHeadData("Manticoil", isSlayer: false);

    /// <summary>
    /// If enabled, will force any spawned Manticoils to become Manti-Toils.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceMantiToilSpawns
    {
        get { return MantiToilData.ForceSpawns; }
        set { MantiToilData.ForceSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Manti-Toil max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceMantiToilMaxSpawnCount
    {
        get { return MantiToilData.ForceMaxSpawnCount; }
        set { MantiToilData.ForceMaxSpawnCount = value; }
    }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Manticoil".
    /// </summary>
    /// <param name="enemyScript">Manticoil "Manticoil" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetMantiToilOnServer(EnemyAI enemyScript)
    {
        return TurretHeadManager.SetEnemyTurretHeadOnServer(enemyScript, isSlayer: false);
    }
    #endregion

    #region Toil-Slayer
    /// <summary>
    /// Toil-Slayer data.
    /// </summary>
    public static TurretHeadData ToilSlayerData => TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: true);

    /// <summary>
    /// If enabled, will force any spawned Coil-Heads to become Toil-Slayers.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceToilSlayerSpawns
    {
        get { return ToilSlayerData.ForceSpawns; }
        set { ToilSlayerData.ForceSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Toil-Slayer max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceToilSlayerMaxSpawnCount
    {
        get { return ToilSlayerData.ForceMaxSpawnCount; }
        set { ToilSlayerData.ForceMaxSpawnCount = value; }
    }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Spring".
    /// </summary>
    /// <param name="enemyScript">Coil-Head "Spring" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetToilSlayerOnServer(EnemyAI enemyScript)
    {
        return TurretHeadManager.SetEnemyTurretHeadOnServer(enemyScript, isSlayer: true);
    }
    #endregion

    #region Manti-Slayer
    /// <summary>
    /// Manti-Slayer data.
    /// </summary>
    public static TurretHeadData MantiSlayerData => TurretHeadManager.GetEnemyTurretHeadData("Manticoil", isSlayer: true);

    /// <summary>
    /// If enabled, will force any spawned Manticoils to become Manti-Slayers.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceMantiSlayerSpawns
    {
        get { return MantiSlayerData.ForceSpawns; }
        set { MantiSlayerData.ForceSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Manti-Slayer max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceMantiSlayerMaxSpawnCount
    {
        get { return MantiSlayerData.ForceMaxSpawnCount; }
        set { MantiSlayerData.ForceMaxSpawnCount = value; }
    }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Manticoil".
    /// </summary>
    /// <param name="enemyScript">Manticoil "Manticoil" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetMantiSlayerOnServer(EnemyAI enemyScript)
    {
        return TurretHeadManager.SetEnemyTurretHeadOnServer(enemyScript, isSlayer: true);
    }
    #endregion

    #region Toil-Player
    /// <summary>
    /// Toil-Player data.
    /// </summary>
    public static TurretHeadData ToilPlayerData => TurretHeadManager.PlayerTurretHeadData;

    /// <summary>
    /// If enabled, will force all Players to become Toil-Players when the round starts.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceToilPlayerSpawns
    {
        get { return ToilPlayerData.ForceSpawns; }
        set { ToilPlayerData.ForceSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Toil-Player max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceToilPlayerMaxSpawnCount
    {
        get { return ToilPlayerData.ForceMaxSpawnCount; }
        set { ToilPlayerData.ForceMaxSpawnCount = value; }
    }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// </summary>
    /// <returns>True if successful.</returns>
    public static bool SetToilPlayerOnServer(PlayerControllerB playerScript, bool isSlayer = false)
    {
        return TurretHeadManager.SetPlayerTurretHeadOnServer(playerScript, isSlayer);
    }
    #endregion

    #region Toil-Masked
    /// <summary>
    /// Toil-Masked data.
    /// </summary>
    public static TurretHeadData ToilMaskedData => TurretHeadManager.GetEnemyTurretHeadData("Masked", isSlayer: false);

    /// <summary>
    /// If enabled, will force any spawned Masked to become a Toil-Masked.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceToilMaskedSpawns
    {
        get { return ToilMaskedData.ForceSpawns; }
        set { ToilMaskedData.ForceSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Toil-Masked max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceToilMaskedMaxSpawnCount
    {
        get { return ToilMaskedData.ForceMaxSpawnCount; }
        set { ToilMaskedData.ForceMaxSpawnCount = value; }
    }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Masked".
    /// </summary>
    /// <param name="enemyScript">Masked "Masked" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetToilMaskedOnServer(EnemyAI enemyScript)
    {
        return TurretHeadManager.SetEnemyTurretHeadOnServer(enemyScript, isSlayer: false);
    }
    #endregion

    #region Slayer-Masked
    /// <summary>
    /// Slayer-Masked data.
    /// </summary>
    public static TurretHeadData SlayerMaskedData => TurretHeadManager.GetEnemyTurretHeadData("Masked", isSlayer: true);

    /// <summary>
    /// If enabled, will force any spawned Masked to become a Slayer-Masked.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static bool ForceSlayerMaskedSpawns
    {
        get { return SlayerMaskedData.ForceSpawns; }
        set { SlayerMaskedData.ForceSpawns = value; }
    }

    /// <summary>
    /// If set to any value above -1, will temporarily override the Slayer-Masked max spawn count.
    /// This will get reset automatically when the day ends.
    /// </summary>
    public static int ForceSlayerMaskedMaxSpawnCount
    {
        get { return SlayerMaskedData.ForceMaxSpawnCount; }
        set { SlayerMaskedData.ForceMaxSpawnCount = value; }
    }

    /// <summary>
    /// This must only be called on the Host/Server.
    /// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Masked".
    /// </summary>
    /// <param name="enemyScript">Masked "Masked" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetSlayerMaskedOnServer(EnemyAI enemyScript)
    {
        return TurretHeadManager.SetEnemyTurretHeadOnServer(enemyScript, isSlayer: true);
    }
    #endregion



    #region Deprecated
    [Obsolete("EnemyTurretPairs is deprecated, please use EnemyTurretHeadControllerPairs instead.", true)]
    public static Dictionary<EnemyAI, ToilHeadTurretBehaviour> EnemyTurretPairs => [];

    [Obsolete("PlayerTurretPairs is deprecated, please use PlayerTurretHeadControllerPairs instead.", true)]
    public static Dictionary<PlayerControllerB, ToilHeadTurretBehaviour> PlayerTurretPairs => [];

    [Obsolete("enemyTurretPairs is deprecated, please use EnemyTurretHeadControllerPairs instead.", true)]
    public static Dictionary<NetworkObject, NetworkObject> enemyTurretPairs => [];

    #region Toil-Head
    [Obsolete("MaxSpawnCount is deprecated, please use ToilHeadMaxSpawnCount instead.", true)]
    public static int MaxSpawnCount => TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: false).GetSpawnDataForCurrentMoon().MaxSpawnCount;

    [Obsolete("SpawnChance is deprecated, please use ToilHeadSpawnChance instead.", true)]
    public static float SpawnChance => TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: false).GetSpawnDataForCurrentMoon().SpawnChance;

    [Obsolete("spawnCount is deprecated, please use ToilHeadSpawnCount instead.", true)]
    public static int spawnCount => TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: false).SpawnCount;

    [Obsolete("forceSpawns is deprecated, please use ForceToilHeadSpawns instead.", true)]
    public static bool forceSpawns
    {
        get { return TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: false).ForceSpawns; }
        set { TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: false).ForceSpawns = value; }
    }

    [Obsolete("forceMaxSpawnCount is deprecated, please use ForceToilHeadMaxSpawnCount instead.", true)]
    public static int forceMaxSpawnCount
    {
        get { return TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: false).ForceMaxSpawnCount; }
        set { TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: false).ForceMaxSpawnCount = value; }
    }
    #endregion

    #region Manti-Toil
    [Obsolete("mantiToilSpawnCount is deprecated, please use MantiToilSpawnCount instead.", true)]
    public static int mantiToilSpawnCount => TurretHeadManager.GetEnemyTurretHeadData("Manticoil", isSlayer: false).SpawnCount;

    [Obsolete("forceMantiToilSpawns is deprecated, please use ForceMantiToilSpawns instead.", true)]
    public static bool forceMantiToilSpawns
    {
        get { return TurretHeadManager.GetEnemyTurretHeadData("Manticoil", isSlayer: false).ForceSpawns; }
        set { TurretHeadManager.GetEnemyTurretHeadData("Manticoil", isSlayer: false).ForceSpawns = value; }
    }

    [Obsolete("forceMantiToilMaxSpawnCount is deprecated, please use ForceMantiToilMaxSpawnCount instead.", true)]
    public static int forceMantiToilMaxSpawnCount
    {
        get { return TurretHeadManager.GetEnemyTurretHeadData("Manticoil", isSlayer: false).ForceMaxSpawnCount; }
        set { TurretHeadManager.GetEnemyTurretHeadData("Manticoil", isSlayer: false).ForceMaxSpawnCount = value; }
    }
    #endregion

    #region Toil-Slayer
    [Obsolete("toilSlayerSpawnCount is deprecated, please use ToilSlayerSpawnCount instead.", true)]
    public static int toilSlayerSpawnCount => TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: true).SpawnCount;

    [Obsolete("forceToilSlayerSpawns is deprecated, please use ForceToilSlayerSpawns instead.", true)]
    public static bool forceToilSlayerSpawns
    {
        get { return TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: true).ForceSpawns; }
        set { TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: true).ForceSpawns = value; }
    }

    [Obsolete("forceToilSlayerMaxSpawnCount is deprecated, please use ForceToilSlayerMaxSpawnCount instead.", true)]
    public static int forceToilSlayerMaxSpawnCount
    {
        get { return TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: true).ForceMaxSpawnCount; }
        set { TurretHeadManager.GetEnemyTurretHeadData("Spring", isSlayer: true).ForceMaxSpawnCount = value; }
    }
    #endregion
    #endregion
}
