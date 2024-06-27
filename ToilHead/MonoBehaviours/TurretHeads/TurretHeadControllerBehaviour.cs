using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class TurretHeadControllerBehaviour : NetworkBehaviour
{
    [Header("Enemy Properties")]
    [Space(5f)]
    public string EnemyName = string.Empty;
    public bool HasScanNode = true;
    public string ScanNodeHeaderText = string.Empty;

    [Header("Turret Properties")]
    [Space(5f)]
    public TurretBehaviour TurretBehaviour = null;
    public Vector3 PositionOffset = Vector3.zero;
    public Vector3 RotationOffset = Vector3.zero;
    public Vector3 LocalScale = Vector3.one;

    protected virtual void Start()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        SetupTurret();
    }

    protected virtual void SetupTurret()
    {
        if (HasScanNode)
        {
            ScanNodeProperties scanNodeProperties = transform.parent.GetComponentInChildren<ScanNodeProperties>();
            scanNodeProperties.headerText = ScanNodeHeaderText;
        }

        TurretBehaviour = GetComponentInChildren<TurretBehaviour>();
        Transform turretTransform = TurretBehaviour.transform;
        turretTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        turretTransform.localScale = LocalScale;

        Transform headTransform = GetHeadTransform();

        ParentToTransformBehaviour behaviour = headTransform.gameObject.AddComponent<ParentToTransformBehaviour>();
        behaviour.SetTargetAndParentTransform(TurretBehaviour.SyncToHeadTransform, headTransform);
        behaviour.SetPositionAndRotationOffset(PositionOffset, RotationOffset);

        Plugin.Instance.LogInfoExtended($"Setup {EnemyName}");

        OnFinishedSetup();
    }

    protected virtual Transform GetHeadTransform()
    {
        return null;
    }

    protected virtual void OnFinishedSetup()
    {

    }

    protected EnemyAI GetEnemyScript()
    {
        return transform.parent.GetComponent<EnemyAI>();
    }

    protected PlayerControllerB GetPlayerScript()
    {
        return transform.parent.GetComponent<PlayerControllerB>();
    }

    protected PlayerControllerB GetDeadBodyPlayerScript()
    {
        if (!transform.parent.TryGetComponent(out RagdollGrabbableObject ragdollGrabbableObject))
        {
            return null;
        }

        return ragdollGrabbableObject.ragdoll.playerScript;
    }
}
