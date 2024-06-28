using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Compatibility;

internal class ARatherSillyCoilHeadCompat
{
    public const string ModGUID = "COREsEND.A_Rather_Silly_Coil_Head";

    public static bool HasMod => Chainloader.PluginInfos.ContainsKey(ModGUID);
}
