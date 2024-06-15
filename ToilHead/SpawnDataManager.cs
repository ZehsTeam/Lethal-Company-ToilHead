namespace com.github.zehsteam.ToilHead;

internal class SpawnDataManager
{
    public static MoonSpawnDataList ToilHeadSpawnDataList { get; private set; }
    public static MoonSpawnDataList MantiToilSpawnDataList { get; private set; }
    public static MoonSpawnDataList ToilSlayerSpawnDataList { get; private set; }

    public static SpawnData ToilationToilHeadSpawnData { get; private set; }
    public static SpawnData ToilationMantiToilSpawnData { get; private set; }
    public static SpawnData ToilationToilSlayerSpawnData { get; private set; }

    public static void Initialize()
    {
        ToilHeadSpawnDataList = new MoonSpawnDataList(Plugin.ConfigManager.ToilHeadSpawnSettingsMoonList.Value, defaultSpawnData: new SpawnData(Plugin.ConfigManager.ToilHeadDefaultSpawnSettings.Value));
        MantiToilSpawnDataList = new MoonSpawnDataList(Plugin.ConfigManager.MantiToilSpawnSettingsMoonList.Value, defaultSpawnData: new SpawnData(Plugin.ConfigManager.MantiToilDefaultSpawnSettings.Value));
        ToilSlayerSpawnDataList = new MoonSpawnDataList(Plugin.ConfigManager.ToilSlayerSpawnSettingsMoonList.Value, defaultSpawnData: new SpawnData(Plugin.ConfigManager.ToilSlayerDefaultSpawnSettings.Value));

        ToilationToilHeadSpawnData = new SpawnData(Plugin.ConfigManager.ToilationToilHeadSpawnSettings.Value);
        ToilationMantiToilSpawnData = new SpawnData(Plugin.ConfigManager.ToilationMantiToilSpawnSettings.Value);
        ToilationToilSlayerSpawnData = new SpawnData(Plugin.ConfigManager.ToilationToilSlayerSpawnSettings.Value);
    }
    
    public static SpawnData GetToilHeadSpawnDataForCurrentMoon()
    {
        if (Utils.IsCurrentMoonToilation())
        {
            return ToilationToilHeadSpawnData;
        }

        return ToilHeadSpawnDataList.GetSpawnDataForCurrentMoon();
    }

    public static SpawnData GetMantiToilSpawnDataForCurrentMoon()
    {
        if (Utils.IsCurrentMoonToilation())
        {
            return ToilationMantiToilSpawnData;
        }

        return MantiToilSpawnDataList.GetSpawnDataForCurrentMoon();
    }

    public static SpawnData GetToilSlayerSpawnDataForCurrentMoon()
    {
        if (Utils.IsCurrentMoonToilation())
        {
            return ToilationToilSlayerSpawnData;
        }

        return ToilSlayerSpawnDataList.GetSpawnDataForCurrentMoon();
    }
}
