using BepInEx.Configuration;
using com.github.zehsteam.ToilHead.MonoBehaviours;
using System.Collections.Generic;
using System.Reflection;

namespace com.github.zehsteam.ToilHead;

public class SyncedConfigManager
{
    private SyncedConfigData hostConfigData;

    #region ConfigEntries
    // Toil-Head Settings
    private ConfigEntry<int> SpawnChanceCfg;
    private ConfigEntry<int> MaxSpawnCountCfg;

    // Turret Settings
    private ConfigEntry<float> TurretLostLOSDurationCfg;
    private ConfigEntry<float> TurretRotationRangeCfg;
    private ConfigEntry<float> TurretCodeAccessCooldownDurationCfg;

    // Turret Detection Settings
    private ConfigEntry<bool> TurretDetectionRotationCfg;
    private ConfigEntry<float> TurretDetectionRotationSpeedCfg;

    // Turret Charging Settings
    private ConfigEntry<float> TurretChargingDurationCfg;
    private ConfigEntry<float> TurretChargingRotationSpeedCfg;

    // Turret Firing Settings
    private ConfigEntry<float> TurretFiringRotationSpeedCfg;

    // Turret Berserk Settings
    private ConfigEntry<float> TurretBerserkDurationCfg;
    private ConfigEntry<float> TurretBerserkRotationSpeedCfg;
    #endregion

    #region Config Setting Get/Set Properties
    // Toil-Head Settings
    internal int SpawnChance { get { return SpawnChanceCfg.Value; } set { SpawnChanceCfg.Value = value; } }
    internal int MaxSpawnCount { get { return MaxSpawnCountCfg.Value; } set { MaxSpawnCountCfg.Value = value; } }

