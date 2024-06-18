using com.github.zehsteam.ToilHead.MonoBehaviours;
using HarmonyLib;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.Patches;

[HarmonyPatch(typeof(Terminal))]
internal class TerminalPatch
{
    [HarmonyPatch("CallFunctionInAccessibleTerminalObject")]
    [HarmonyPostfix]
    static void CallFunctionInAccessibleTerminalObjectPatch(string word, ref bool ___broadcastedCodeThisFrame)
    {
        FollowTerminalAccessibleObjectBehaviour[] array = Object.FindObjectsByType<FollowTerminalAccessibleObjectBehaviour>(FindObjectsSortMode.None);

        foreach (var item in array)
        {
            if (item.objectCode != word) continue;

            Plugin.Instance.LogInfoExtended("Found accessible terminal object with corresponding string, calling function.");
            ___broadcastedCodeThisFrame = true;
            item.CallFunctionFromTerminal();
        }
    }
}
