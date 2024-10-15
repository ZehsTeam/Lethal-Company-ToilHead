using com.github.zehsteam.ToilHead.MonoBehaviours;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

internal static class Content
{
    // Network Handler
    public static GameObject NetworkHandlerPrefab;

    // Turrets
    public static GameObject TurretPropPrefab;
    public static GameObject MinigunPropPrefab;

    // Turret-Head Controllers
    public static GameObject ToilPlayerControllerPrefab;
    public static GameObject SlayerPlayerControllerPrefab;
    public static GameObject ToiledDeadBodyControllerPrefab;
    public static GameObject SlayedDeadBodyControllerPrefab;
    public static GameObject ToilHeadControllerPrefab;
    public static GameObject ToilSlayerControllerPrefab;
    public static GameObject MantiToilControllerPrefab;
    public static GameObject MantiSlayerControllerPrefab;
    public static GameObject ToilMaskedControllerPrefab;
    public static GameObject SlayerMaskedControllerPrefab;

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

            Plugin.logger.LogInfo("Successfully loaded assets from AssetBundle!");
        }
        catch (System.Exception e)
        {
            Plugin.logger.LogError($"Error: Failed to load assets from AssetBundle.\n\n{e}");
        }
    }
}
