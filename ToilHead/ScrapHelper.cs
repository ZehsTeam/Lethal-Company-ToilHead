using BepInEx.Bootstrap;

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

    public static void RegisterScrap(Item item, int iRarity, bool twoHanded, int carryWeight, int minValue, int maxValue)
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
            LethalLib.Modules.Items.RegisterScrap(item, iRarity, LethalLib.Modules.Levels.LevelTypes.All);

            Plugin.logger.LogInfo($"Registered \"{item.itemName}\" scrap item with {iRarity} rarity.");
        }
        catch (System.Exception e)
        {
            Plugin.logger.LogError($"Error: Failed to register \"{item.itemName}\" scrap item.\n\n{e}");
        }
    }
}
