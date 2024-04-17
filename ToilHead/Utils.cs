using com.github.zehsteam.ToilHead.MonoBehaviours;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

public class Utils
{
    public static bool RandomPercent(int percent)
    {
        if (percent <= 0) return false;
        if (percent >= 100) return true;
        return Random.Range(1f, 100f) <= percent;
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

        return enemyAI.transform.Find("ToilHeadTurretContainer(Clone)") != null;
    }

    public static ToilHeadTurretBehaviour GetToilHeadTurretBehaviour(EnemyAI enemyAI)
    {
        Transform toilHeadTurretTransform = enemyAI.transform.Find("ToilHeadTurretContainer(Clone)");
        if (toilHeadTurretTransform == null) return null;

        return toilHeadTurretTransform.gameObject.GetComponentInChildren<ToilHeadTurretBehaviour>();
    }
}
