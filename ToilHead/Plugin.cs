using BepInEx;
using com.github.zehsteam.ToilHead.Dependencies;
using com.github.zehsteam.ToilHead.Dependencies.LethalConfigMod;
using com.github.zehsteam.ToilHead.Helpers;
using com.github.zehsteam.ToilHead.Managers;
using com.github.zehsteam.ToilHead.Patches;
using HarmonyLib;

namespace com.github.zehsteam.ToilHead;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(LethalConfigProxy.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(LethalLibProxy.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(MonsterPlushiesProxy.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
internal class Plugin : BaseUnityPlugin
{
    private readonly Harmony _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static Plugin Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        ToilHead.Logger.Initialize(BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID));
        ToilHead.Logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        ConfigManager.Initialize(Config);

        _harmony.PatchAll(typeof(GameNetworkManagerPatch));
        _harmony.PatchAll(typeof(StartOfRoundPatch));
        _harmony.PatchAll(typeof(RoundManagerPatch));
        _harmony.PatchAll(typeof(TerminalPatch));
        _harmony.PatchAll(typeof(PlayerControllerBPatch));
        _harmony.PatchAll(typeof(RagdollGrabbableObjectPatch));
        _harmony.PatchAll(typeof(EnemyAIPatch));
        _harmony.PatchAll(typeof(SpringManAIPatch));
        _harmony.PatchAll(typeof(MaskedPlayerEnemyPatch));
        _harmony.PatchAll(typeof(TurretPatch));

        Assets.Load();

        TurretHeadManager.Initialize();

        RegisterScrapItems();

        NetworkUtils.NetcodePatcherAwake();
    }

    public void OnLocalDisconnect()
    {
        ToilHead.Logger.LogInfo($"Local player disconnected. Removing hostConfigData.");
        ConfigManager.SetHostConfigData(null);

        TurretHeadManager.Reset();
    }

    public void OnNewLevelLoaded()
    {
        Asteroid13Proxy.SpawnSecrets();
    }

    public void OnNewLevelFinishedLoading()
    {
        TurretHeadManager.TrySetPlayerTurretHeadsOnServer();
    }

    public void OnShipHasLeft()
    {
        TurretHeadManager.Reset();
    }

    private void RegisterScrapItems()
    {
        if (!LethalLibProxy.IsInstalled) return;
        if (!MonsterPlushiesProxy.IsInstalled) return;

        try
        {
            ScrapHelper.RegisterScrap(Content.ToilHeadPlush, ConfigManager.ToilHeadPlushieSpawnWeight.Value, ConfigManager.ToilHeadPlushieSpawnAllMoons.Value, ConfigManager.ToilHeadPlushieMoonSpawnList.Value, twoHanded: false, ConfigManager.ToilHeadPlushieCarryWeight.Value, ConfigManager.ToilHeadPlushieMinValue.Value, ConfigManager.ToilHeadPlushieMaxValue.Value);
            ScrapHelper.RegisterScrap(Content.ToilSlayerPlush, ConfigManager.ToilSlayerPlushieSpawnWeight.Value, ConfigManager.ToilSlayerPlushieSpawnAllMoons.Value, ConfigManager.ToilSlayerPlushieMoonSpawnList.Value, twoHanded: false, ConfigManager.ToilSlayerPlushieCarryWeight.Value, ConfigManager.ToilSlayerPlushieMinValue.Value, ConfigManager.ToilSlayerPlushieMaxValue.Value);
        }
        catch (System.Exception e)
        {
            ToilHead.Logger.LogWarning($"Warning: Failed to register scrap items.\n\n{e}");
        }
    }
}
