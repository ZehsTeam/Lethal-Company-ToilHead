using com.github.zehsteam.ToilHead.Patches;
using System.Collections.Generic;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead;

public class Api
{
    public static int SpawnChance => Plugin.Instance.ConfigManager.SpawnChance;
    public static int MaxSpawnCount => Plugin.Instance.ConfigManager.MaxSpawnCount;

    public static Dictionary<NetworkObject, NetworkObject> enemyTurretPairs => EnemyAIPatch.enemyTurretPairs;
    public static int spawnCount => EnemyAIPatch.spawnCount;

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
