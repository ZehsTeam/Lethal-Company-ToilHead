using UnityEngine;

namespace com.github.zehsteam.ToilHead;

internal class ParentToTransformBehaviour : MonoBehaviour
{
    private Transform parent;
    private Vector3 positionOffset;
    private Vector3 rotationOffset;

    private void Update()
    {
        if (parent == null) return;

        Vector3 position = parent.localPosition + positionOffset;

        transform.localPosition = position;
        transform.localRotation = parent.localRotation;
        transform.Rotate(rotationOffset, Space.Self);
    }

    public void SetParent(Transform parent)
    {
        this.parent = parent;
    }

    public void SetPositionOffset(Vector3 positionOffset)
    {
        this.positionOffset = positionOffset;
    }

    public void SetRotationOffset(Vector3 rotationOffset)
    {
        this.rotationOffset = rotationOffset;
    }
}
