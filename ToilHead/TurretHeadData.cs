using UnityEngine;

namespace com.github.zehsteam.ToilHead;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class TurretHeadData
{
    public string EnemyName { get; private set; }
    public bool IsSlayer { get; private set; }
    public GameObject ControllerPrefab { get; private set; }
    public MoonSpawnDataList MoonSpawnDataList { get; private set; }
    public SpawnData ToilationSpawnData { get; private set; }

    public int SpawnCount { get; private set; }
    public bool ForceSpawns = false;
    public int ForceMaxSpawnCount = -1;

    public TurretHeadData(string enemyName, bool isSlayer, GameObject controllerPrefab, MoonSpawnDataList moonSpawnDataList, SpawnData toilationSpawnData)
    {
        EnemyName = enemyName;
        IsSlayer = isSlayer;
        ControllerPrefab = controllerPrefab;
        MoonSpawnDataList = moonSpawnDataList;
        ToilationSpawnData = toilationSpawnData;
    }

    public void Reset()
    {
        SpawnCount = 0;
        ForceSpawns = false;
        ForceMaxSpawnCount = -1;
    }

    public void AddToSpawnCount()
    {
        SpawnCount++;
    }

    public SpawnData GetSpawnDataForCurrentMoon()
    {
        if (Utils.IsCurrentMoonToilation())
        {
            return ToilationSpawnData;
        }

        return MoonSpawnDataList.GetSpawnDataForCurrentMoon();
    }
}
