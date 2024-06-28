using GameNetcodeStuff;
using System.Collections;
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

    protected ParentToTransformBehaviour _parentToTransformBehaviour;

    protected virtual void Start()
    {
        if (!IsServer)
        {
            SetupTurret();
        }

        StartCoroutine(LateStartCO());
    }

    protected IEnumerator LateStartCO()
    {
        yield return null;

        LateStart();
    }

    protected virtual void LateStart()
    {
        SetPTTBOffsetValues();
    }

    public virtual void SetupTurret()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

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

        Transform PTTBContainerTransform = GetPTTBContainerTransform();
        if (PTTBContainerTransform == null) PTTBContainerTransform = headTransform;

        _parentToTransformBehaviour = PTTBContainerTransform.gameObject.AddComponent<ParentToTransformBehaviour>();
        _parentToTransformBehaviour.SetTargetAndParentTransform(TurretBehaviour.SyncToHeadTransform, headTransform);
        SetPTTBOffsetValues();

        Plugin.Instance.LogInfoExtended($"Setup {EnemyName}");

        OnFinishedSetup();
    }

    protected virtual Transform GetHeadTransform()
    {
        return null;
    }

    protected virtual Transform GetPTTBContainerTransform()
    {
        return null;
    }

    protected virtual void SetPTTBOffsetValues()
    {
        Vector3 parentLocalScale = transform.parent.localScale;

        Vector3 positionOffset = PositionOffset;
        positionOffset.x *= parentLocalScale.x;
        positionOffset.y *= parentLocalScale.y;
        positionOffset.z *= parentLocalScale.z;

        _parentToTransformBehaviour.SetPositionAndRotationOffset(positionOffset, RotationOffset);
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
