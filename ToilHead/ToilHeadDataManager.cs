using System.Collections.Generic;

namespace com.github.zehsteam.ToilHead;

internal class ToilHeadDataManager
{
    public static List<ToilHeadData> dataList { get; private set; }

    public static ToilHeadData otherData { get; private set; }

    public static void Initialize()
    {
        dataList = [
            new ToilHeadData("44 Liquidation", new ToilHeadConfigData(Plugin.ConfigManager.LiquidationSpawnSettings)),
            new ToilHeadData("5 Embrion", new ToilHeadConfigData(Plugin.ConfigManager.EmbrionSpawnSettings)),
            new ToilHeadData("68 Artifice", new ToilHeadConfigData(Plugin.ConfigManager.ArtificeSpawnSettings)),
            new ToilHeadData("8 Titan", new ToilHeadConfigData(Plugin.ConfigManager.TitanSpawnSettings)),
            new ToilHeadData("7 Dine", new ToilHeadConfigData(Plugin.ConfigManager.DineSpawnSettings)),
            new ToilHeadData("85 Rend", new ToilHeadConfigData(Plugin.ConfigManager.RendSpawnSettings)),
            new ToilHeadData("20 Adamance", new ToilHeadConfigData(Plugin.ConfigManager.AdamanceSpawnSettings)),
            new ToilHeadData("61 March", new ToilHeadConfigData(Plugin.ConfigManager.MarchSpawnSettings)),
            new ToilHeadData("21 Offense", new ToilHeadConfigData(Plugin.ConfigManager.OffenseSpawnSettings)),
            new ToilHeadData("56 Vow", new ToilHeadConfigData(Plugin.ConfigManager.VowSpawnSettings)),
            new ToilHeadData("220 Assurance", new ToilHeadConfigData(Plugin.ConfigManager.AssuranceSpawnSettings)),
            new ToilHeadData("41 Experimentation", new ToilHeadConfigData(Plugin.ConfigManager.ExperimentationSpawnSettings)),
        ];

        RegisterCustomSpawnSettings();

        otherData = new ToilHeadData("", new ToilHeadConfigData(Plugin.ConfigManager.OtherSpawnSettings));

        Plugin.logger.LogInfo("Finished initializing ToilHeadDataManager.");
    }

    private static void RegisterCustomSpawnSettings()
    {
        foreach (var entry in Plugin.ConfigManager.CustomSpawnSettings.Split(";"))
        {
            if (string.IsNullOrWhiteSpace(entry)) continue;

            string[] items = entry.Split(",");
            if (items.Length < 3) continue;

            string planetName = items[0];
            if (string.IsNullOrWhiteSpace(planetName)) continue;

            ToilHeadData newData = new ToilHeadData(planetName, new ToilHeadConfigData($"{items[1]},{items[2]}"));
            dataList.Add(newData);
            Plugin.logger.LogInfo($"Registered moon \"{planetName}\" with MaxSpawnCount: {newData.configData.maxSpawnCount}, SpawnChance: {newData.configData.spawnChance}");
        }
    }

    public static ToilHeadData GetDataForCurrentLevel()
    {
        string planetName = StartOfRound.Instance.currentLevel.PlanetName;

        foreach (var data in dataList)
        {
            if (data.planetName == planetName)
            {
                return data;
            }
        }

        return otherData;
    }
}
