using HarmonyLib;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal static class RoundManagerPatch
{
    [HarmonyPatch(nameof(RoundManager.LoadNewLevel))]
    [HarmonyPostfix]
    private static void LoadNewLevelPatch()
    {
        // Call on Host/Server
        Plugin.Instance.OnNewLevelLoaded();
    }

    [HarmonyPatch(nameof(RoundManager.GenerateNewLevelClientRpc))]
    [HarmonyPrefix]
    private static void GenerateNewLevelClientRpcPatch()
    {
        if (NetworkUtils.IsServer) return;

        // Call on Client
        Plugin.Instance.OnNewLevelLoaded();
    }

    [HarmonyPatch(nameof(RoundManager.FinishGeneratingNewLevelClientRpc))]
    [HarmonyPostfix]
    private static void FinishGeneratingNewLevelClientRpcPatch()
    {
        Plugin.Instance.OnNewLevelFinishedLoading();
    }
}
