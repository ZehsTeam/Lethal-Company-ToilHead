using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class SyncYRotationBehaviour : MonoBehaviour
{
    public Transform TargetTransform = null;

    private void Update()
    {
        if (TargetTransform == null) return;

        transform.rotation = Quaternion.Euler(new Vector3(0f, TargetTransform.eulerAngles.y, 0f));
    }
}
