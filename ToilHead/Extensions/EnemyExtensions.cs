using com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;

namespace com.github.zehsteam.ToilHead.Extensions;

internal static class EnemyExtensions
{
    public static bool IsValidEnemy(this EnemyAI enemyScript)
    {
        if (IsSpring(enemyScript)) return true;
        if (IsManticoil(enemyScript)) return true;
        if (IsMasked(enemyScript)) return true;

        return false;
    }

    public static bool IsSpring(this EnemyAI enemyScript)
    {
        return enemyScript.enemyType.enemyName == "Spring";
    }

    public static bool IsManticoil(this EnemyAI enemyScript)
    {
        return enemyScript.enemyType.enemyName == "Manticoil";
    }

    public static bool IsMasked(this EnemyAI enemyScript)
    {
        return enemyScript.enemyType.enemyName == "Masked";
    }

    public static bool IsTurretHead(this EnemyAI enemyScript)
    {
        return enemyScript.GetComponentInChildren<TurretHeadControllerBehaviour>() != null;
    }
}
