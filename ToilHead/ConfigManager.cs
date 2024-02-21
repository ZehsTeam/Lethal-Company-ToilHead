using BepInEx.Configuration;
using System;

namespace com.github.zehsteam.ToilHead;

internal class ConfigManager
{
    // Settings
    private ConfigEntry<int> SpawnChanceCfg;
    private ConfigEntry<int> MaxSpawnsCfg;
    private ConfigEntry<int> SpawnTurretFacingForwardWeightCfg;
    private ConfigEntry<int> SpawnTurretFacingBackwardWeightCfg;

    // Settings
    internal int SpawnChance
    {
        get
        {
            return SpawnChanceCfg.Value;
        }
        set
        {
            SpawnChanceCfg.Value = Math.Clamp(value, 0, 100);
        }
    }

    internal int MaxSpawns
    {
        get
        {
            return MaxSpawnsCfg.Value;
        }
        set
        {
            MaxSpawnsCfg.Value = value;
        }
    }

    internal int SpawnTurretFacingForwardWeight
    {
        get
        {
            return SpawnTurretFacingForwardWeightCfg.Value;
        }
        set
        {
            SpawnTurretFacingForwardWeightCfg.Value = value;
        }
    }

    internal int SpawnTurretFacingBackwardWeight
    {
        get
        {
            return SpawnTurretFacingBackwardWeightCfg.Value;
        }
        set
        {
            SpawnTurretFacingBackwardWeightCfg.Value = value;
        }
    }

    public ConfigManager()
    {
        BindConfigs();
    }

    private void BindConfigs()
    {
        ConfigFile config = ToilHeadBase.Instance.Config;

        // Settings
        SpawnChanceCfg = config.Bind(
            new ConfigDefinition("Settings", "spawnChance"),
            60,
            new ConfigDescription("The percent chance for a Coil-Head to turn into a Toil-Head.",
            new AcceptableValueRange<int>(0, 100))
        );

        MaxSpawnsCfg = config.Bind(
            new ConfigDefinition("Settings", "maxSpawns"),
            3,
            new ConfigDescription("The max amount of Toil-Heads that can spawn.")
        );

        SpawnTurretFacingForwardWeightCfg = config.Bind(
            new ConfigDefinition("Settings", "spawnTurretFacingForwardWeight"),
            1,
            new ConfigDescription("The weight chance for the turret to spawn facing forward.",
            new AcceptableValueRange<int>(0, 100))
        );

        SpawnTurretFacingBackwardWeightCfg = config.Bind(
            new ConfigDefinition("Settings", "spawnTurretFacingBackwardWeight"),
            3,
            new ConfigDescription("The weight chance for the turret to spawn facing backward.",
            new AcceptableValueRange<int>(0, 100))
        );
    }
}
