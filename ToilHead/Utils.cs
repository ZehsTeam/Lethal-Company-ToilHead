using com.github.zehsteam.ToilHead.MonoBehaviours;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

public class Utils
{
    public static bool RandomPercent(int percent)
    {
        if (percent <= 0) return false;
        if (percent >= 100) return true;
        return Random.value <= percent * 0.01f;
    }

    public static void DisableColliders(GameObject gameObject, bool keepScanNodeEnabled = false)
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

        foreach (var collider in colliders)
        {
            if (keepScanNodeEnabled && collider.gameObject.name == "ScanNode")
            {
                continue;
            }

            collider.enabled = false;
        }
    }

    public static bool IsSpring(EnemyAI enemyAI)
    {
        return enemyAI.enemyType.enemyName == "Spring";
    }

    public static bool IsToilHead(EnemyAI enemyAI)
    {
        if (!IsSpring(enemyAI)) return false;

        return enemyAI.GetComponentInChildren<ToilHeadTurretBehaviour>() != null;
    }

    public static ToilHeadTurretBehaviour GetToilHeadTurretBehaviour(EnemyAI enemyAI)
    {
        return enemyAI.GetComponentInChildren<ToilHeadTurretBehaviour>();
    }
}
