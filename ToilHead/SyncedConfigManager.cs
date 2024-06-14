using BepInEx.Configuration;
using com.github.zehsteam.ToilHead.MonoBehaviours;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace com.github.zehsteam.ToilHead;

public class SyncedConfigManager
{
    public SyncedConfigData hostConfigData;

    // General Settings
    public ExtendedConfigEntry<bool> EnableConfiguration;
    public ExtendedConfigEntry<bool> ExtendedLogging;

    // Toil-Head Settings
    public ExtendedConfigEntry<bool> SpawnToilHeadPlayerRagdolls;
    public ExtendedConfigEntry<bool> RealToilHeadPlayerRagdolls;
    public ExtendedConfigEntry<string> CustomSpawnSettings;
    public ExtendedConfigEntry<string> OtherSpawnSettings;
    public ExtendedConfigEntry<string> LiquidationSpawnSettings;
    public ExtendedConfigEntry<string> EmbrionSpawnSettings;
    public ExtendedConfigEntry<string> ArtificeSpawnSettings;
    public ExtendedConfigEntry<string> TitanSpawnSettings;
    public ExtendedConfigEntry<string> DineSpawnSettings;
    public ExtendedConfigEntry<string> RendSpawnSettings;
    public ExtendedConfigEntry<string> AdamanceSpawnSettings;
    public ExtendedConfigEntry<string> MarchSpawnSettings;
    public ExtendedConfigEntry<string> OffenseSpawnSettings;
    public ExtendedConfigEntry<string> VowSpawnSettings;
    public ExtendedConfigEntry<string> AssuranceSpawnSettings;
    public ExtendedConfigEntry<string> ExperimentationSpawnSettings;

    // Manti-Toil Settings
    public ExtendedConfigEntry<int> MantiToilMaxSpawnCount;
    public ExtendedConfigEntry<int> MantiToilSpawnChance;

    // Toil-Slayer Settings
    public ExtendedConfigEntry<int> ToilSlayerMaxSpawnCount;
    public ExtendedConfigEntry<int> ToilSlayerSpawnChance;

    // Plushie Settings
    public ExtendedConfigEntry<int> PlushieSpawnWeight;
    public ExtendedConfigEntry<bool> PlushieSpawnAllMoons;
    public ExtendedConfigEntry<string> PlushieMoonSpawnList;
    public ExtendedConfigEntry<int> PlushieCarryWeight;
    public ExtendedConfigEntry<int> PlushieMinValue;
    public ExtendedConfigEntry<int> PlushieMaxValue;

    // Turret Settings
    public ExtendedConfigEntry<float> TurretLostLOSDuration;
    public ExtendedConfigEntry<float> TurretRotationRange;
    public ExtendedConfigEntry<float> TurretCodeAccessCooldownDuration;

    // Turret Detection Settings
    public ExtendedConfigEntry<bool> TurretDetectionRotation;
    public ExtendedConfigEntry<float> TurretDetectionRotationSpeed;

    // Turret Charging Settings
    public ExtendedConfigEntry<float> TurretChargingDuration;
    public ExtendedConfigEntry<float> TurretChargingRotationSpeed;

    // Turret Firing Settings
    public ExtendedConfigEntry<float> TurretFiringRotationSpeed;

    // Turret Berserk Settings
    public ExtendedConfigEntry<float> TurretBerserkDuration;
    public ExtendedConfigEntry<float> TurretBerserkRotationSpeed;

    public SyncedConfigManager()
    {
        BindConfigs();
        ClearUnusedEntries();
    }

