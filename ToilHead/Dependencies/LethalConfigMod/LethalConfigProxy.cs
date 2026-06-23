using BepInEx.Bootstrap;
using BepInEx.Configuration;
using com.github.zehsteam.ToilHead.Extensions;
using HarmonyLib;
using LethalConfig;
using LethalConfig.ConfigItems;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace com.github.zehsteam.ToilHead.Dependencies.LethalConfigMod;

internal static class LethalConfigProxy
{
    public const string PLUGIN_GUID = LethalConfig.PluginInfo.Guid;
    public static bool IsInstalled => Chainloader.PluginInfos.ContainsKey(PLUGIN_GUID);

    #region Public Methods
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static void SkipAutoGen()
    {
        LethalConfigManager.SkipAutoGen();
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static object AddConfig<T>(ConfigEntry<T> configEntry, bool requiresRestart = false)
    {
        if (typeof(T).IsEnum)
        {
            MethodInfo method = AccessTools.Method(typeof(LethalConfigProxy), nameof(AddEnumDropdown));
            MethodInfo genericMethod = method.MakeGenericMethod(typeof(T));
            return genericMethod.Invoke(null, [configEntry, requiresRestart]) as BaseConfigItem;
        }

        AcceptableValueBase acceptableValues = configEntry.Description.AcceptableValues;

        if (acceptableValues != null)
        {
            // Check if it is an AcceptableValueRange for either float or int
            if (acceptableValues is AcceptableValueRange<float> || acceptableValues is AcceptableValueRange<int>)
            {
                return AddConfigSlider(configEntry, requiresRestart);
            }
            // Check if it is an AcceptableValueList for string
            else if (acceptableValues is AcceptableValueList<string>)
            {
                return AddConfigDropdown(configEntry, requiresRestart);
            }
        }

        if (configEntry is ConfigEntry<string> hexColorConfigEntry && IsConfigEntryForHexColor(hexColorConfigEntry))
        {
            return AddConfigItem(new HexColorInputFieldConfigItem(hexColorConfigEntry, requiresRestart));
        }

        // Use pattern matching or type checks to determine which type-specific ConfigItem to create
        return configEntry switch
        {
            ConfigEntry<int> intEntry => AddConfigItem(new IntInputFieldConfigItem(intEntry, requiresRestart)),
            ConfigEntry<float> floatEntry => AddConfigItem(new FloatInputFieldConfigItem(floatEntry, requiresRestart)),
            ConfigEntry<bool> boolEntry => AddConfigItem(new BoolCheckBoxConfigItem(boolEntry, requiresRestart)),
            ConfigEntry<string> strEntry => AddConfigItem(new TextInputFieldConfigItem(strEntry, requiresRestart)),
            _ => throw new NotSupportedException($"Unsupported type: {typeof(T)}"),
        };
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static object AddButton(string section, string name, string buttonText, string description, Action callback)
    {
        BaseConfigItem configItem = new GenericButtonConfigItem(section, name, description, buttonText, () => callback?.Invoke());
        return AddConfigItem(configItem);
    }
    #endregion

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static BaseConfigItem AddConfigSlider<T>(ConfigEntry<T> configEntry, bool requiresRestart = false)
    {
        // Handle sliders for float and int specifically
        return configEntry switch
        {
            ConfigEntry<float> floatEntry => AddConfigItem(new FloatSliderConfigItem(floatEntry, requiresRestart)),
            ConfigEntry<int> intEntry => AddConfigItem(new IntSliderConfigItem(intEntry, requiresRestart)),
            _ => throw new NotSupportedException($"Slider not supported for type: {typeof(T)}"),
        };
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static BaseConfigItem AddConfigDropdown<T>(ConfigEntry<T> configEntry, bool requiresRestart = false)
    {
        // Handle dropdown for string or enum-like entries
        return configEntry switch
        {
            ConfigEntry<string> stringEntry => AddConfigItem(new TextDropDownConfigItem(stringEntry, requiresRestart)),
            _ => throw new NotSupportedException($"Dropdown not supported for type: {typeof(T)}"),
        };
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static BaseConfigItem AddEnumDropdown<T>(ConfigEntry<T> configEntry, bool requiresRestart = false) where T : Enum
    {
        return AddConfigItem(new EnumDropDownConfigItem<T>(configEntry, requiresRestart));
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static bool IsConfigEntryForHexColor(ConfigEntry<string> configEntry)
    {
        if (configEntry.DefaultValue is not string value)
            return false;

        return value.IsHexColor();
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static BaseConfigItem AddConfigItem(BaseConfigItem configItem)
    {
        LethalConfigManager.AddConfigItem(configItem);
        return configItem;
    }
}
