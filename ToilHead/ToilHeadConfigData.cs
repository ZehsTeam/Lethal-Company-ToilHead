using System.Linq;

namespace com.github.zehsteam.ToilHead;

public class ToilHeadConfigData
{
    public int maxSpawnCount = 1;
    public int spawnChance = 30;

    public ToilHeadConfigData(string configString)
    {
        ParseConfigString(configString);
    }

    private void ParseConfigString(string configString)
    {
        string[] words = configString.Split(',').Select(_ => _.Trim()).ToArray();

        if (words.Length < 2)
        {
            Plugin.logger.LogError("ParseConfigString Error: Invalid config string length.");
            return;
        }

        // spawnCount
        if (int.TryParse(words[0], out int parsedInt))
        {
            maxSpawnCount = parsedInt;
        }
        else
        {
            Plugin.logger.LogError("ParseConfigString Error: Failed to parse int (maxSpawnCount) in config string.");
        }

        // spawnNearPlayer
        if (int.TryParse(words[1], out parsedInt))
        {
            spawnChance = parsedInt;
        }
        else
        {
            Plugin.logger.LogError("ParseConfigString Error: Failed to parse bool (spawnChance) in config string.");
        }
    }
}
