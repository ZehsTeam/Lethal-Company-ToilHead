using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ToilHeadControllerBehaviour : TurretHeadControllerBehaviour
{
    protected override Transform GetHeadTransform()
    {
        return transform.parent.Find("SpringManModel").Find("Head");
    }

    protected override void OnFinishedSetup()
    {
        EnemyAI enemyScript = GetEnemyScript();
        TurretHeadManager.AddEnemyTurretHeadControllerPair(enemyScript, this);
        TurretHeadManager.AddToSpawnCount(enemyScript, TurretBehaviour.IsMinigun);
    }
}
