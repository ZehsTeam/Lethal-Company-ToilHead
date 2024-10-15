using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Compatibility;

internal static class SCP173CoilheadSFXProxy
{
    public const string ModGUID = "raydenoir.SCP_173";

    public static bool HasMod => Chainloader.PluginInfos.ContainsKey(ModGUID);
}
