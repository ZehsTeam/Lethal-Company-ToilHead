using UnityEngine;

namespace com.github.zehsteam.ToilHead;

internal static class Secret
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
        if (Content.TurretPropPrefab == null)
        {
            Plugin.Instance.LogWarningExtended("Warning: Failed to spawn Asteroid13 secrets. turretPropPrefab is null.");
            return;
        }

        // Turret
        Vector3 scale = Content.TurretPropPrefab.transform.localScale * 1.3f;

        // Turret -> coilheadstuck_model
        Vector3 position = new Vector3(0f, 3.031f, -0.464f);
        Vector3 rotation = new Vector3(-27.2f, 0f, 0f);

        // Turret -> coilheadstuck_2_model
        Vector3 position2 = new Vector3(0f, 3.105f, -0.11f);
        Vector3 rotation2 = Vector3.zero;

        // Turret -> coilheadrigged
        Vector3 position3 = new Vector3(0f, 6.217f, -0.225f);
        Vector3 rotation3 = Vector3.zero;

        // CoilHead model near power
        SpawnTurretProp("coilheadstuck_model", position, rotation, scale);

        // Big CoilHead in crater
        SpawnTurretProp("coilheadrigged", position3, rotation3, scale * 2f);

        // CoilHead followers of the Big CoilHead (Row #1)
        SpawnTurretProp("coilheadstuck_model (5)", position, rotation, scale);
        SpawnTurretProp("coilheadstuck_model (7)", position, rotation, scale);
        SpawnTurretProp("coilheadstuck_model (8)", position, rotation, scale);
        SpawnTurretProp("coilheadstuck_model (9)", position, rotation, scale);
        SpawnTurretProp("coilheadstuck_model (10)", position, rotation, scale);
        SpawnTurretProp("coilheadstuck_model (6)", position, rotation, scale);
        // CoilHead followers of the Big CoilHead (Row #2)
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (29)", position, rotation, scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (24)", position, rotation, scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (25)", position, rotation, scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (26)", position, rotation, scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (27)", position, rotation, scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (28)", position, rotation, scale);
        // CoilHead followers of the Big CoilHead (Row #3)
        SpawnTurretProp("coilheadstuck_model (23)", position, rotation, scale);
        SpawnTurretProp("coilheadstuck_model (18)", position, rotation, scale);
        SpawnTurretProp("coilheadstuck_model (19)", position, rotation, scale);
        SpawnTurretProp("coilheadstuck_model (20)", position, rotation, scale);
        SpawnTurretProp("coilheadstuck_model (21)", position, rotation, scale);
        SpawnTurretProp("coilheadstuck_model (22)", position, rotation, scale);
        // CoilHead followers of the Big CoilHead (Row #4)
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (17)", position, rotation, scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (12)", position, rotation, scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (13)", position, rotation, scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (14)", position, rotation, scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (15)", position, rotation, scale);
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_model (16)", position, rotation, scale);

        // CoilHead near fire exit
        SpawnTurretProp("asteroid(Clone)/coilheadstuck_2_model (7)", position2, rotation2, scale);

        // CoilHead near fire exit 2
        SpawnTurretProp("coilheadstuck_2_model (1)", position2, rotation2, scale);

        // CoilHead near border
        SpawnTurretProp("coilheadstuck_model (11)", position, rotation, scale);

        // Giant CoilHead in crater
        SpawnTurretProp("coilheadrigged (1)", position3, rotation3, scale * 2f);

        Plugin.Instance.LogInfoExtended("Spawned Asteroid13 secrets.");
    }

    private static void SpawnTurretProp(string parentName, Vector3 positionOffset, Vector3 rotationOffset)
    {
        SpawnTurretProp(parentName, positionOffset, rotationOffset, Content.TurretPropPrefab.transform.localScale);
    }

    private static void SpawnTurretProp(string parentName, Vector3 positionOffset, Vector3 rotationOffset, Vector3 scale)
    {
        if (Content.TurretPropPrefab == null)
        {
            Plugin.Instance.LogWarningExtended("Warning: Failed to Instantiate turretPropPrefab. turretPropPrefab is null.");
            return;
        }

        GameObject parentObject = GameObject.Find(parentName);

        if (parentObject == null)
        {
            Plugin.Instance.LogWarningExtended($"Warning: Failed to Instantiate turretPropPrefab. Parent GameObject is null.");
            return;
        }

        Transform turretTransform = Object.Instantiate(Content.TurretPropPrefab, Vector3.zero, Quaternion.identity, parentObject.transform).transform;

        turretTransform.localScale = scale;

        turretTransform.localRotation = Quaternion.identity;
        turretTransform.Rotate(rotationOffset, Space.Self);

        turretTransform.localPosition = positionOffset;

        Utils.DisableColliders(turretTransform.gameObject);
    }
}
