using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class MantiToilControllerBehaviour : TurretHeadControllerBehaviour
{
    protected override Transform GetHeadTransform()
    {
        return transform.parent.Find("DoublewingModel").Find("BodyAnimContainer").GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).Find("Head").GetChild(0);
    }

    protected override void OnFinishedSetup()
    {
        TurretBehaviour.UseMantiToilSettings = true;

        if (IsServer)
        {
            EnemyAI enemyScript = GetEnemyScript();
            TurretHeadManager.AddEnemyTurretHeadControllerPair(enemyScript, this);
            TurretHeadManager.AddToEnemySpawnCount(enemyScript, TurretBehaviour.IsMinigun);
        }
    }
}
