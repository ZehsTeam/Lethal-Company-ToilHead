using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Compatibility;

internal class MonsterPlushiesCompat
{
    public const string ModGUID = "scin.monsterplushies";

    public static bool HasMod = false;

    public static void Initialize()
    {
        HasMod = Chainloader.PluginInfos.ContainsKey(ModGUID);
    }
}
