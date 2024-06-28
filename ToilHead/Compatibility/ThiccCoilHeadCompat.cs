using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Compatibility;

internal class ThiccCoilHeadCompat
{
    public const string ModGUID = "ThiccCoilHead";

    public static bool HasMod => Chainloader.PluginInfos.ContainsKey(ModGUID);
}
