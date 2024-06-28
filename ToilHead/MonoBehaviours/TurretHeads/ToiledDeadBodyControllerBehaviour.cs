using System.Collections;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ToiledDeadBodyControllerBehaviour : TurretHeadControllerBehaviour
{
    protected override void Start()
    {
        if (IsServer)
        {
            base.Start();
        }
        else
        {
            StartCoroutine(LateSetupTurret());
        }
    }

    private IEnumerator LateSetupTurret()
    {
        RagdollGrabbableObject ragdollGrabbableObject = transform.parent.GetComponent<RagdollGrabbableObject>();

        yield return Utils.WaitUntil(() =>
        {
            return ragdollGrabbableObject.ragdoll != null;
        });

        if (ragdollGrabbableObject.ragdoll == null)
        {
            Plugin.logger.LogError("Error: Failed to late setup player ragdoll Turret-Head turret. DeadBodyInfo is null.");
            yield break;
        }

        yield return null;

        SetupTurret();
    }

    protected override Transform GetHeadTransform()
    {
        RagdollGrabbableObject ragdollGrabbableObject = transform.parent.GetComponent<RagdollGrabbableObject>();

        if (ragdollGrabbableObject == null)
        {
            Plugin.logger.LogError("Error: Failed to setup player ragdoll Turret-Head turret. RagdollGrabbableObject is null.");
            return null;
        }

        if (ragdollGrabbableObject.ragdoll == null)
        {
            Plugin.logger.LogError("Error: Failed to setup player ragdoll Turret-Head turret. DeadBodyInfo is null.");
            return null;
        }

        return ragdollGrabbableObject.ragdoll.transform.Find("spine.001").Find("spine.002").Find("spine.003").Find("spine.004").Find("SpringContainer").Find("SpringMetarig").GetChild(0).GetChild(0).GetChild(0).GetChild(0);
    }

    protected override void OnFinishedSetup()
    {
        if (IsServer)
        {
            if (!Plugin.ConfigManager.SpawnRealToiledPlayerRagdolls.Value)
            {
                TurretBehaviour.SetCanTargetPlayersClientRpc(false);
            }

            TurretHeadManager.AddDeadBodyTurretHeadControllerPair(GetDeadBodyPlayerScript(), this);
        }
    }
}
