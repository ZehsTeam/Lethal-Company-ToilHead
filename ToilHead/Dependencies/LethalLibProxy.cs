using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Compatibility;

internal static class LethalLibProxy
{
    public const string ModGUID = LethalLib.Plugin.ModGUID;

    public static bool HasMod => Chainloader.PluginInfos.ContainsKey(ModGUID);
}
