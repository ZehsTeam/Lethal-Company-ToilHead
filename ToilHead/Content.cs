﻿using com.github.zehsteam.ToilHead.MonoBehaviours;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

internal class Content
{
    // NetworkHandler
    public static GameObject networkHandlerPrefab;

    // Turrets
    public static GameObject turretPrefab;
    public static GameObject turretPropPrefab;

    // Toil-Head Plushie
    public static Item toilHeadPlush;

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
            networkHandlerPrefab = assetBundle.LoadAsset<GameObject>("NetworkHandler");
            networkHandlerPrefab.AddComponent<PluginNetworkBehaviour>();

            // Turrets
            turretPrefab = assetBundle.LoadAsset<GameObject>("ToilHeadTurretContainer");
            turretPropPrefab = assetBundle.LoadAsset<GameObject>("ToilHeadTurretProp");

            // Toil-Head Plushie
            toilHeadPlush = assetBundle.LoadAsset<Item>("ToilHeadPlush");

            Plugin.logger.LogInfo("Successfully loaded assets from AssetBundle!");
        }
        catch (System.Exception e)
        {
            Plugin.logger.LogError($"Error: failed to load assets from AssetBundle.\n\n{e}");
        }
    }
}
