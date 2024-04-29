using BepInEx.Configuration;
using com.github.zehsteam.ToilHead.MonoBehaviours;
using System.Collections.Generic;
using System.Reflection;

namespace com.github.zehsteam.ToilHead;

public class SyncedConfigManager
{
    private SyncedConfigData hostConfigData;

    // General Settings
    private ConfigEntry<bool> ExtendedLoggingCfg;

    #region ConfigEntries
    // Toil-Head Settings
    private ConfigEntry<string> OtherSpawnSettingsCfg;
    private ConfigEntry<string> LiquidationSpawnSettingsCfg;
    private ConfigEntry<string> EmbrionSpawnSettingsCfg;
    private ConfigEntry<string> ArtificeSpawnSettingsCfg;
    private ConfigEntry<string> TitanSpawnSettingsCfg;
    private ConfigEntry<string> DineSpawnSettingsCfg;
    private ConfigEntry<string> RendSpawnSettingsCfg;
    private ConfigEntry<string> AdamanceSpawnSettingsCfg;
    private ConfigEntry<string> OffenseSpawnSettingsCfg;
    private ConfigEntry<string> MarchSpawnSettingsCfg;
    private ConfigEntry<string> VowSpawnSettingsCfg;

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
    // General Settings
    internal bool ExtendedLogging { get { return ExtendedLoggingCfg.Value; } set { ExtendedLoggingCfg.Value = value; } }

    // Toil-Head Settings
    internal string OtherSpawnSettings { get { return OtherSpawnSettingsCfg.Value; } set { OtherSpawnSettingsCfg.Value = value; } }
    internal string LiquidationSpawnSettings { get { return LiquidationSpawnSettingsCfg.Value; } set { LiquidationSpawnSettingsCfg.Value = value; } }
    internal string EmbrionSpawnSettings { get { return EmbrionSpawnSettingsCfg.Value; } set { EmbrionSpawnSettingsCfg.Value = value; } }
    internal string ArtificeSpawnSettings { get { return ArtificeSpawnSettingsCfg.Value; } set { ArtificeSpawnSettingsCfg.Value = value; } }
    internal string TitanSpawnSettings { get { return TitanSpawnSettingsCfg.Value; } set { TitanSpawnSettingsCfg.Value = value; } }
    internal string DineSpawnSettings { get { return DineSpawnSettingsCfg.Value; } set { DineSpawnSettingsCfg.Value = value; } }
    internal string RendSpawnSettings { get { return RendSpawnSettingsCfg.Value; } set { RendSpawnSettingsCfg.Value = value; } }
    internal string AdamanceSpawnSettings { get { return AdamanceSpawnSettingsCfg.Value; } set { AdamanceSpawnSettingsCfg.Value = value; } }
    internal string OffenseSpawnSettings { get { return OffenseSpawnSettingsCfg.Value; } set { OffenseSpawnSettingsCfg.Value = value; } }
    internal string MarchSpawnSettings { get { return MarchSpawnSettingsCfg.Value; } set { MarchSpawnSettingsCfg.Value = value; } }
    internal string VowSpawnSettings { get { return VowSpawnSettingsCfg.Value; } set { VowSpawnSettingsCfg.Value = value; } }

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

        ExtendedLoggingCfg = config.Bind(
            new ConfigDefinition("General Settings", "ExtendedLogging"),
            false,
            new ConfigDescription("Enable extended logging.")
        );

