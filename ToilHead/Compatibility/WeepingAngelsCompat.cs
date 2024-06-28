using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Compatibility;

internal class WeepingAngelsCompat
{
    public const string ModGUID = "raydenoir.WeepingAngel";

    public static bool HasMod => Chainloader.PluginInfos.ContainsKey(ModGUID);
}
