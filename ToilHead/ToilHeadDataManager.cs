using System.Collections.Generic;
using System.Linq;

namespace com.github.zehsteam.ToilHead;

internal class ToilHeadDataManager
{
    public static List<ToilHeadData> dataList { get; private set; }

    public static ToilHeadData otherData { get; private set; }

    public static void Initialize()
    {
        dataList = [
            new ToilHeadData("44 Liquidation", new ToilHeadConfigData(Plugin.ConfigManager.LiquidationSpawnSettings.Value)),
            new ToilHeadData("5 Embrion", new ToilHeadConfigData(Plugin.ConfigManager.EmbrionSpawnSettings.Value)),
            new ToilHeadData("68 Artifice", new ToilHeadConfigData(Plugin.ConfigManager.ArtificeSpawnSettings.Value)),
            new ToilHeadData("8 Titan", new ToilHeadConfigData(Plugin.ConfigManager.TitanSpawnSettings.Value)),
            new ToilHeadData("7 Dine", new ToilHeadConfigData(Plugin.ConfigManager.DineSpawnSettings.Value)),
            new ToilHeadData("85 Rend", new ToilHeadConfigData(Plugin.ConfigManager.RendSpawnSettings.Value)),
            new ToilHeadData("20 Adamance", new ToilHeadConfigData(Plugin.ConfigManager.AdamanceSpawnSettings.Value)),
            new ToilHeadData("61 March", new ToilHeadConfigData(Plugin.ConfigManager.MarchSpawnSettings.Value)),
            new ToilHeadData("21 Offense", new ToilHeadConfigData(Plugin.ConfigManager.OffenseSpawnSettings.Value)),
            new ToilHeadData("56 Vow", new ToilHeadConfigData(Plugin.ConfigManager.VowSpawnSettings.Value)),
            new ToilHeadData("220 Assurance", new ToilHeadConfigData(Plugin.ConfigManager.AssuranceSpawnSettings.Value)),
            new ToilHeadData("41 Experimentation", new ToilHeadConfigData(Plugin.ConfigManager.ExperimentationSpawnSettings.Value)),
        ];

        RegisterCustomSpawnSettings();

        otherData = new ToilHeadData("", new ToilHeadConfigData(Plugin.ConfigManager.OtherSpawnSettings.Value));

        Plugin.logger.LogInfo("Finished initializing ToilHeadDataManager.");
    }

    private static void RegisterCustomSpawnSettings()
    {
        foreach (var entry in Plugin.ConfigManager.CustomSpawnSettings.Value.Split(","))
        {
            if (string.IsNullOrWhiteSpace(entry)) continue;

            string[] items = entry.Split(":").Select(_ => _.Trim()).ToArray();
            if (items.Length < 3) continue;

            string planetName = items[0];
            if (string.IsNullOrWhiteSpace(planetName)) continue;

            ToilHeadData newData = new ToilHeadData(planetName, new ToilHeadConfigData($"{items[1]},{items[2]}"));
            dataList.Add(newData);
            Plugin.logger.LogInfo($"Registered \"{planetName}\" moon with MaxSpawnCount: {newData.configData.maxSpawnCount}, SpawnChance: {newData.configData.spawnChance}");
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
