using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Dependencies;

internal static class LethalLibProxy
{
    public const string PLUGIN_GUID = LethalLib.Plugin.ModGUID;
    public static bool IsInstalled => Chainloader.PluginInfos.ContainsKey(PLUGIN_GUID);
}
