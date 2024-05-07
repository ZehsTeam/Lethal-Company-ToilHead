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
        SpawnTurretProp("coilheadstuck_model", new Vector3(0f,3.02f, -0.49f), new Vector3(-38f, 0f, 0f), scale);

        // Big CoilHead in crater
        SpawnTurretProp("coilheadrigged", new Vector3(0f, 6.2f, -0.25f), new Vector3(0f, 0f, 0f), scale * 2f);

        // CoilHead followers of the Big CoilHead (Row #1)
        SpawnTurretProp("coilheadstuck_model (5)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("coilheadstuck_model (7)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("coilheadstuck_model (8)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("coilheadstuck_model (9)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("coilheadstuck_model (10)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("coilheadstuck_model (6)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        // CoilHead followers of the Big CoilHead (Row #2)
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (29)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (24)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (25)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (26)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (27)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (28)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        // CoilHead followers of the Big CoilHead (Row #3)
        SpawnTurretProp("coilheadstuck_model (23)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("coilheadstuck_model (18)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("coilheadstuck_model (19)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("coilheadstuck_model (20)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("coilheadstuck_model (21)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("coilheadstuck_model (22)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        // CoilHead followers of the Big CoilHead (Row #4)
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (17)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (12)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (13)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (14)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (15)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (16)", new Vector3(0f, 3.03f, -0.48f), new Vector3(-32f, 0f, 0f), scale);

        // CoilHead near fire exit
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_2_model (7)", new Vector3(0f, 3.1f, -0.12f), new Vector3(0f, 0f, 0f), scale);

        // CoilHead near fire exit 2
        SpawnTurretProp("coilheadstuck_2_model (1)", new Vector3(0f, 3.1f, -0.12f), new Vector3(0f, 0f, 0f), scale);

        // CoilHead near border
        SpawnTurretProp("coilheadstuck_model (11)", new Vector3(0f, 3.02f, -0.475f), new Vector3(-30f, 0f, 0f), scale);
    }

    private static void SpawnTurretProp(string parentName, Vector3 positionOffset, Vector3 rotationOffset)
    {
        SpawnTurretProp(parentName, positionOffset, rotationOffset, Content.turretPropPrefab.transform.localScale);
    }

    private static void SpawnTurretProp(string parentName, Vector3 positionOffset, Vector3 rotationOffset, Vector3 scale)
    {
        if (Content.turretPropPrefab == null)
        {
            Plugin.logger.LogWarning("Warning: Failed to Instantiate turretPropPrefab. turretPropPrefab is null.");
            return;
        }

        GameObject parentObject = GameObject.Find(parentName);

        if (parentObject == null)
        {
            Plugin.logger.LogWarning($"Warning: Failed to Instantiate turretPropPrefab. Parent GameObject is null.");
            return;
        }

        GameObject turretObject = Object.Instantiate(Content.turretPropPrefab, parentObject.transform.position, Quaternion.identity);
        turretObject.transform.SetParent(parentObject.transform);
        turretObject.transform.localPosition = positionOffset;
        turretObject.transform.localRotation = Quaternion.identity;
        turretObject.transform.Rotate(rotationOffset, Space.Self);
        turretObject.transform.localScale = scale;

        Utils.DisableColliders(turretObject);
    }
}
