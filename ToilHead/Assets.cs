using com.github.zehsteam.ToilHead.MonoBehaviours;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

internal static class Assets
{
    public static readonly string AssetBundleFileName = "toilhead_assets";
    public static AssetBundle AssetBundle { get; private set; }
    public static bool IsLoaded { get; private set; }

    // Network Handler
    public static GameObject NetworkHandlerPrefab { get; private set; }

    // Turrets
    public static GameObject TurretPropPrefab { get; private set; }
    public static GameObject MinigunPropPrefab { get; private set; }

    // Turret-Head Controllers
    public static GameObject ToilPlayerControllerPrefab { get; private set; }
    public static GameObject SlayerPlayerControllerPrefab { get; private set; }
    public static GameObject ToiledDeadBodyControllerPrefab { get; private set; }
    public static GameObject SlayedDeadBodyControllerPrefab { get; private set; }
    public static GameObject ToilHeadControllerPrefab { get; private set; }
    public static GameObject ToilSlayerControllerPrefab { get; private set; }
    public static GameObject MantiToilControllerPrefab { get; private set; }
    public static GameObject MantiSlayerControllerPrefab { get; private set; }
    public static GameObject ToilMaskedControllerPrefab { get; private set; }
    public static GameObject SlayerMaskedControllerPrefab { get; private set; }

    // Plushies
    public static Item ToilHeadPlush { get; private set; }
    public static Item ToilSlayerPlush { get; private set; }

    public static void Load()
    {
        string pluginFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string assetBundlePath = Path.Combine(pluginFolder, AssetBundleFileName);

        if (!File.Exists(assetBundlePath))
        {
            Logger.LogFatal($"Failed to load assets. AssetBundle file could not be found at path \"{assetBundlePath}\". Make sure the \"{AssetBundleFileName}\" file is in the same folder as the mod's DLL file.");
            return;
        }

        AssetBundle = AssetBundle.LoadFromFile(assetBundlePath);

        if (AssetBundle == null)
        {
            Logger.LogFatal($"Failed to load assets. AssetBundle is null.");
            return;
        }

        OnAssetBundleLoaded(AssetBundle);

        IsLoaded = true;
    }

    private static void OnAssetBundleLoaded(AssetBundle assetBundle)
    {
        // Network Handler
        NetworkHandlerPrefab = assetBundle.LoadAsset<GameObject>("NetworkHandler");
        NetworkHandlerPrefab.AddComponent<PluginNetworkBehaviour>();

        // Turrets
        TurretPropPrefab = assetBundle.LoadAsset<GameObject>("TurretHeadTurretProp");
        MinigunPropPrefab = assetBundle.LoadAsset<GameObject>("MinigunTurretHeadTurretProp");

        // Turret-Head Controllers
        ToilPlayerControllerPrefab = assetBundle.LoadAsset<GameObject>("ToilPlayerController");
        SlayerPlayerControllerPrefab = assetBundle.LoadAsset<GameObject>("SlayerPlayerController");
        ToiledDeadBodyControllerPrefab = assetBundle.LoadAsset<GameObject>("ToiledDeadBodyController");
        SlayedDeadBodyControllerPrefab = assetBundle.LoadAsset<GameObject>("SlayedDeadBodyController");
        ToilHeadControllerPrefab = assetBundle.LoadAsset<GameObject>("ToilHeadController");
        ToilSlayerControllerPrefab = assetBundle.LoadAsset<GameObject>("ToilSlayerController");
        MantiToilControllerPrefab = assetBundle.LoadAsset<GameObject>("MantiToilController");
        MantiSlayerControllerPrefab = assetBundle.LoadAsset<GameObject>("MantiSlayerController");
        ToilMaskedControllerPrefab = assetBundle.LoadAsset<GameObject>("ToilMaskedController");
        SlayerMaskedControllerPrefab = assetBundle.LoadAsset<GameObject>("SlayerMaskedController");

        // Plushies
        ToilHeadPlush = assetBundle.LoadAsset<Item>("ToilHeadPlush");
        ToilSlayerPlush = assetBundle.LoadAsset<Item>("ToilSlayerPlush");
    }

    private static T LoadAsset<T>(string name, AssetBundle assetBundle) where T : Object
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Logger.LogError($"Failed to load asset of type \"{typeof(T).Name}\" from AssetBundle. Name is null or whitespace.");
            return null;
        }

        if (assetBundle == null)
        {
            Logger.LogError($"Failed to load asset of type \"{typeof(T).Name}\" with name \"{name}\" from AssetBundle. AssetBundle is null.");
            return null;
        }

        T asset = assetBundle.LoadAsset<T>(name);

        if (asset == null)
        {
            Logger.LogError($"Failed to load asset of type \"{typeof(T).Name}\" with name \"{name}\" from AssetBundle. No asset found with that type and name.");
            return null;
        }

        return asset;
    }

    private static bool TryLoadAsset<T>(string name, AssetBundle assetBundle, out T asset) where T : Object
    {
        asset = LoadAsset<T>(name, assetBundle);
        return asset != null;
    }
}
