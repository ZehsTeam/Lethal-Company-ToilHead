using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(GameNetworkManager))]
internal class GameNetworkManagerPatch
{
    public static GameObject networkPrefab;

    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch()
    {
        LoadAssetsFromAssetBundle();
    }

    private static void LoadAssetsFromAssetBundle()
    {
        if (networkPrefab != null) return;

        var dllFolderPath = System.IO.Path.GetDirectoryName(ToilHeadBase.Instance.Info.Location);
        var assetBundleFilePath = System.IO.Path.Combine(dllFolderPath, "toilhead_assets");
        AssetBundle MainAssetBundle = AssetBundle.LoadFromFile(assetBundleFilePath);

        networkPrefab = MainAssetBundle.LoadAsset<GameObject>("NetworkHandler");
        networkPrefab.AddComponent<PluginNetworkBehaviour>();

        NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);
    }
}
