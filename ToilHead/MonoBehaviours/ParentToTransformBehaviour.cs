using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ParentToTransformBehaviour : MonoBehaviour
{
    private Transform targetTransform;
    private Transform parentTransform;

    private Vector3 positionOffset;
    private Vector3 rotationOffset;

    private void Update()
    {
        if (targetTransform == null || parentTransform == null) return;

        targetTransform.rotation = parentTransform.rotation;
        targetTransform.Rotate(rotationOffset, Space.Self);

        Vector3 position = parentTransform.position;
        position += targetTransform.right * positionOffset.x + targetTransform.up * positionOffset.y + targetTransform.forward * positionOffset.z;

        targetTransform.position = position;
    }

    public void SetTargetAndParent(Transform target, Transform parent)
    {
        targetTransform = target;
        parentTransform = parent;
    }

    public void SetPositionOffset(Vector3 offset)
    {
        positionOffset = offset;
    }

    public void SetRotationOffset(Vector3 offset)
    {
        rotationOffset = offset;
    }
}
