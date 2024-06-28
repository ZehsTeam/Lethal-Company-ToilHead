using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Compatibility;

internal class SCP173CoilheadSFXCompat
{
    public const string ModGUID = "raydenoir.SCP_173";

    public static bool HasMod => Chainloader.PluginInfos.ContainsKey(ModGUID);
}
