using UnityEngine;

namespace com.github.zehsteam.ToilHead;

internal class Secret
{
    public static void SpawnSecrets()
    {
        string planetName = StartOfRound.Instance.currentLevel.PlanetName;

        if (planetName == "57 Asteroid-13")
        {
            SpawnAsteroid13Secrets();
        }
    }

    public static void SpawnAsteroid13Secrets()
    {
        // Turret scale
        Vector3 scale = Content.turretPropPrefab.transform.localScale * 1.3f;

        // CoilHead model near power
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(19f, 180f, 0f), scale);

        // Big CoilHead in crater
        SpawnTurretProp("/asteroid(Clone)/coilheadrigged", new Vector3(-0.15f, 4.8f, -0.2f), new Vector3(0f, 180f, 0f), scale * 2f);

        // CoilHead followers of the Big CoilHead (Row #1)
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (5)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (7)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (8)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (9)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (10)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (6)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        // CoilHead followers of the Big CoilHead (Row #2)
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (29)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (24)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (25)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (26)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (27)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (28)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        // CoilHead followers of the Big CoilHead (Row #3)
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (23)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (18)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (19)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (20)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (21)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (22)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        // CoilHead followers of the Big CoilHead (Row #4)
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (17)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (12)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (13)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (14)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (15)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (16)", new Vector3(-0.05f, 2.4f, -0.15f), new Vector3(15f, 180f, 0f), scale);

        // CoilHead near fire exit
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_2_model (7)", new Vector3(-0.05f, 2.4f, -0.05f), new Vector3(3f, 180f, 0f), scale);

        // CoilHead near fire exit 2
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_2_model (1)", new Vector3(-0.05f, 2.45f, -0.05f), new Vector3(0f, 180f, 0f), scale);

        // CoilHead near border
        SpawnTurretProp("/asteroid(Clone)/coilheadstuck_model (11)", new Vector3(-0.05f, 2.5f, -0.1f), new Vector3(25f, 180f, 0f), scale);
    }

    private static void SpawnTurretProp(string parentName, Vector3 positionOffset, Vector3 rotationOffset)
    {
        SpawnTurretProp(parentName, positionOffset, rotationOffset, Content.turretPropPrefab.transform.localScale);
    }

    private static void SpawnTurretProp(string parentName, Vector3 positionOffset, Vector3 rotationOffset, Vector3 scale)
    {
        if (Content.turretPropPrefab == null)
        {
            Plugin.logger.LogWarning("Warning: failed to Instantiate turretPropPrefab. turretPropPrefab could not be found.");
            return;
        }

        GameObject parentObject = GameObject.Find(parentName);

        if (parentObject == null)
        {
            Plugin.logger.LogWarning($"Warning: failed to Instantiate turretPropPrefab. Could not find parent GameObject \"{parentName}\".");
            return;
        }

        GameObject turretObject = Object.Instantiate(Content.turretPropPrefab, parentObject.transform.position, Quaternion.identity);
        turretObject.name = "TurretProp";
        turretObject.transform.SetParent(parentObject.transform);
        turretObject.transform.localPosition = positionOffset;
        turretObject.transform.localRotation = Quaternion.identity;
        turretObject.transform.localScale = scale;
        turretObject.transform.Rotate(rotationOffset, Space.Self);

        Utils.DisableColliders(turretObject);
    }
}
