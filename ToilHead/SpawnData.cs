using System.Collections.Generic;
using System.Linq;

namespace com.github.zehsteam.ToilHead;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class SpawnData
{
    public int MaxSpawnCount;
    public int SpawnChance;

    public SpawnData(string value)
    {
        ParseValue(value);
    }

    protected virtual void ParseValue(string value)
    {
        string[] items = value.Split(':').Select(_ => _.Trim()).ToArray();

        int length = 2;

        if (items.Length != length)
        {
            Plugin.logger.LogError($"ParseValue Error: Invalid item length for string \"{value}\". Length is {items.Length} but should have been {length}.");
            return;
        }

        TryParseItem(items[0], out MaxSpawnCount);
        TryParseItem(items[1], out SpawnChance);

        Plugin.Instance.LogInfoExtended($"Parsed SpawnData value string. MaxSpawnCount: {MaxSpawnCount}, SpawnChance: {SpawnChance}");
    }

    protected bool TryParseItem(string value, out int parsedInt)
    {
        if (int.TryParse(value, out parsedInt))
        {
            return true;
        }
        else
        {
            Plugin.logger.LogError($"TryParseItem Error: Failed to parse int from string \"{value}\".");
        }

        return false;
    }
}

public class MoonSpawnData : SpawnData
{
    public string PlanetName;

    public MoonSpawnData(string value) : base(value) { }

    protected override void ParseValue(string value)
    {
        string[] items = value.Split(':').Select(_ => _.Trim()).ToArray();

        int length = 3;

        if (items.Length != length)
        {
            Plugin.logger.LogError($"ParseValue Error: Invalid item length for string \"{value}\". Length is {items.Length} but should have been {length}.");
            return;
        }

        PlanetName = items[0];
        TryParseItem(items[1], out MaxSpawnCount);
        TryParseItem(items[2], out SpawnChance);

        Plugin.Instance.LogInfoExtended($"Parsed MoonSpawnData value string. PlanetName: \"{PlanetName}\", MaxSpawnCount: {MaxSpawnCount}, SpawnChance: {SpawnChance}");
    }
}

public class MoonSpawnDataList
{
    public List<MoonSpawnData> List = [];
    public SpawnData DefaultSpawnData;
    
    public MoonSpawnDataList(string value)
    {
        ParseValue(value);
    }
    
    public MoonSpawnDataList(string value, SpawnData defaultSpawnData)
    {
        ParseValue(value);
        DefaultSpawnData = defaultSpawnData;
    }

    private void ParseValue(string value)
    {
        string[] items = value.Split(',').Select(_ => _.Trim()).ToArray();

        List = [];

        foreach (var item in items)
        {
            List.Add(new MoonSpawnData(item));
        }
    }
    
    public SpawnData GetSpawnDataForCurrentMoon()
    {
        if (StartOfRound.Instance == null) return DefaultSpawnData;

        return GetSpawnDataForMoon(StartOfRound.Instance.currentLevel.PlanetName);
    }

    public SpawnData GetSpawnDataForMoon(string planetName)
    {
        foreach (var item in List)
        {
            if (item.PlanetName == planetName) return item;
        }

        return DefaultSpawnData;
    }
}
