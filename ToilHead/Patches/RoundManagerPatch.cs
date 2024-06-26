﻿using HarmonyLib;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatch
{
    [HarmonyPatch("LoadNewLevel")]
    [HarmonyPostfix]
    static void LoadNewLevelPatch()
    {
        // Call on Host/Server
        Plugin.Instance.OnNewLevelLoaded();
    }

    [HarmonyPatch("GenerateNewLevelClientRpc")]
    [HarmonyPrefix]
    static void GenerateNewLevelClientRpcPatch()
    {
        if (Plugin.IsHostOrServer) return;

        // Call on Client
        Plugin.Instance.OnNewLevelLoaded();
    }

    [HarmonyPatch("FinishGeneratingNewLevelClientRpc")]
    [HarmonyPostfix]
    static void FinishGeneratingNewLevelClientRpcPatch()
    {
        Plugin.Instance.OnNewLevelFinishedLoading();
    }
}
