using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ToilSlayerControllerBehaviour : ToilHeadControllerBehaviour
{
    [Header("Toil-Slayer Properties")]
    [Space(5f)]
    public Material SpringManMaterial = null;

    protected override void Start()
    {
        base.Start();

        SetMaterials();
    }

    private void SetMaterials()
    {
        Transform springManModelTransfrom = transform.parent.Find("SpringManModel");

        Renderer headRenderer = springManModelTransfrom.Find("Head").GetComponent<Renderer>();
        Renderer bodyRenderer = springManModelTransfrom.Find("Body").GetComponent<Renderer>();

        headRenderer.materials = [SpringManMaterial];

        Material[] bodyMaterials = bodyRenderer.materials;
        bodyMaterials[1] = SpringManMaterial;
        bodyRenderer.materials = bodyMaterials;
    }
}
