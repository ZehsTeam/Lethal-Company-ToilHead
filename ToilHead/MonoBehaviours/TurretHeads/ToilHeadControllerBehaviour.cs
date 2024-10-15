using com.github.zehsteam.ToilHead.Compatibility;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ToilHeadControllerBehaviour : TurretHeadControllerBehaviour
{
    protected override void LateStart()
    {
        base.LateStart();

        if (FNaFEndoCoilheadProxy.HasMod || ThiccCoilHeadProxy.HasMod)
        {
            _parentToTransformBehaviour.SetTargetAndParentTransform(TurretBehaviour.SyncToHeadTransform, GetHeadTransform());
        }
    }

    public override void SetupTurret()
    {
        if (SCP173CoilheadSFXProxy.HasMod)
        {
            PositionOffset = new Vector3(0f, 4.35f, -0.075f);
            RotationOffset = Vector3.zero;
        }

        if (WeepingAngelsProxy.HasMod)
        {
            PositionOffset = new Vector3(0f, 3.6f, -0.65f);
            RotationOffset = Vector3.zero;
        }

        if (FNaFEndoCoilheadProxy.HasMod)
        {
            PositionOffset = new Vector3(-0.033f, 0.55f, 0.05f);
            RotationOffset = Vector3.zero;
        }

        if (ThiccCoilHeadProxy.HasMod)
        {
            PositionOffset = new Vector3(-0.02f, 1.2f, -0.075f);
            RotationOffset = Vector3.zero;
        }

        if (ARatherSillyCoilHeadProxy.HasMod)
        {
            PositionOffset = new Vector3(0f, 0.25f, -0.07f);
        }

        base.SetupTurret();
    }

    protected override Transform GetHeadTransform()
    {
        if (SCP173CoilheadSFXProxy.HasMod || WeepingAngelsProxy.HasMod)
        {
            return transform.parent.Find("SpringManModel");
        }

        if (FNaFEndoCoilheadProxy.HasMod || ThiccCoilHeadProxy.HasMod)
        {
            return transform.parent.Find("SpringManModel").Find("AnimContainer").Find("metarig").GetChild(0).GetChild(0).GetChild(0).GetChild(0).Find("springBone");
        }

        return transform.parent.Find("SpringManModel").Find("Head");
    }

    protected override Transform GetPTTBContainerTransform()
    {
        return transform.parent.Find("SpringManModel").Find("Head");
    }

    protected override void OnFinishedSetup()
    {
        if (IsServer)
        {
            EnemyAI enemyScript = GetEnemyScript();
            TurretHeadManager.AddEnemyTurretHeadControllerPair(enemyScript, this);
            TurretHeadManager.AddToEnemySpawnCount(enemyScript, TurretBehaviour.IsMinigun);
        }
    }
}
