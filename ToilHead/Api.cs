using com.github.zehsteam.ToilHead.Patches;
using System.Collections.Generic;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead;

public class Api
{
    public static ToilHeadData ToilHeadData => ToilHeadDataManager.GetToilHeadDataForCurrentLevel();
    public static int MaxSpawnCount => ToilHeadData.configData.maxSpawnCount;
    public static int SpawnChance => ToilHeadData.configData.spawnChance;

    public static Dictionary<NetworkObject, NetworkObject> enemyTurretPairs => EnemyAIPatch.enemyTurretPairs;
    public static int spawnCount => EnemyAIPatch.spawnCount;

    /// <summary>
    /// If enabled, will force any spawned Coil-Heads to become Toil-Heads. This will get reset automatically when the day ends.
    /// </summary>
    public static bool forceToilHeadSpawns { get { return EnemyAIPatch.forceToilHeadSpawns; } set { EnemyAIPatch.forceToilHeadSpawns = value; } }

    /// <summary>
    /// This must only be called on the Host/Server
    /// </summary>
    /// <param name="enemyAI">Coil-Head "Spring" EnemyAI instance.</param>
    /// <returns>True if successful.</returns>
    public static bool SetToilHeadOnServer(EnemyAI enemyAI)
    {
        return Plugin.Instance.SetToilHeadOnServer(enemyAI);
    }
}
