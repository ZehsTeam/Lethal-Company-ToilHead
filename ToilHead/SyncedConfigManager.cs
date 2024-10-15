using BepInEx.Configuration;
using com.github.zehsteam.ToilHead.MonoBehaviours;
using System.Collections.Generic;
using System.Reflection;

namespace com.github.zehsteam.ToilHead;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class SyncedConfigManager
{
    public SyncedConfigData HostConfigData { get; private set; }

    #region Config Settings
    // General Settings
    public ExtendedConfigEntry<bool> EnableConfiguration { get; private set; }
    public ExtendedConfigEntry<bool> ExtendedLogging { get; private set; }

    // Toilation Settings
    public ExtendedConfigEntry<string> ToilationToilPlayerSpawnSettings { get; private set; }
    public ExtendedConfigEntry<float> ToilationToilPlayerSlayerChance { get; private set; }
    public ExtendedConfigEntry<string> ToilationToilHeadSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> ToilationToilSlayerSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> ToilationMantiToilSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> ToilationMantiSlayerSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> ToilationToilMaskedSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> ToilationSlayerMaskedSpawnSettings { get; private set; }

    // Toil-Player Settings
    public ExtendedConfigEntry<string> ToilPlayerDefaultSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> ToilPlayerSpawnSettingsMoonList { get; private set; }
    public ExtendedConfigEntry<float> ToilPlayerSlayerChance { get; private set; }

    // Player Ragdoll Settings
    public ExtendedConfigEntry<bool> SpawnToiledPlayerRagdolls { get; private set; }
    public ExtendedConfigEntry<bool> SpawnRealToiledPlayerRagdolls { get; private set; }

    // Toil-Head Settings
    public ExtendedConfigEntry<string> ToilHeadDefaultSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> ToilHeadSpawnSettingsMoonList { get; private set; }

    // Manti-Toil Settings
    public ExtendedConfigEntry<string> MantiToilDefaultSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> MantiToilSpawnSettingsMoonList { get; private set; }

    // Toil-Slayer Settings
    public ExtendedConfigEntry<string> ToilSlayerDefaultSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> ToilSlayerSpawnSettingsMoonList { get; private set; }

    // Manti-Slayer Settings
    public ExtendedConfigEntry<string> MantiSlayerDefaultSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> MantiSlayerSpawnSettingsMoonList { get; private set; }

    // Toil-Masked Settings
    public ExtendedConfigEntry<string> ToilMaskedDefaultSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> ToilMaskedSpawnSettingsMoonList { get; private set; }

    // Slayer-Masked Settings
    public ExtendedConfigEntry<string> SlayerMaskedDefaultSpawnSettings { get; private set; }
    public ExtendedConfigEntry<string> SlayerMaskedSpawnSettingsMoonList { get; private set; }

    // Toil-Head Plushie Settings
    public ExtendedConfigEntry<int> ToilHeadPlushieSpawnWeight { get; private set; }
    public ExtendedConfigEntry<bool> ToilHeadPlushieSpawnAllMoons { get; private set; }
    public ExtendedConfigEntry<string> ToilHeadPlushieMoonSpawnList { get; private set; }
    public ExtendedConfigEntry<int> ToilHeadPlushieCarryWeight { get; private set; }
    public ExtendedConfigEntry<int> ToilHeadPlushieMinValue { get; private set; }
    public ExtendedConfigEntry<int> ToilHeadPlushieMaxValue { get; private set; }

    // Toil-Slayer Plushie Settings
    public ExtendedConfigEntry<int> ToilSlayerPlushieSpawnWeight { get; private set; }
    public ExtendedConfigEntry<bool> ToilSlayerPlushieSpawnAllMoons { get; private set; }
    public ExtendedConfigEntry<string> ToilSlayerPlushieMoonSpawnList { get; private set; }
    public ExtendedConfigEntry<int> ToilSlayerPlushieCarryWeight { get; private set; }
    public ExtendedConfigEntry<int> ToilSlayerPlushieMinValue { get; private set; }
    public ExtendedConfigEntry<int> ToilSlayerPlushieMaxValue { get; private set; }

    #region Turret Settings
    // Turret Settings
    public ExtendedConfigEntry<float> TurretLostLOSDuration { get; private set; }
    public ExtendedConfigEntry<float> TurretRotationRange { get; private set; }
    public ExtendedConfigEntry<float> TurretCodeAccessCooldownDuration { get; private set; }

