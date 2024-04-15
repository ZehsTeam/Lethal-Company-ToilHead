using BepInEx.Configuration;
using com.github.zehsteam.ToilHead.MonoBehaviours;
using System.Collections.Generic;
using System.Reflection;

namespace com.github.zehsteam.ToilHead;

public class SyncedConfigManager
{
    private SyncedConfigData hostConfigData;

    #region ConfigEntries
    // Coil-Head Settings
    private ConfigEntry<int> SpawnChanceCfg;
    private ConfigEntry<int> MaxSpawnCountCfg;

    // Turret Settings
    private ConfigEntry<bool> TurretRotationWhenSearchingCfg;
    private ConfigEntry<float> TurretDetectionRotationSpeedCfg;
    private ConfigEntry<float> TurretChargingRotationSpeedCfg;
    private ConfigEntry<float> TurretRotationRangeCfg;
    private ConfigEntry<float> TurretCodeAccessCooldownDurationCfg;
    #endregion

    #region Config Setting Get/Set Properties
    // Coil-Head Settings
    internal int SpawnChance { get { return SpawnChanceCfg.Value; } set { SpawnChanceCfg.Value = value; } }
    internal int MaxSpawnCount { get { return MaxSpawnCountCfg.Value; } set { MaxSpawnCountCfg.Value = value; } }

    // Turret Settings
    internal bool TurretRotationWhenSearching
    {
        get
        {
            return hostConfigData == null ? TurretRotationWhenSearchingCfg.Value : hostConfigData.turretRotationWhenSearching;
        }
        set
        {
            TurretRotationWhenSearchingCfg.Value = value;
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
    #endregion

    public SyncedConfigManager()
    {
        BindConfigs();
        ClearUnusedEntries();
    }

    private void BindConfigs()
    {
        ConfigFile config = Plugin.Instance.Config;

        // CoilHead Settings
        SpawnChanceCfg = config.Bind(
            new ConfigDefinition("General Settings", "spawnChance"),
            30,
            new ConfigDescription("The percent chance for a Coil-Head to turn into a Toil-Head.",
            new AcceptableValueRange<int>(0, 100))
        );
        MaxSpawnCountCfg = config.Bind(
            new ConfigDefinition("General Settings", "maxSpawnCount"),
            1,
            new ConfigDescription("The max amount of Toil-Heads that can spawn.")
        );

        // Turret Settings
        TurretRotationWhenSearchingCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "turretRotationWhenSearching"),
            false,
            new ConfigDescription("If enabled, the turret will rotate when searching for players.")
        );
        TurretDetectionRotationSpeedCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "turretDetectionRotationSpeed"),
            28f,
            new ConfigDescription("The rotation speed of the turret when searching for players.")
        );
        TurretChargingRotationSpeedCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "turretChargingRotationSpeed"),
            95f,
            new ConfigDescription("The rotation speed of the turret when charging at the target player.")
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
            new ConfigDescription("The duration of the turret being disabled from the terminal in seconds.")
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
