using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Dependencies;

internal static class MonsterPlushiesProxy
{
    public const string PLUGIN_GUID = "scin.monsterplushies";
    public static bool IsInstalled => Chainloader.PluginInfos.ContainsKey(PLUGIN_GUID);
}