    private void BindConfigs()
    {
        // General Settings
        EnableConfiguration = new("General Settings", "EnableConfiguration", defaultValue: false, "Enable if you want to use custom set config setting values. If disabled, the default config setting values will be used.");
        ExtendedLogging = new("General Settings", "ExtendedLogging", defaultValue: false, "Enable extended logging.");

        // Toil-Head Settings
        SpawnToilHeadPlayerRagdolls = new("Toil-Head Settings", "SpawnToilHeadPlayerRagdolls", defaultValue: true, "If enabled, will spawn a Toil-Head player ragdoll when a player dies to a Toil-Head in any way.", useEnableConfiguration: true);
        RealToilHeadPlayerRagdolls = new("Toil-Head Settings", "RealToilHeadPlayerRagdolls", defaultValue: true, "If enabled, will spawn a real turret on the Toil-Head player ragdoll.", useEnableConfiguration: true);
        CustomSpawnSettings = new("Toil-Head Settings", "CustomSpawnSettings", defaultValue: "57 Asteroid-13:2:30,523 Ooblterra:3:80,", GetOtherSpawnSettingsDescription(), useEnableConfiguration: true);
        OtherSpawnSettings = new("Toil-Head Settings", "OtherSpawnSettings", defaultValue: "1,30", GetOtherSpawnSettingsDescription(), useEnableConfiguration: true);
        LiquidationSpawnSettings = new("Toil-Head Settings", "LiquidationSpawnSettings", defaultValue: "1,30", GetSpawnSettingsDescription("44-Liquidation"), useEnableConfiguration: true);
        EmbrionSpawnSettings = new("Toil-Head Settings", "EmbrionSpawnSettings", defaultValue: "1, 20", GetSpawnSettingsDescription("5-Embrion"), useEnableConfiguration: true);
        ArtificeSpawnSettings = new("Toil-Head Settings", "ArtificeSpawnSettings", defaultValue: "2,70", GetSpawnSettingsDescription("68-Artifice"), useEnableConfiguration: true);
        TitanSpawnSettings = new("Toil-Head Settings", "TitanSpawnSettings", defaultValue: "2,50", GetSpawnSettingsDescription("8-Titan"), useEnableConfiguration: true);
        DineSpawnSettings = new("Toil-Head Settings", "DineSpawnSettings", defaultValue: "1,45", GetSpawnSettingsDescription("7-Dine"), useEnableConfiguration: true);
        RendSpawnSettings = new("Toil-Head Settings", "RendSpawnSettings", defaultValue: "1,40", GetSpawnSettingsDescription("85-Rend"), useEnableConfiguration: true);
        AdamanceSpawnSettings = new("Toil-Head Settings", "AdamanceSpawnSettings", defaultValue: "1,30", GetSpawnSettingsDescription("20-Adamance"), useEnableConfiguration: true);
        MarchSpawnSettings = new("Toil-Head Settings", "MarchSpawnSettings", defaultValue: "1,20", GetSpawnSettingsDescription("61-March"), useEnableConfiguration: true);
        OffenseSpawnSettings = new("Toil-Head Settings", "OffenseSpawnSettings", defaultValue: "1,20", GetSpawnSettingsDescription("21-Offense"), useEnableConfiguration: true);
        VowSpawnSettings = new("Toil-Head Settings", "VowSpawnSettings", defaultValue: "1,20", GetSpawnSettingsDescription("56-Vow"), useEnableConfiguration: true);
        AssuranceSpawnSettings = new("Toil-Head Settings", "AssuranceSpawnSettings", defaultValue: "1,20", GetSpawnSettingsDescription("220-Assurance"), useEnableConfiguration: true);
        ExperimentationSpawnSettings = new("Toil-Head Settings", "ExperimentationSpawnSettings", defaultValue: "1,10", GetSpawnSettingsDescription("41-Experimentation"), useEnableConfiguration: true);

        // Manti-Toil Settings
        MantiToilMaxSpawnCount = new("Manti-Toil Settings", "MantiToilMaxSpawnCount", defaultValue: 5, "Manti-Toil max spawn count.", useEnableConfiguration: true);
        MantiToilSpawnChance = new("Manti-Toil Settings", "MantiToilSpawnChance", defaultValue: 50, "The percent chance a Manticoil turns into a Manti-Toil.", useEnableConfiguration: true);

        // Toil-Slayer Settings
        ToilSlayerMaxSpawnCount = new("Toil-Slayer Settings", "ToilSlayerMaxSpawnCount", defaultValue: 1, "Toil-Slayer max spawn count.", useEnableConfiguration: true);
        ToilSlayerSpawnChance = new("Toil-Slayer Settings", "ToilSlayerSpawnChance", defaultValue: 10, "The percent chance a Coil-Head turns into a Toil-Slayer.", useEnableConfiguration: true);

        // Plushie Settings
        PlushieSpawnWeight = new("Plushie Settings", "PlushieSpawnWeight", defaultValue: 10, "Toil-Head plushie spawn chance weight.", useEnableConfiguration: true);
        PlushieSpawnAllMoons = new("Plushie Settings", "PlushieSpawnAllMoons", defaultValue: true, "If true, the Toil-Head plushie will spawn on all moons. If false, the Toil-Head plushie will only spawn on moons set in the moons list.", useEnableConfiguration: true);
        PlushieMoonSpawnList = new("Plushie Settings", "PlushieMoonSpawnList", defaultValue: "Experimentation, Assurance, Vow, Offense, March, Adamance, Rend, Dine, Titan, Artifice, Embrion", "The list of moons the Toil-Head plushie will spawn on.\nCurrently only works for vanilla moons.\nOnly works if PlushieSpawnAllMoons is false.", useEnableConfiguration: true);
        PlushieCarryWeight = new("Plushie Settings", "PlushieCarryWeight", defaultValue: 6, "Toil-Head plushie carry weight in pounds.", useEnableConfiguration: true);
        PlushieMinValue = new("Plushie Settings", "PlushieMinValue", defaultValue: 80, "Toil-Head plushie min scrap value.", useEnableConfiguration: true);
        PlushieMaxValue = new("Plushie Settings", "PlushieMaxValue", defaultValue: 250, "Toil-Head plushie max scrap value.", useEnableConfiguration: true);

        // Turret Settings
        TurretLostLOSDuration = new("Turret Settings", "TurretLostLOSDuration", defaultValue: 0.75f, "The duration until the turret loses the target player when not in line of sight.\nVanilla Turret Default value: 2", useEnableConfiguration: true);
        TurretLostLOSDuration.GetValue = () =>
        {
            return hostConfigData == null ? TurretLostLOSDuration.ConfigEntry.Value : hostConfigData.turretLostLOSDuration;
        };
        TurretRotationRange = new("Turret Settings", "TurretRotationRange", defaultValue: 75f, "The rotation range of the turret in degrees.\nVanilla Turret Default value: 75", useEnableConfiguration: true);
        TurretRotationRange.GetValue = () =>
        {
            return hostConfigData == null ? TurretRotationRange.ConfigEntry.Value : hostConfigData.turretRotationRange;
        };
        TurretCodeAccessCooldownDuration = new("Turret Settings", "TurretCodeAccessCooldownDuration", defaultValue: 7f, "The duration of the turret being disabled from the terminal in seconds.\nVanilla Turret Default value: 7", useEnableConfiguration: true);
        TurretCodeAccessCooldownDuration.GetValue = () =>
        {
            return hostConfigData == null ? TurretCodeAccessCooldownDuration.ConfigEntry.Value : hostConfigData.turretCodeAccessCooldownDuration;
        };

        // Turret Detection Settings
        TurretDetectionRotation = new("Turret Detection Settings", "TurretDetectionRotation", defaultValue: false, "If enabled, the turret will rotate when searching for players.\nVanilla Turret Default value: true", useEnableConfiguration: true);
        TurretDetectionRotation.GetValue = () =>
        {
            return hostConfigData == null ? TurretDetectionRotation.ConfigEntry.Value : hostConfigData.turretDetectionRotation;
        };
        TurretDetectionRotationSpeed = new("Turret Detection Settings", "TurretDetectionRotationSpeed", defaultValue: 28f, "The rotation speed of the turret when in detection state.\nVanilla Turret Default value: 28", useEnableConfiguration: true);
        TurretDetectionRotationSpeed.GetValue = () =>
        {
            return hostConfigData == null ? TurretDetectionRotationSpeed.ConfigEntry.Value : hostConfigData.turretDetectionRotationSpeed;
        };

        // Turret Charging Settings
        TurretChargingDuration = new("Turret Charging Settings", "TurretChargingDuration", defaultValue: 2f, "The duration of the turret charging state.\nVanilla Turret Default value: 1.5", useEnableConfiguration: true);
        TurretChargingDuration.GetValue = () =>
        {
            return hostConfigData == null ? TurretChargingDuration.ConfigEntry.Value : hostConfigData.turretChargingDuration;
        };
        TurretChargingRotationSpeed = new("Turret Charging Settings", "TurretChargingRotationSpeed", defaultValue: 95f, "The rotation speed of the turret when in charging state.\nVanilla Turret Default value: 95", useEnableConfiguration: true);
        TurretChargingRotationSpeed.GetValue = () =>
        {
            return hostConfigData == null ? TurretChargingRotationSpeed.ConfigEntry.Value : hostConfigData.turretChargingRotationSpeed;
        };

        // Turret Firing Settings
        TurretFiringRotationSpeed = new("Turret Firing Settings", "TurretFiringRotationSpeed", defaultValue: 95f, "The rotation speed of the turret when in firing state.\nVanilla Turret Default value: 95", useEnableConfiguration: true);
        TurretFiringRotationSpeed.GetValue = () =>
        {
            return hostConfigData == null ? TurretFiringRotationSpeed.ConfigEntry.Value : hostConfigData.turretFiringRotationSpeed;
        };

        // Turret Berserk Settings
        TurretBerserkDuration = new("Turret Berserk Settings", "TurretBerserkDuration", defaultValue: 9f, "The duration of the turret berserk state.\nVanilla Turret Default value: 9", useEnableConfiguration: true);
        TurretBerserkDuration.GetValue = () =>
        {
            return hostConfigData == null ? TurretBerserkDuration.ConfigEntry.Value : hostConfigData.turretBerserkDuration;
        };
        TurretBerserkRotationSpeed = new("Turret Berserk Settings", "TurretBerserkRotationSpeed", defaultValue: 77f, "The rotation speed of the turret when in berserk state.\nVanilla Turret Default value: 77", useEnableConfiguration: true);
        TurretBerserkRotationSpeed.GetValue = () =>
        {
            return hostConfigData == null ? TurretBerserkRotationSpeed.ConfigEntry.Value : hostConfigData.turretBerserkRotationSpeed;
        };
    }

