using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.ToilHead.Compatibility;
using com.github.zehsteam.ToilHead.Patches;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(LethalLibProxy.ModGUID, BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(MonsterPlushiesProxy.ModGUID, BepInDependency.DependencyFlags.SoftDependency)]
internal class Plugin : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static Plugin Instance;
    internal static ManualLogSource logger;

    internal static SyncedConfigManager ConfigManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        logger = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        harmony.PatchAll(typeof(GameNetworkManagerPatch));
        harmony.PatchAll(typeof(StartOfRoundPatch));
        harmony.PatchAll(typeof(RoundManagerPatch));
        harmony.PatchAll(typeof(TerminalPatch));
        harmony.PatchAll(typeof(PlayerControllerBPatch));
        harmony.PatchAll(typeof(RagdollGrabbableObjectPatch));
        harmony.PatchAll(typeof(EnemyAIPatch));
        harmony.PatchAll(typeof(SpringManAIPatch));
        harmony.PatchAll(typeof(MaskedPlayerEnemyPatch));
        harmony.PatchAll(typeof(TurretPatch));

        ConfigManager = new SyncedConfigManager();

        Content.Load();
        TurretHeadManager.Initialize();

        RegisterScrapItems();
        NetcodePatcherAwake();
    }

    private void NetcodePatcherAwake()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();

        foreach (var type in types)
        {
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);

                if (attributes.Length > 0)
                {
                    method.Invoke(null, null);
                }
            }
        }
    }

    public void OnLocalDisconnect()
    {
        logger.LogInfo($"Local player disconnected. Removing hostConfigData.");
        ConfigManager.SetHostConfigData(null);

        TurretHeadManager.Reset();
    }

    public void OnNewLevelLoaded()
    {
        Secret.SpawnSecrets();
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
        if (!LethalLibProxy.HasMod) return;
        if (!MonsterPlushiesProxy.HasMod) return;

        try
        {
            ScrapHelper.RegisterScrap(Content.ToilHeadPlush, ConfigManager.ToilHeadPlushieSpawnWeight.Value, ConfigManager.ToilHeadPlushieSpawnAllMoons.Value, ConfigManager.ToilHeadPlushieMoonSpawnList.Value, twoHanded: false, ConfigManager.ToilHeadPlushieCarryWeight.Value, ConfigManager.ToilHeadPlushieMinValue.Value, ConfigManager.ToilHeadPlushieMaxValue.Value);
            ScrapHelper.RegisterScrap(Content.ToilSlayerPlush, ConfigManager.ToilSlayerPlushieSpawnWeight.Value, ConfigManager.ToilSlayerPlushieSpawnAllMoons.Value, ConfigManager.ToilSlayerPlushieMoonSpawnList.Value, twoHanded: false, ConfigManager.ToilSlayerPlushieCarryWeight.Value, ConfigManager.ToilSlayerPlushieMinValue.Value, ConfigManager.ToilSlayerPlushieMaxValue.Value);
        }
        catch (System.Exception e)
        {
            logger.LogWarning($"Warning: Failed to register scrap items.\n\n{e}");
        }
    }

    public void LogInfoExtended(object data)
    {
        if (ConfigManager.ExtendedLogging.Value)
        {
            logger.LogInfo(data);
        }
    }

    public void LogWarningExtended(object data)
    {
        if (ConfigManager.ExtendedLogging.Value)
        {
            logger.LogWarning(data);
        }
    }

    public void LogErrorExtended(object data)
    {
        if (ConfigManager.ExtendedLogging.Value)
        {
            logger.LogError(data);
        }
    }
}
