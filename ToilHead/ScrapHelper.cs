using BepInEx.Bootstrap;
using System.Collections.Generic;
using System.Linq;
using static LethalLib.Modules.Levels;

namespace com.github.zehsteam.ToilHead;

internal class ScrapHelper
{
    public const string MonsterPlushiesGUID = "scin.monsterplushies";

    public static bool HasLethalLib()
    {
        return Chainloader.PluginInfos.ContainsKey(LethalLib.Plugin.ModGUID);
    }

    public static bool HasMonsterPlushieMod()
    {
        return Chainloader.PluginInfos.ContainsKey(MonsterPlushiesGUID);
    }

    public static void RegisterScrap(Item item, int iRarity, bool spawnAllMoons, string moonSpawnList, bool twoHanded, int carryWeight, int minValue, int maxValue)
    {
        if (!HasLethalLib()) return;
        if (!HasMonsterPlushieMod()) return;

        try
        {
            item.twoHanded = twoHanded;
            item.weight = carryWeight / 105f + 1f;
            item.minValue = minValue;
            item.maxValue = maxValue;

            LethalLib.Modules.Utilities.FixMixerGroups(item.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(item.spawnPrefab);

            if (spawnAllMoons)
            {
                LethalLib.Modules.Items.RegisterScrap(item, iRarity, LevelTypes.All);
                Plugin.logger.LogInfo($"Registered \"{item.itemName}\" scrap item with {iRarity} rarity.");
            }
            else
            {
                RegisterScrapForMoons(item, iRarity, moonSpawnList);
            }
        }
        catch (System.Exception e)
        {
            Plugin.logger.LogError($"Error: Failed to register \"{item.itemName}\" scrap item.\n\n{e}");
        }
    }
    
    private static void RegisterScrapForMoons(Item item, int iRarity, string moonSpawnList)
    {
        Dictionary<string, LevelTypes> vanillaMoonLevelTypePair = [];
        vanillaMoonLevelTypePair.Add("Experimentation", LevelTypes.ExperimentationLevel);
        vanillaMoonLevelTypePair.Add("Assurance", LevelTypes.AssuranceLevel);
        vanillaMoonLevelTypePair.Add("Vow", LevelTypes.VowLevel);
        vanillaMoonLevelTypePair.Add("Offense", LevelTypes.OffenseLevel);
        vanillaMoonLevelTypePair.Add("March", LevelTypes.MarchLevel);
        vanillaMoonLevelTypePair.Add("Adamance", LevelTypes.AdamanceLevel);
        vanillaMoonLevelTypePair.Add("Rend", LevelTypes.RendLevel);
        vanillaMoonLevelTypePair.Add("Dine", LevelTypes.DineLevel);
        vanillaMoonLevelTypePair.Add("Titan", LevelTypes.TitanLevel);
        vanillaMoonLevelTypePair.Add("Artifice", LevelTypes.ArtificeLevel);
        vanillaMoonLevelTypePair.Add("Embrion", LevelTypes.EmbrionLevel);

        foreach (var moon in moonSpawnList.Split(',').Select(_ => GetFormattedSting(_.Trim())))
        {
            if (vanillaMoonLevelTypePair.TryGetValue(moon, out LevelTypes levelTypes))
            {
                LethalLib.Modules.Items.RegisterScrap(item, iRarity, levelTypes);
                Plugin.logger.LogInfo($"Registered \"{item.itemName}\" scrap item on moon \"{moon}\" with {iRarity} rarity.");
                continue;
            }
        }
    }

    private static string GetFormattedSting(string value)
    {
        if (value.Length <= 1) return value.ToUpper();

        return value.Substring(0, 1).ToUpper() + value.Substring(1, value.Length - 1).ToLower();
    }
}
