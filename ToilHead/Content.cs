using com.github.zehsteam.ToilHead.MonoBehaviours;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

internal class Content
{
    // NetworkHandler
    public static GameObject NetworkHandlerPrefab;

    // Turrets
    public static GameObject TurretPrefab;
    public static GameObject TurretPropPrefab;
    public static GameObject MinigunTurretPrefab;
    public static GameObject MinigunTurretPropPrefab;

    // Plushies
    public static Item ToilHeadPlush;
    public static Item ToilSlayerPlush;

    public static void Load()
    {
        LoadAssetsFromAssetBundle();
    }

    private static void LoadAssetsFromAssetBundle()
    {
        try
        {
            var dllFolderPath = System.IO.Path.GetDirectoryName(Plugin.Instance.Info.Location);
            var assetBundleFilePath = System.IO.Path.Combine(dllFolderPath, "toilhead_assets");
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundleFilePath);

            // NetworkHandler
            NetworkHandlerPrefab = assetBundle.LoadAsset<GameObject>("NetworkHandler");
            NetworkHandlerPrefab.AddComponent<PluginNetworkBehaviour>();

            // Turrets
            TurretPrefab = assetBundle.LoadAsset<GameObject>("ToilHeadTurretContainer");
            TurretPropPrefab = assetBundle.LoadAsset<GameObject>("ToilHeadTurretProp");
            MinigunTurretPrefab = assetBundle.LoadAsset<GameObject>("MinigunTurretContainer");
            MinigunTurretPropPrefab = assetBundle.LoadAsset<GameObject>("MinigunTurretProp");

            // Plushies
            ToilHeadPlush = assetBundle.LoadAsset<Item>("ToilHeadPlush");
            ToilSlayerPlush = assetBundle.LoadAsset<Item>("ToilSlayerPlush");

            Plugin.logger.LogInfo("Successfully loaded assets from AssetBundle!");
        }
        catch (System.Exception e)
        {
            Plugin.logger.LogError($"Error: failed to load assets from AssetBundle.\n\n{e}");
        }
    }
}
