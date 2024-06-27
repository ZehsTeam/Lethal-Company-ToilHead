using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ParentToTransformBehaviour : MonoBehaviour
{
    private Transform _targetTransform;
    private Transform _parentTransform;

    private Vector3 _positionOffset;
    private Vector3 _rotationOffset;

    private void Update()
    {
        if (_targetTransform == null || _parentTransform == null) return;
        if (!_parentTransform.gameObject.activeSelf) return;

        _targetTransform.rotation = _parentTransform.rotation;
        _targetTransform.Rotate(_rotationOffset, Space.Self);

        Vector3 position = _parentTransform.position;
        position += _targetTransform.right * _positionOffset.x + _targetTransform.up * _positionOffset.y + _targetTransform.forward * _positionOffset.z;

        _targetTransform.position = position;
    }

    public void SetTargetAndParentTransform(Transform targetTransform, Transform parentTransform)
    {
        _targetTransform = targetTransform;
        _parentTransform = parentTransform;
    }

    public void SetPositionAndRotationOffset(Vector3 positionOffset, Vector3 rotationOffset)
    {
        _positionOffset = positionOffset;
        _rotationOffset = rotationOffset;
    }
}
