using BepInEx.Configuration;
using System;

namespace com.github.zehsteam.ToilHead;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ExtendedConfigEntry<T>
{
    public ConfigEntry<T> ConfigEntry;
    public Func<T> GetValue;
    public Action<T> SetValue;

    public T DefaultValue => (T)ConfigEntry.DefaultValue;
    public T Value { get { return GetValue(); } set { SetValue(value); } }

    public bool UseEnableConfiguration = true;

    public ExtendedConfigEntry(string section, string key, T defaultValue, string description, bool useEnableConfiguration = true)
    {
        ConfigEntry = Plugin.Instance.Config.Bind(section, key, defaultValue, description);
        UseEnableConfiguration = useEnableConfiguration;
        Initialize();
    }

    public ExtendedConfigEntry(string section, string key, T defaultValue, ConfigDescription configDescription = null, bool useEnableConfiguration = true)
    {
        ConfigEntry = Plugin.Instance.Config.Bind(section, key, defaultValue, configDescription);
        UseEnableConfiguration = useEnableConfiguration;
        Initialize();
    }

    private void Initialize()
    {
        if (GetValue == null)
        {
            GetValue = () =>
            {
                if (UseEnableConfiguration && !Plugin.ConfigManager.EnableConfiguration.Value)
                {
                    return DefaultValue;
                }

                return ConfigEntry.Value;
            };
        }

        if (SetValue == null)
        {
            SetValue = (T value) =>
            {
                ConfigEntry.Value = value;
            };
        }
    }

    public void ResetToDefault()
    {
        ConfigEntry.Value = (T)ConfigEntry.DefaultValue;
    }
}