    private string GetSpawnSettingsDescription(string planetName)
    {
        string description = $"Toil-Head spawn settings for {planetName}.\n";
        description += "MaxSpawnCount,SpawnChance\n";
        description += "<int>,<int>";

        return description;
    }

    private string GetCustomSpawnSettingsDescription()
    {
        string description = $"Toil-Head spawn settings for modded moons.\n";
        description += $"This setting will override OtherSpawnSettings for the specified moons.\n";
        description += $"This setting uses SelectableLevel.PlanetName\n";
        description += "PlanetName:MaxSpawnCount:SpawnChance,\n";
        description += "<string>:<int>:<int>,";

        return description;
    }

    private string GetOtherSpawnSettingsDescription()
    {
        string description = $"Toil-Head default spawn settings for modded moons.\n";
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

public class ExtendedConfigEntry<T>
{
    public ConfigEntry<T> ConfigEntry;
    public Func<T> GetValue;
    public Action<T> SetValue;

    public T DefaultValue => (T)ConfigEntry.DefaultValue;
    public T Value { get { return GetValue(); } set { SetValue(value); } }

    public bool UseEnableConfiguration = false;

    public ExtendedConfigEntry(string section, string key, T defaultValue, string description, bool useEnableConfiguration = false)
    {
        ConfigEntry = Plugin.Instance.Config.Bind(section, key, defaultValue, description);
        UseEnableConfiguration = useEnableConfiguration;
        Initialize();
    }

    public ExtendedConfigEntry(string section, string key, T defaultValue, ConfigDescription configDescription = null, bool useEnableConfiguration = false)
    {
        ConfigEntry = Plugin.Instance.Config.Bind(section, key, defaultValue, configDescription);
        UseEnableConfiguration = useEnableConfiguration;
        Initialize();
    }

    private void Initialize()
    {
        if (GetValue == null)
        {
            GetValue = () =>
            {
                if (UseEnableConfiguration && !Plugin.ConfigManager.EnableConfiguration.Value)
                {
                    return DefaultValue;
                }

                return ConfigEntry.Value;
            };
        }

        if (SetValue == null)
        {
            SetValue = (T value) =>
            {
                ConfigEntry.Value = value;
            };
        }
    }

    public void ResetToDefault()
    {
        ConfigEntry.Value = (T)ConfigEntry.DefaultValue;
    }
}
