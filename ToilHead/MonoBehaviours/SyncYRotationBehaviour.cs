using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class SyncYRotationBehaviour : MonoBehaviour
{
    public Transform targetTransform = null;

    private void Update()
    {
        if (targetTransform == null) return;

        transform.rotation = Quaternion.Euler(new Vector3(0f, targetTransform.eulerAngles.y, 0f));
    }
}
