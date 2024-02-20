using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

internal class Secret
{
    public static GameObject turretPropPrefab;

    public static void SpawnAsteroid13Secret()
    {
        if (StartOfRound.Instance.currentLevel.name != "57 Asteroid-13") return;

        // CoilHead model near power
        GameObject coilHeadModel = GameObject.Find("/asteroid(Clone)/coilheadstuck_model");

        // Big CoilHead in crater
        GameObject coilHeadModel2 = GameObject.Find("/asteroid(Clone)/coilheadrigged");

        // CoilHead followers of the Big CoilHead
        GameObject coilHeadModel3 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (5)");
        GameObject coilHeadModel4 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (7)");
        GameObject coilHeadModel5 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (8)");
        GameObject coilHeadModel6 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (9)");
        GameObject coilHeadModel7 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (10)");
        GameObject coilHeadModel8 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (6)");

        // CoilHead near fire exit
        GameObject coilHeadModel9 = GameObject.Find("/asteroid(Clone)/coilheadstuck_2_model (7)");

        // CoilHead near fire exit 2
        GameObject coilHeadModel10 = GameObject.Find("/asteroid(Clone)/coilheadstuck_2_model (1)");

        // CoilHead near border
        GameObject coilHeadModel11 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (11)");

        // CoilHead model near power
        SpawnTurretProp(coilHeadModel, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(19f, 180f, 0f));

        // Big CoilHead in crater
        SpawnTurretProp(coilHeadModel2, new Vector3(-0.15f, 4.8f, -0.2f), new Vector3(0f, 180f, 0f), turretPropPrefab.transform.localScale * 2f);

        // CoilHead followers of the Big CoilHead
        SpawnTurretProp(coilHeadModel3, new Vector3(-0.05f, 2.4f, -0.25f), new Vector3(15f, 180f, 0f));
        SpawnTurretProp(coilHeadModel4, new Vector3(-0.05f, 2.4f, -0.25f), new Vector3(15f, 180f, 0f));
        SpawnTurretProp(coilHeadModel5, new Vector3(-0.05f, 2.4f, -0.25f), new Vector3(15f, 180f, 0f));
        SpawnTurretProp(coilHeadModel6, new Vector3(-0.05f, 2.4f, -0.25f), new Vector3(15f, 180f, 0f));
        SpawnTurretProp(coilHeadModel7, new Vector3(-0.05f, 2.4f, -0.25f), new Vector3(15f, 180f, 0f));
        SpawnTurretProp(coilHeadModel8, new Vector3(-0.05f, 2.4f, -0.25f), new Vector3(15f, 180f, 0f));

        // CoilHead near fire exit
        SpawnTurretProp(coilHeadModel9, new Vector3(-0.05f, 2.4f, -0.05f), new Vector3(3f, 180f, 0f));

        // CoilHead near fire exit 2
        SpawnTurretProp(coilHeadModel10, new Vector3(-0.05f, 2.45f, -0.05f), new Vector3(0f, 180f, 0f));

        // CoilHead near border
        SpawnTurretProp(coilHeadModel11, new Vector3(-0.05f, 2.5f, -0.1f), new Vector3(25f, 180f, 0f));
    }

    private static void SpawnTurretProp(GameObject parent, Vector3 positionOffset, Vector3 rotationOffset)
    {
        SpawnTurretProp(parent, positionOffset, rotationOffset, turretPropPrefab.transform.localScale);
    }

    private static void SpawnTurretProp(GameObject parent, Vector3 positionOffset, Vector3 rotationOffset, Vector3 scale)
    {
        if (parent == null) return;

        GameObject turret = Object.Instantiate(turretPropPrefab, parent.transform.position, Quaternion.identity);

        turret.name = "TurretProp";

        turret.transform.SetParent(parent.transform);
        turret.transform.localPosition = positionOffset;
        turret.transform.localRotation = Quaternion.identity;
        turret.transform.localScale = scale;
        turret.transform.Rotate(rotationOffset, Space.Self);

        List<Collider> colliders = turret.GetComponentsInChildren<Collider>().ToList();
        colliders.ForEach(collider => collider.enabled = false);
    }
}