    // Turret Detection Settings
    public ExtendedConfigEntry<bool> TurretDetectionRotation { get; private set; }
    public ExtendedConfigEntry<float> TurretDetectionRotationSpeed { get; private set; }

    // Turret Charging Settings
    public ExtendedConfigEntry<float> TurretChargingDuration { get; private set; }
    public ExtendedConfigEntry<float> TurretChargingRotationSpeed { get; private set; }

    // Turret Firing Settings
    public ExtendedConfigEntry<float> TurretFiringRotationSpeed { get; private set; }

    // Turret Berserk Settings
    public ExtendedConfigEntry<float> TurretBerserkDuration { get; private set; }
    public ExtendedConfigEntry<float> TurretBerserkRotationSpeed { get; private set; }
    #endregion
    #endregion

    public SyncedConfigManager()
    {
        BindConfigs();
        ClearUnusedEntries();
    }

    private void BindConfigs()
    {
        // General Settings
        EnableConfiguration = new("General Settings", "EnableConfiguration", defaultValue: false, "Enable if you want to use custom set config setting values. If disabled, the default config setting values will be used.", useEnableConfiguration: false);
        ExtendedLogging = new("General Settings", "ExtendedLogging", defaultValue: false, "Enable extended logging.", useEnableConfiguration: false);

        // Toilation Settings
        ToilationToilPlayerSpawnSettings = new("Toilation Settings", "ToilPlayerSpawnSettings", defaultValue: "1:7", GetDescriptionForMoonSpawnSettings("Toil-Player", "69-Toilation"));
        ToilationToilPlayerSlayerChance = new("Toilation Settings", "ToilPlayerSlayerChance", defaultValue: 20, "The percent chance a Toil-Player will become a Slayer-Player for 69-Toilation.");
        ToilationToilHeadSpawnSettings = new("Toilation Settings", "ToilHeadSpawnSettings", defaultValue: "6:75", GetDescriptionForMoonSpawnSettings("Toil-Head", "69-Toilation"));
        ToilationMantiToilSpawnSettings = new("Toilation Settings", "MantiToilSpawnSettings", defaultValue: "50:90", GetDescriptionForMoonSpawnSettings("Manti-Toil", "69-Toilation"));
        ToilationToilSlayerSpawnSettings = new("Toilation Settings", "ToilSlayerSpawnSettings", defaultValue: "2:10", GetDescriptionForMoonSpawnSettings("Toil-Slayer", "69-Toilation"));
        ToilationMantiSlayerSpawnSettings = new("Toilation Settings", "MantiSlayerSpawnSettings", defaultValue: "2:20", GetDescriptionForMoonSpawnSettings("Manti-Slayer", "69-Toilation"));
        ToilationToilMaskedSpawnSettings = new("Toilation Settings", "ToilMaskedSpawnSettings", defaultValue: "2:15", GetDescriptionForMoonSpawnSettings("Toil-Masked", "69-Toilation"));
        ToilationSlayerMaskedSpawnSettings = new("Toilation Settings", "SlayerMaskedSpawnSettings", defaultValue: "1:5", GetDescriptionForMoonSpawnSettings("Slayer-Masked", "69-Toilation"));

        // Toil-Player Settings
        ToilPlayerDefaultSpawnSettings = new("Toil-Player Settings", "ToilPlayerDefaultSpawnSettings", defaultValue: "1:3", GetDescriptionForDefaultSpawnSettings("Toil-Player"));
        string toilPlayerSpawnSettingMoonListValue = "85 Rend:1:4, 7 Dine:1:4, 8 Titan:1:5, 68 Artifice:1:5, 57 Asteroid-13:1:5, 523 Ooblterra:1:6";
        ToilPlayerSpawnSettingsMoonList = new("Toil-Player Settings", "ToilPlayerSpawnSettingsMoonList", defaultValue: toilPlayerSpawnSettingMoonListValue, GetDescriptionForMoonSpawnSettingsList("Toil-Player"));
        ToilPlayerSlayerChance = new("Toil-Player Settings", "ToilPlayerSlayerChance", defaultValue: 10, "The percent chance a Toil-Player will become a Slayer-Player.");

        // Player Ragdoll Settings
        SpawnToiledPlayerRagdolls = new("Player Ragdoll Settings", "SpawnToiledPlayerRagdolls", defaultValue: true, "If enabled, will spawn a Toiled player ragdoll when a player dies to a Turret-Head in any way.");
        SpawnRealToiledPlayerRagdolls = new("Player Ragdoll Settings", "SpawnRealToiledPlayerRagdolls", defaultValue: true, "If enabled, will spawn a real turret on the Toiled player ragdoll.");

        // Toil-Head Settings
        ToilHeadDefaultSpawnSettings = new("Toil-Head Settings", "ToilHeadDefaultSpawnSettings", defaultValue: "1:30", GetDescriptionForDefaultSpawnSettings("Toil-Head"));
        string toilHeadSpawnSettingMoonListValue = "41 Experimentation:1:10, 220 Assurance:1:20, 56 Vow:1:20, 21 Offense:1:20, 61 March:1:20, 20 Adamance:1:30, 85 Rend:1:40, 7 Dine:1:45, 8 Titan:1:50, 68 Artifice:2:70, 5 Embrion:1:30, 57 Asteroid-13:2:30, 523 Ooblterra:2:70";
        ToilHeadSpawnSettingsMoonList = new("Toil-Head Settings", "ToilHeadSpawnSettingsMoonList", defaultValue: toilHeadSpawnSettingMoonListValue, GetDescriptionForMoonSpawnSettingsList("Toil-Head"));

        // Manti-Toil Settings
        MantiToilDefaultSpawnSettings = new("Manti-Toil Settings", "MantiToilDefaultSpawnSettings", defaultValue: "5:50", GetDescriptionForDefaultSpawnSettings("Manti-Toil"));
        string mantiToilSpawnSettingsMoonListValue = "20 Adamance:5:60, 85 Rend:5:60, 7 Dine:5:65, 8 Titan:5:70, 68 Artifice:8:75";
        MantiToilSpawnSettingsMoonList = new("Manti-Toil Settings", "MantiToilSpawnSettingsMoonList", defaultValue: mantiToilSpawnSettingsMoonListValue, GetDescriptionForMoonSpawnSettingsList("Manti-Toil"));

        // Toil-Slayer Settings
        ToilSlayerDefaultSpawnSettings = new("Toil-Slayer Settings", "ToilSlayerDefaultSpawnSettings", defaultValue: "1:10", GetDescriptionForDefaultSpawnSettings("Toil-Slayer"));
        string toilSlayerSpawnSettingsMoonListValue = "20 Adamance:1:15, 85 Rend:1:15, 7 Dine:1:15, 8 Titan:1:20, 68 Artifice:1:20, 57 Asteroid-13:1:15, 523 Ooblterra:1:25";
        ToilSlayerSpawnSettingsMoonList = new("Toil-Slayer Settings", "ToilSlayerSpawnSettingsMoonList", defaultValue: toilSlayerSpawnSettingsMoonListValue, GetDescriptionForMoonSpawnSettingsList("Toil-Slayer"));

        // Manti-Slayer Settings
        MantiSlayerDefaultSpawnSettings = new("Manti-Slayer Settings", "MantiSlayerDefaultSpawnSettings", defaultValue: "1:10", GetDescriptionForDefaultSpawnSettings("Manti-Slayer"));
        string mantiSlayerSpawnSettingsMoonListValue = "20 Adamance:1:15, 85 Rend:1:15, 7 Dine:1:15, 8 Titan:1:20, 68 Artifice:1:20, 57 Asteroid-13:1:15, 523 Ooblterra:1:25";
        MantiSlayerSpawnSettingsMoonList = new("Manti-Slayer Settings", "MantiSlayerSpawnSettingsMoonList", defaultValue: mantiSlayerSpawnSettingsMoonListValue, GetDescriptionForMoonSpawnSettingsList("Manti-Slayer"));

        // Toil-Masked Settings
        ToilMaskedDefaultSpawnSettings = new("Toil-Masked Settings", "ToilMaskedDefaultSpawnSettings", defaultValue: "1:10", GetDescriptionForDefaultSpawnSettings("Toil-Masked"));
        string toilMaskedSpawnSettingMoonListValue = "";
        ToilMaskedSpawnSettingsMoonList = new("Toil-Masked Settings", "ToilMaskedSpawnSettingsMoonList", defaultValue: toilMaskedSpawnSettingMoonListValue, GetDescriptionForMoonSpawnSettingsList("Toil-Masked"));

        // Slayer-Masked Settings
        SlayerMaskedDefaultSpawnSettings = new("Slayer-Masked Settings", "SlayerMaskedDefaultSpawnSettings", defaultValue: "1:5", GetDescriptionForDefaultSpawnSettings("Slayer-Masked"));
        string slayerMaskedSpawnSettingMoonListValue = "";
        SlayerMaskedSpawnSettingsMoonList = new("Slayer-Masked Settings", "SlayerMaskedSpawnSettingsMoonList", defaultValue: slayerMaskedSpawnSettingMoonListValue, GetDescriptionForMoonSpawnSettingsList("Slayer-Masked"));

        // Toil-Head Plushie Settings
        ToilHeadPlushieSpawnWeight = new("Toil-Head Plushie Settings", "SpawnWeight", defaultValue: 10, "Toil-Head plushie spawn chance weight.");
        ToilHeadPlushieSpawnAllMoons = new("Toil-Head Plushie Settings", "SpawnAllMoons", defaultValue: true, "If true, the Toil-Head plushie will spawn on all moons. If false, the Toil-Head plushie will only spawn on moons set in the moons list.");
        ToilHeadPlushieMoonSpawnList = new("Toil-Head Plushie Settings", "MoonSpawnList", defaultValue: "Experimentation, Assurance, Vow, Offense, March, Adamance, Rend, Dine, Titan, Artifice, Embrion", "The list of moons the Toil-Head plushie will spawn on.\nCurrently only works for vanilla moons.\nOnly works if PlushieSpawnAllMoons is false.");
        ToilHeadPlushieCarryWeight = new("Toil-Head Plushie Settings", "CarryWeight", defaultValue: 6, "Toil-Head plushie carry weight in pounds.");
        ToilHeadPlushieMinValue = new("Toil-Head Plushie Settings", "MinValue", defaultValue: 80, "Toil-Head plushie min scrap value.");
        ToilHeadPlushieMaxValue = new("Toil-Head Plushie Settings", "MaxValue", defaultValue: 250, "Toil-Head plushie max scrap value.");

        // Toil-Slayer Plushie Settings
        ToilSlayerPlushieSpawnWeight = new("Toil-Slayer Plushie Settings", "SpawnWeight", defaultValue: 5, "Toil-Slayer plushie spawn chance weight.");
        ToilSlayerPlushieSpawnAllMoons = new("Toil-Slayer Plushie Settings", "SpawnAllMoons", defaultValue: true, "If true, the Toil-Slayer plushie will spawn on all moons. If false, the Toil-Slayer plushie will only spawn on moons set in the moons list.");
        ToilSlayerPlushieMoonSpawnList = new("Toil-Slayer Plushie Settings", "MoonSpawnList", defaultValue: "Experimentation, Assurance, Vow, Offense, March, Adamance, Rend, Dine, Titan, Artifice, Embrion", "The list of moons the Toil-Slayer plushie will spawn on.\nCurrently only works for vanilla moons.\nOnly works if PlushieSpawnAllMoons is false.");
        ToilSlayerPlushieCarryWeight = new("Toil-Slayer Plushie Settings", "CarryWeight", defaultValue: 12, "Toil-Slayer plushie carry weight in pounds.");
        ToilSlayerPlushieMinValue = new("Toil-Slayer Plushie Settings", "MinValue", defaultValue: 150, "Toil-Slayer plushie min scrap value.");
        ToilSlayerPlushieMaxValue = new("Toil-Slayer Plushie Settings", "MaxValue", defaultValue: 380, "Toil-Slayer plushie max scrap value.");

        #region Turret Settings
        // Turret Settings
        TurretLostLOSDuration = new("Turret Settings", "LostLOSDuration", defaultValue: 0.75f, "The duration until the turret loses the target player when not in line of sight.\nVanilla Turret Default value: 2");
        TurretLostLOSDuration.GetValue = () =>
        {
            return HostConfigData == null ? TurretLostLOSDuration.ConfigEntry.Value : HostConfigData.TurretLostLOSDuration;
        };
        TurretRotationRange = new("Turret Settings", "RotationRange", defaultValue: 75f, "The rotation range of the turret in degrees.\nVanilla Turret Default value: 75");
        TurretRotationRange.GetValue = () =>
        {
            return HostConfigData == null ? TurretRotationRange.ConfigEntry.Value : HostConfigData.TurretRotationRange;
        };
        TurretCodeAccessCooldownDuration = new("Turret Settings", "CodeAccessCooldownDuration", defaultValue: 7f, "The duration of the turret being disabled from the terminal in seconds.\nVanilla Turret Default value: 7");
        TurretCodeAccessCooldownDuration.GetValue = () =>
        {
            return HostConfigData == null ? TurretCodeAccessCooldownDuration.ConfigEntry.Value : HostConfigData.TurretCodeAccessCooldownDuration;
        };

        // Turret Detection Settings
        TurretDetectionRotation = new("Turret Detection Settings", "Rotation", defaultValue: false, "If enabled, the turret will rotate when searching for players.\nVanilla Turret Default value: true");
        TurretDetectionRotation.GetValue = () =>
        {
            return HostConfigData == null ? TurretDetectionRotation.ConfigEntry.Value : HostConfigData.TurretDetectionRotation;
        };
        TurretDetectionRotationSpeed = new("Turret Detection Settings", "RotationSpeed", defaultValue: 28f, "The rotation speed of the turret when in detection state.\nVanilla Turret Default value: 28");
        TurretDetectionRotationSpeed.GetValue = () =>
        {
            return HostConfigData == null ? TurretDetectionRotationSpeed.ConfigEntry.Value : HostConfigData.TurretDetectionRotationSpeed;
        };

        // Turret Charging Settings
        TurretChargingDuration = new("Turret Charging Settings", "ChargingDuration", defaultValue: 2f, "The duration of the turret charging state.\nVanilla Turret Default value: 1.5");
        TurretChargingDuration.GetValue = () =>
        {
            return HostConfigData == null ? TurretChargingDuration.ConfigEntry.Value : HostConfigData.TurretChargingDuration;
        };
        TurretChargingRotationSpeed = new("Turret Charging Settings", "RotationSpeed", defaultValue: 95f, "The rotation speed of the turret when in charging state.\nVanilla Turret Default value: 95");
        TurretChargingRotationSpeed.GetValue = () =>
        {
            return HostConfigData == null ? TurretChargingRotationSpeed.ConfigEntry.Value : HostConfigData.TurretChargingRotationSpeed;
        };

        // Turret Firing Settings
        TurretFiringRotationSpeed = new("Turret Firing Settings", "RotationSpeed", defaultValue: 95f, "The rotation speed of the turret when in firing state.\nVanilla Turret Default value: 95");
        TurretFiringRotationSpeed.GetValue = () =>
        {
            return HostConfigData == null ? TurretFiringRotationSpeed.ConfigEntry.Value : HostConfigData.TurretFiringRotationSpeed;
        };

        // Turret Berserk Settings
        TurretBerserkDuration = new("Turret Berserk Settings", "BerserkDuration", defaultValue: 9f, "The duration of the turret berserk state.\nVanilla Turret Default value: 9");
        TurretBerserkDuration.GetValue = () =>
        {
            return HostConfigData == null ? TurretBerserkDuration.ConfigEntry.Value : HostConfigData.TurretBerserkDuration;
        };
        TurretBerserkRotationSpeed = new("Turret Berserk Settings", "RotationSpeed", defaultValue: 77f, "The rotation speed of the turret when in berserk state.\nVanilla Turret Default value: 77");
        TurretBerserkRotationSpeed.GetValue = () =>
        {
            return HostConfigData == null ? TurretBerserkRotationSpeed.ConfigEntry.Value : HostConfigData.TurretBerserkRotationSpeed;
        };
        #endregion
    }

