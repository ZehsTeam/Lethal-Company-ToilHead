namespace com.github.zehsteam.ToilHead;

internal class SpawnDataManager
{
    public static MoonSpawnDataList ToilHeadSpawnDataList { get; private set; }
    public static MoonSpawnDataList MantiToilSpawnDataList { get; private set; }
    public static MoonSpawnDataList ToilSlayerSpawnDataList { get; private set; }
    public static MoonSpawnDataList MantiSlayerSpawnDataList { get; private set; }
    public static MoonSpawnDataList ToilPlayerSpawnDataList { get; private set; }

    public static SpawnData ToilationToilHeadSpawnData { get; private set; }
    public static SpawnData ToilationMantiToilSpawnData { get; private set; }
    public static SpawnData ToilationToilSlayerSpawnData { get; private set; }
    public static SpawnData ToilationMantiSlayerSpawnData { get; private set; }
    public static SpawnData ToilationToilPlayerSpawnData { get; private set; }

    public static void Initialize()
    {
        ToilHeadSpawnDataList = new MoonSpawnDataList(Plugin.ConfigManager.ToilHeadSpawnSettingsMoonList.Value, defaultSpawnData: new SpawnData(Plugin.ConfigManager.ToilHeadDefaultSpawnSettings.Value));
        MantiToilSpawnDataList = new MoonSpawnDataList(Plugin.ConfigManager.MantiToilSpawnSettingsMoonList.Value, defaultSpawnData: new SpawnData(Plugin.ConfigManager.MantiToilDefaultSpawnSettings.Value));
        ToilSlayerSpawnDataList = new MoonSpawnDataList(Plugin.ConfigManager.ToilSlayerSpawnSettingsMoonList.Value, defaultSpawnData: new SpawnData(Plugin.ConfigManager.ToilSlayerDefaultSpawnSettings.Value));
        MantiSlayerSpawnDataList = new MoonSpawnDataList(Plugin.ConfigManager.MantiSlayerSpawnSettingsMoonList.Value, defaultSpawnData: new SpawnData(Plugin.ConfigManager.MantiSlayerDefaultSpawnSettings.Value));
        ToilPlayerSpawnDataList = new MoonSpawnDataList(Plugin.ConfigManager.ToilPlayerSpawnSettingsMoonList.Value, defaultSpawnData: new SpawnData(Plugin.ConfigManager.ToilPlayerDefaultSpawnSettings.Value));

        ToilationToilHeadSpawnData = new SpawnData(Plugin.ConfigManager.ToilationToilHeadSpawnSettings.Value);
        ToilationMantiToilSpawnData = new SpawnData(Plugin.ConfigManager.ToilationMantiToilSpawnSettings.Value);
        ToilationToilSlayerSpawnData = new SpawnData(Plugin.ConfigManager.ToilationToilSlayerSpawnSettings.Value);
        ToilationMantiSlayerSpawnData = new SpawnData(Plugin.ConfigManager.ToilationMantiSlayerSpawnSettings.Value);
        ToilationToilPlayerSpawnData = new SpawnData(Plugin.ConfigManager.ToilationToilPlayerSpawnSettings.Value);
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

    public static SpawnData GetMantiSlayerSpawnDataForCurrentMoon()
    {
        if (Utils.IsCurrentMoonToilation())
        {
            return ToilationMantiSlayerSpawnData;
        }

        return MantiSlayerSpawnDataList.GetSpawnDataForCurrentMoon();
    }

    public static SpawnData GetToilPlayerSpawnDataForCurrentMoon()
    {
        if (Utils.IsCurrentMoonToilation())
        {
            return ToilationToilPlayerSpawnData;
        }

        return ToilPlayerSpawnDataList.GetSpawnDataForCurrentMoon();
    }
}
