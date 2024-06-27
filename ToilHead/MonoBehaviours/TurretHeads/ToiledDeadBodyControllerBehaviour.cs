using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ToiledDeadBodyControllerBehaviour : TurretHeadControllerBehaviour
{
    protected override Transform GetHeadTransform()
    {
        return transform.parent.Find("spine.003").Find("spine.004").Find("SpringContainer").Find("SpringMetarig").GetChild(0).GetChild(0).GetChild(0).GetChild(0);
    }

    protected override void OnFinishedSetup()
    {
        TurretHeadManager.AddDeadBodyTurretHeadControllerPair(GetDeadBodyPlayerScript(), this);
    }
}