    // Turret Settings
    internal float TurretLostLOSDuration
    {
        get
        {
            return hostConfigData == null ? TurretLostLOSDurationCfg.Value : hostConfigData.turretLostLOSDuration;
        }
        set
        {
            TurretLostLOSDurationCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal float TurretRotationRange
    {
        get
        {
            return hostConfigData == null ? TurretRotationRangeCfg.Value : hostConfigData.turretRotationRange;
        }
        set
        {
            TurretRotationRangeCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal float TurretCodeAccessCooldownDuration
    {
        get
        {
            return hostConfigData == null ? TurretCodeAccessCooldownDurationCfg.Value : hostConfigData.turretCodeAccessCooldownDuration;
        }
        set
        {
            TurretCodeAccessCooldownDurationCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    // Turret Detection Settings
    internal bool TurretDetectionRotation
    {
        get
        {
            return hostConfigData == null ? TurretDetectionRotationCfg.Value : hostConfigData.turretDetectionRotation;
        }
        set
        {
            TurretDetectionRotationCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal float TurretDetectionRotationSpeed
    {
        get
        {
            return hostConfigData == null ? TurretDetectionRotationSpeedCfg.Value : hostConfigData.turretDetectionRotationSpeed;
        }
        set
        {
            TurretDetectionRotationSpeedCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    // Turret Charging Settings
    internal float TurretChargingDuration
    {
        get
        {
            return hostConfigData == null ? TurretChargingDurationCfg.Value : hostConfigData.turretChargingDuration;
        }
        set
        {
            TurretChargingDurationCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal float TurretChargingRotationSpeed
    {
        get
        {
            return hostConfigData == null ? TurretChargingRotationSpeedCfg.Value : hostConfigData.turretChargingRotationSpeed;
        }
        set
        {
            TurretChargingRotationSpeedCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    // Turret Firing Settings
    internal float TurretFiringRotationSpeed
    {
        get
        {
            return hostConfigData == null ? TurretFiringRotationSpeedCfg.Value : hostConfigData.turretFiringRotationSpeed;
        }
        set
        {
            TurretFiringRotationSpeedCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    // Turret Berserk Settings
    internal float TurretBerserkDuration
    {
        get
        {
            return hostConfigData == null ? TurretBerserkDurationCfg.Value : hostConfigData.turretBerserkDuration;
        }
        set
        {
            TurretBerserkDurationCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal float TurretBerserkRotationSpeed
    {
        get
        {
            return hostConfigData == null ? TurretBerserkRotationSpeedCfg.Value : hostConfigData.turretBerserkRotationSpeed;
        }
        set
        {
            TurretBerserkRotationSpeedCfg.Value = value;
            SyncedConfigsChanged();
        }
    }
    #endregion

    public SyncedConfigManager()
    {
        BindConfigs();
        ClearUnusedEntries();
    }

    private void BindConfigs()
    {
        ConfigFile config = Plugin.Instance.Config;

        // Toil-Head Settings
        SpawnChanceCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "spawnChance"),
            30,
            new ConfigDescription("The percent chance for a Coil-Head to turn into a Toil-Head.",
            new AcceptableValueRange<int>(0, 100))
        );
        MaxSpawnCountCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "maxSpawnCount"),
            1,
            new ConfigDescription("The max amount of Toil-Heads that can spawn.")
        );

        // Turret Settings
        TurretLostLOSDurationCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "turretLostLOSDuration"),
            0.75f,
            new ConfigDescription("The duration until the turret loses the target player when not in line of sight.\nVanilla Turret Default value: 2")
        );
        TurretRotationRangeCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "turretRotationRange"),
            75f,
            new ConfigDescription("The rotation range of the turret in degrees.",
            new AcceptableValueRange<float>(0f, 360f))
        );
        TurretCodeAccessCooldownDurationCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "turretCodeAccessCooldownDuration"),
            10f,
            new ConfigDescription("The duration of the turret being disabled from the terminal in seconds.\nVanilla Turret Default value: 7")
        );

        // Turret Detection Settings
        TurretDetectionRotationCfg = config.Bind(
            new ConfigDefinition("Turret Detection Settings", "turretDetectionRotation"),
            false,
            new ConfigDescription("If enabled, the turret will rotate when searching for players.\nVanilla Turret Default value: true")
        );
        TurretDetectionRotationSpeedCfg = config.Bind(
            new ConfigDefinition("Turret Detection Settings", "turretDetectionRotationSpeed"),
            28f,
            new ConfigDescription("The rotation speed of the turret when in detection state.")
        );

        // Turret Charging Settings
        TurretChargingDurationCfg = config.Bind(
            new ConfigDefinition("Turret Charging Settings", "turretChargingDuration"),
            2f,
            new ConfigDescription("The duration of the turret charging state.\nVanilla Turret Default value: 1.5")
        );
        TurretChargingRotationSpeedCfg = config.Bind(
            new ConfigDefinition("Turret Charging Settings", "turretChargingRotationSpeed"),
            95f,
            new ConfigDescription("The rotation speed of the turret when in charging state.")
        );

        // Turret Firing Settings
        TurretFiringRotationSpeedCfg = config.Bind(
            new ConfigDefinition("Turret Firing Settings", "turretFiringRotationSpeed"),
            95f,
            new ConfigDescription("The rotation speed of the turret when in firing state.")
        );

        // Turret Berserk Settings
        TurretBerserkDurationCfg = config.Bind(
            new ConfigDefinition("Turret Berserk Settings", "turretBerserkDuration"),
            9f,
            new ConfigDescription("The duration of the turret berserk state.")
        );
        TurretBerserkRotationSpeedCfg = config.Bind(
            new ConfigDefinition("Turret Berserk Settings", "turretBerserkRotationSpeed"),
            77f,
            new ConfigDescription("The rotation speed of the turret when in berserk state.")
        );
    }

    private void ClearUnusedEntries()
    {
        ConfigFile configFile = Plugin.Instance.Config;

        // Normally, old unused config entries don't get removed, so we do it with this piece of code. Credit to Kittenji.
        PropertyInfo orphanedEntriesProp = configFile.GetType().GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(configFile, null);
        orphanedEntries.Clear(); // Clear orphaned entries (Unbinded/Abandoned entries)
        configFile.Save(); // Save the config file to save these changes
    }

    internal void SetHostConfigData(SyncedConfigData syncedConfigData)
    {
        hostConfigData = syncedConfigData;
    }

    private void SyncedConfigsChanged()
    {
        if (!Plugin.IsHostOrServer) return;

        PluginNetworkBehaviour.Instance.SendConfigToPlayerClientRpc(new SyncedConfigData(this));
    }
}