    #region Get Description
    private string GetDescriptionForSpawnSettings(string enemyName)
    {
        string description = $"{enemyName} spawn settings.\n";
        description += "MaxSpawnCount,SpawnChance\n";
        description += "<int>,<float>";

        return description;
    }

    private string GetDescriptionForMoonSpawnSettings(string enemyName, string planetName)
    {
        string description = $"{enemyName} spawn settings for {planetName}.\n";
        description += "MaxSpawnCount,SpawnChance\n";
        description += "<int>,<float>";

        return description;
    }

    private string GetDescriptionForMoonSpawnSettingsList(string enemyName)
    {
        string description = $"{enemyName} spawn settings list for moons.\n";
        description += "Separate each entry with a comma.\n";
        description += "PlanetName:MaxSpawnCount:SpawnChance\n";
        description += "<string>:<int>:<float>";

        return description;
    }

    private string GetDescriptionForDefaultSpawnSettings(string enemyName)
    {
        string description = $"{enemyName} default spawn settings for all moons.\n";
        description += "MaxSpawnCount:SpawnChance\n";
        description += "<int>:<float>";

        return description;
    }
    #endregion

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
        HostConfigData = syncedConfigData;
    }

    private void SyncedConfigsChanged()
    {
        if (!NetworkUtils.IsServer) return;

        PluginNetworkBehaviour.Instance.SendConfigToPlayerClientRpc(new SyncedConfigData(this));
    }
}
