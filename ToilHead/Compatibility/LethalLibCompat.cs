﻿using BepInEx.Bootstrap;

namespace com.github.zehsteam.ToilHead.Compatibility;

internal class LethalLibCompat
{
    public const string ModGUID = LethalLib.Plugin.ModGUID;

    public static bool HasMod => Chainloader.PluginInfos.ContainsKey(ModGUID);
}
