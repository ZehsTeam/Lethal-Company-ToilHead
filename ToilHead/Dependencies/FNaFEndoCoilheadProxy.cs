using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Compatibility;

internal static class FNaFEndoCoilheadProxy
{
    public const string ModGUID = "FNaFEndoCoilhead";

    public static bool HasMod => Chainloader.PluginInfos.ContainsKey(ModGUID);
}