        #region Toil-Head Settings
        // Toil-Head Settings
        OtherSpawnSettingsCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "OtherSpawnSettings"),
            $"1,30",
            new ConfigDescription(GetSpawnSettingsDescription("Other/Modded moons"))
        );
        LiquidationSpawnSettingsCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "LiquidationSpawnSettings"),
            $"1,20",
            new ConfigDescription(GetSpawnSettingsDescription("44-Liquidation"))
        );
        EmbrionSpawnSettingsCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "EmbrionSpawnSettings"),
            $"1,20",
            new ConfigDescription(GetSpawnSettingsDescription("5-Embrion"))
        );
        ArtificeSpawnSettingsCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "ArtificeSpawnSettings"),
            $"1,70",
            new ConfigDescription(GetSpawnSettingsDescription("68-Artifice"))
        );
        TitanSpawnSettingsCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "TitanSpawnSettings"),
            $"1,50",
            new ConfigDescription(GetSpawnSettingsDescription("8-Titan"))
        );
        DineSpawnSettingsCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "DineSpawnSettings"),
            $"1,30",
            new ConfigDescription(GetSpawnSettingsDescription("7-Dine"))
        );
        RendSpawnSettingsCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "RendSpawnSettings"),
            $"1,40",
            new ConfigDescription(GetSpawnSettingsDescription("85-Rend"))
        );
        AdamanceSpawnSettingsCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "AdamanceSpawnSettings"),
            $"1,25",
            new ConfigDescription(GetSpawnSettingsDescription("20-Adamance"))
        );
        OffenseSpawnSettingsCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "OffenseSpawnSettings"),
            $"1,20",
            new ConfigDescription(GetSpawnSettingsDescription("21-Offense"))
        );
        MarchSpawnSettingsCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "MarchSpawnSettings"),
            $"1,20",
            new ConfigDescription(GetSpawnSettingsDescription("61-March"))
        );
        VowSpawnSettingsCfg = config.Bind(
            new ConfigDefinition("Toil-Head Settings", "VowSpawnSettings"),
            $"1,20",
            new ConfigDescription(GetSpawnSettingsDescription("56-Vow"))
        );
        #endregion

        #region All Turret Settings
        // Turret Settings
        TurretLostLOSDurationCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "TurretLostLOSDuration"),
            0.75f,
            new ConfigDescription("The duration until the turret loses the target player when not in line of sight.\nVanilla Turret Default value: 2")
        );
        TurretRotationRangeCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "TurretRotationRange"),
            75f,
            new ConfigDescription("The rotation range of the turret in degrees.",
            new AcceptableValueRange<float>(0f, 360f))
        );
        TurretCodeAccessCooldownDurationCfg = config.Bind(
            new ConfigDefinition("Turret Settings", "TurretCodeAccessCooldownDuration"),
            10f,
            new ConfigDescription("The duration of the turret being disabled from the terminal in seconds.\nVanilla Turret Default value: 7")
        );

        // Turret Detection Settings
        TurretDetectionRotationCfg = config.Bind(
            new ConfigDefinition("Turret Detection Settings", "TurretDetectionRotation"),
            false,
            new ConfigDescription("If enabled, the turret will rotate when searching for players.\nVanilla Turret Default value: true")
        );
        TurretDetectionRotationSpeedCfg = config.Bind(
            new ConfigDefinition("Turret Detection Settings", "TurretDetectionRotationSpeed"),
            28f,
            new ConfigDescription("The rotation speed of the turret when in detection state.")
        );

        // Turret Charging Settings
        TurretChargingDurationCfg = config.Bind(
            new ConfigDefinition("Turret Charging Settings", "TurretChargingDuration"),
            2f,
            new ConfigDescription("The duration of the turret charging state.\nVanilla Turret Default value: 1.5")
        );
        TurretChargingRotationSpeedCfg = config.Bind(
            new ConfigDefinition("Turret Charging Settings", "TurretChargingRotationSpeed"),
            95f,
            new ConfigDescription("The rotation speed of the turret when in charging state.")
        );

        // Turret Firing Settings
        TurretFiringRotationSpeedCfg = config.Bind(
            new ConfigDefinition("Turret Firing Settings", "TurretFiringRotationSpeed"),
            95f,
            new ConfigDescription("The rotation speed of the turret when in firing state.")
        );

        // Turret Berserk Settings
        TurretBerserkDurationCfg = config.Bind(
            new ConfigDefinition("Turret Berserk Settings", "TurretBerserkDuration"),
            9f,
            new ConfigDescription("The duration of the turret berserk state.")
        );
        TurretBerserkRotationSpeedCfg = config.Bind(
            new ConfigDefinition("Turret Berserk Settings", "TurretBerserkRotationSpeed"),
            77f,
            new ConfigDescription("The rotation speed of the turret when in berserk state.")
        );
        #endregion
    }

    private string GetSpawnSettingsDescription(string planetName)
    {
        string description = $"Toil-Head spawn settings for {planetName}.\n";
        description += "MaxSpawnCount,SpawnChance\n";
        description += "<int>,<int>";

        return description;
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
