using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Compatibility;

internal class FNaFEndoCoilheadCompat
{
    public const string ModGUID = "FNaFEndoCoilhead";

    public static bool HasMod => Chainloader.PluginInfos.ContainsKey(ModGUID);
}
