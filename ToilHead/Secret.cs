using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

internal class Secret
{
    public static GameObject turretPropPrefab;

    public static void SpawnAsteroid13Secrets()
    {
        bool correctLevelName = StartOfRound.Instance.currentLevel.name == "Asteroid13Level";
        bool correctPlanetName = StartOfRound.Instance.currentLevel.PlanetName == "57 Asteroid-13";
        if (!correctLevelName && !correctPlanetName) return;

        // CoilHead model near power
        GameObject coilHeadModel = GameObject.Find("/asteroid(Clone)/coilheadstuck_model");

        // Big CoilHead in crater
        GameObject coilHeadModel2 = GameObject.Find("/asteroid(Clone)/coilheadrigged");

        // CoilHead followers of the Big CoilHead (Row #1)
        GameObject coilHeadModel3 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (5)");
        GameObject coilHeadModel4 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (7)");
        GameObject coilHeadModel5 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (8)");
        GameObject coilHeadModel6 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (9)");
        GameObject coilHeadModel7 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (10)");
        GameObject coilHeadModel8 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (6)");
        // CoilHead followers of the Big CoilHead (Row #2)
        GameObject coilHeadModel12 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (29)");
        GameObject coilHeadModel13 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (24)");
        GameObject coilHeadModel14 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (25)");
        GameObject coilHeadModel15 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (26)");
        GameObject coilHeadModel16 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (27)");
        GameObject coilHeadModel17 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (28)");
        // CoilHead followers of the Big CoilHead (Row #3)
        GameObject coilHeadModel18 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (23)");
        GameObject coilHeadModel19 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (18)");
        GameObject coilHeadModel20 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (19)");
        GameObject coilHeadModel21 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (20)");
        GameObject coilHeadModel22 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (21)");
        GameObject coilHeadModel23 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (22)");
        // CoilHead followers of the Big CoilHead (Row #4)
        GameObject coilHeadModel24 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (17)");
        GameObject coilHeadModel25 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (12)");
        GameObject coilHeadModel26 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (13)");
        GameObject coilHeadModel27 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (14)");
        GameObject coilHeadModel28 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (15)");
        GameObject coilHeadModel29 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (16)");

        // CoilHead near fire exit
        GameObject coilHeadModel9 = GameObject.Find("/asteroid(Clone)/coilheadstuck_2_model (7)");

        // CoilHead near fire exit 2
        GameObject coilHeadModel10 = GameObject.Find("/asteroid(Clone)/coilheadstuck_2_model (1)");

        // CoilHead near border
        GameObject coilHeadModel11 = GameObject.Find("/asteroid(Clone)/coilheadstuck_model (11)");

        // Turret scale
        Vector3 scale = turretPropPrefab.transform.localScale * 1.3f;

        // CoilHead model near power
        SpawnTurretProp(coilHeadModel, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(19f, 180f, 0f), scale);

        // Big CoilHead in crater
        SpawnTurretProp(coilHeadModel2, new Vector3(-0.15f, 4.8f, -0.2f), new Vector3(0f, 180f, 0f), scale * 2f);

        // CoilHead followers of the Big CoilHead (Row #1)
        SpawnTurretProp(coilHeadModel3, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel4, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel5, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel6, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel7, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel8, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        // CoilHead followers of the Big CoilHead (Row #2)
        SpawnTurretProp(coilHeadModel12, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel13, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel14, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel15, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel16, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel17, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        // CoilHead followers of the Big CoilHead (Row #3)
        SpawnTurretProp(coilHeadModel18, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel19, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel20, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel21, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel22, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel23, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        // CoilHead followers of the Big CoilHead (Row #4)
        SpawnTurretProp(coilHeadModel24, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel25, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel26, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel27, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel28, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp(coilHeadModel29, new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);

        // CoilHead near fire exit
        SpawnTurretProp(coilHeadModel9, new Vector3(-0.05f, 2.4f, -0.05f), new Vector3(3f, 180f, 0f), scale);

        // CoilHead near fire exit 2
        SpawnTurretProp(coilHeadModel10, new Vector3(-0.05f, 2.45f, -0.05f), new Vector3(0f, 180f, 0f), scale);

        // CoilHead near border
        SpawnTurretProp(coilHeadModel11, new Vector3(-0.05f, 2.5f, -0.1f), new Vector3(25f, 180f, 0f), scale);
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
