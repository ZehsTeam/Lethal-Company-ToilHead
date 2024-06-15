using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ToilSlayerBehaviour : NetworkBehaviour
{
    public Material springManMaterial = null;

    private void Start()
    {
        if (!IsCoilHead()) return;

        SetCoilHeadMaterial();
    }

    private void SetCoilHeadMaterial()
    {
        Transform springManModelTransfrom = transform.parent.GetChild(0);

        Renderer headRenderer = springManModelTransfrom.Find("Head").GetComponent<Renderer>();
        Renderer bodyRenderer = springManModelTransfrom.Find("Body").GetComponent<Renderer>();

        headRenderer.materials = [springManMaterial];

        Material[] bodyMaterials = bodyRenderer.materials;
        bodyMaterials[1] = springManMaterial;
        bodyRenderer.materials = bodyMaterials;
    }

    private bool IsCoilHead()
    {
        try
        {
            if (transform.parent.TryGetComponent(out EnemyAI enemyAI))
            {
                return enemyAI.enemyType.enemyName == "Spring";
            }
        }
        catch { }

        return false;
    }
}
