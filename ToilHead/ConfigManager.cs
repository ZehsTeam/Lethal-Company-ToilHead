using BepInEx.Configuration;

namespace com.github.zehsteam.ToilHead;

internal class ConfigManager
{
    // Coil-Head Settings
    private ConfigEntry<int> SpawnChanceCfg;
    private ConfigEntry<int> MaxSpawnCountCfg;

    // Turret Settings
    private ConfigEntry<bool> HideTurretBodyCfg;
    private ConfigEntry<int> SpawnTurretFacingForwardWeightCfg;
    private ConfigEntry<int> SpawnTurretFacingBackwardWeightCfg;

    // Coil-Head Settings
    internal int SpawnChance { get { return SpawnChanceCfg.Value; } set { SpawnChanceCfg.Value = value; } }
    internal int MaxSpawnCount { get { return MaxSpawnCountCfg.Value; } set { MaxSpawnCountCfg.Value = value; } }

    // Turret Settings
    internal bool HideTurretBody { get { return HideTurretBodyCfg.Value; } set { HideTurretBodyCfg.Value = value; } }
    internal int SpawnTurretFacingForwardWeight { get { return SpawnTurretFacingForwardWeightCfg.Value; } set { SpawnTurretFacingForwardWeightCfg.Value = value; } }
    internal int SpawnTurretFacingBackwardWeight { get { return SpawnTurretFacingBackwardWeightCfg.Value; } set { SpawnTurretFacingBackwardWeightCfg.Value = value; } }

    public ConfigManager()
    {
        BindConfigs();
    }

    private void BindConfigs()
    {
        ConfigFile config = Plugin.Instance.Config;

        // CoilHead Settings
        SpawnChanceCfg = config.Bind(
            new ConfigDefinition("Coil-Head Settings", "spawnChance"),
            30,
            new ConfigDescription("The percent chance for a Coil-Head to turn into a Toil-Head.",
            new AcceptableValueRange<int>(0, 100))
        );
        MaxSpawnCountCfg = config.Bind(
            new ConfigDefinition("Coil-Head Settings", "maxSpawnCount"),
            1,
            new ConfigDescription("The max amount of Toil-Heads that can spawn.")
        );

        // Turret Settings
        HideTurretBodyCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "hideTurretBody"),
            true,
            new ConfigDescription("If enabled, the turret body will be removed.")
        );
        SpawnTurretFacingForwardWeightCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "spawnTurretFacingForwardWeight"),
            1,
            new ConfigDescription("The weight chance for the turret to spawn facing forwards. (Only applies when hideTurretBody is set to false)",
            new AcceptableValueRange<int>(0, 100))
        );
        SpawnTurretFacingBackwardWeightCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "spawnTurretFacingBackwardWeight"),
            3,
            new ConfigDescription("The weight chance for the turret to spawn facing backwards. (Only applies when hideTurretBody is set to false)",
            new AcceptableValueRange<int>(0, 100))
        );
    }
}
