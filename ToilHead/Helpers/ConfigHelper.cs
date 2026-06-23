using BepInEx.Configuration;
using com.github.zehsteam.ToilHead.Dependencies.LethalConfigMod;
using com.github.zehsteam.ToilHead.Extensions;
using com.github.zehsteam.ToilHead.Managers;
using System;

namespace com.github.zehsteam.ToilHead.Helpers;

internal static class ConfigHelper
{
    #region LethalConfig
    public static void SkipAutoGen()
    {
        if (LethalConfigProxy.IsInstalled)
        {
            LethalConfigProxy.SkipAutoGen();
        }
    }

    public static void AddButton(string section, string name, string buttonText, string description, Action callback)
    {
        if (LethalConfigProxy.IsInstalled)
        {
            LethalConfigProxy.AddButton(section, name, buttonText, description, callback);
        }
    }
    #endregion

    public static ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, string description, bool requiresRestart = false, AcceptableValueBase acceptableValues = null, Action<T> settingChanged = null, ConfigFile configFile = null)
    {
        configFile ??= ConfigManager.ConfigFile;

        var configEntry = configFile.Bind(section, key, defaultValue, description, acceptableValues);

        if (settingChanged != null)
        {
            configEntry.SettingChanged += (_, _) => settingChanged?.Invoke(configEntry.Value);
        }

        if (LethalConfigProxy.IsInstalled)
        {
            LethalConfigProxy.AddConfig(configEntry, requiresRestart);
        }

        return configEntry;
    }
}
