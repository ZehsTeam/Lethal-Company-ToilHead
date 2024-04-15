using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

public class SyncYRotationBehaviour : MonoBehaviour
{
    public Transform targetTransform = null;

    private void Update()
    {
        if (targetTransform == null) return;

        transform.rotation = Quaternion.Euler(new Vector3(0f, targetTransform.eulerAngles.y, 0f));
    }
}
