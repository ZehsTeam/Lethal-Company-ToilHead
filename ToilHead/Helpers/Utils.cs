using BepInEx;
using BepInEx.Configuration;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using System.Collections;
using System;

namespace com.github.zehsteam.ToilHead.Helpers;

internal static class Utils
{
    public static string GetPluginDirectoryPath()
    {
        return Path.GetDirectoryName(Plugin.Instance.Info.Location);
    }

    public static string GetConfigDirectoryPath()
    {
        return Paths.ConfigPath;
    }

    public static string GetPluginPersistentDataPath()
    {
        return Path.Combine(Application.persistentDataPath, MyPluginInfo.PLUGIN_NAME);
    }

    public static ConfigFile CreateConfigFile(BaseUnityPlugin plugin, string path, string name = null, bool saveOnInit = false)
    {
        BepInPlugin metadata = MetadataHelper.GetMetadata(plugin);
        name ??= metadata.GUID;
        name += ".cfg";
        return new ConfigFile(Path.Combine(path, name), saveOnInit, metadata);
    }

    public static ConfigFile CreateLocalConfigFile(BaseUnityPlugin plugin, string name = null, bool saveOnInit = false)
    {
        return CreateConfigFile(plugin, GetConfigDirectoryPath(), name, saveOnInit);
    }

    public static ConfigFile CreateGlobalConfigFile(BaseUnityPlugin plugin, string name = null, bool saveOnInit = false)
    {
        string path = GetPluginPersistentDataPath();
        name ??= "global";
        return CreateConfigFile(plugin, path, name, saveOnInit);
    }

    public static bool RollPercentChance(float percent)
    {
        if (percent <= 0f) return false;
        if (percent >= 100f) return true;
        return Random.value * 100f <= percent;
    }



    public static void DisableColliders(GameObject gameObject, bool keepScanNodeEnabled = false)
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

        foreach (var collider in colliders)
        {
            if (keepScanNodeEnabled && collider.gameObject.name == "ScanNode")
            {
                continue;
            }

            collider.enabled = false;
        }
    }

    public static void DisableRenderers(GameObject gameObject)
    {
        foreach (var renderer in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }

        foreach (var renderer in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.enabled = false;
        }
    }

    public static IEnumerator WaitUntil(Func<bool> predicate, float maxDuration = 5f, int iterationsPerSecond = 10)
    {
        float timer = 0f;

        float timePerIteration = 1f / iterationsPerSecond;

        while (timer < maxDuration)
        {
            if (predicate())
            {
                break;
            }

            yield return new WaitForSeconds(timePerIteration);
            timer += Time.deltaTime;
        }
    }

    public static bool IsCurrentMoonToilation()
    {
        if (StartOfRound.Instance == null) return false;

        string planetName = StartOfRound.Instance.currentLevel.PlanetName;

        if (planetName.Equals("69 Toilation", System.StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (planetName.Contains("Toilation", System.StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }
}
