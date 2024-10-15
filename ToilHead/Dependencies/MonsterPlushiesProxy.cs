using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Compatibility;

internal static class MonsterPlushiesProxy
{
    public const string ModGUID = "scin.monsterplushies";

    public static bool HasMod => Chainloader.PluginInfos.ContainsKey(ModGUID);
}
