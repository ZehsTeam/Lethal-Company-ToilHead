using GameNetcodeStuff;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

internal class Utils
{
    public static void DisableColliders(GameObject gameObject, bool keepScanNodeEnabled = false)
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

        foreach (var collider in colliders)
        {
            if (keepScanNodeEnabled && collider.gameObject.name == "ScanNode")
            {
                continue;
            }

            collider.enabled = false;
        }
    }

    public static int GetLocalPlayerClientId()
    {
        return (int)GameNetworkManager.Instance.localPlayerController.playerClientId;
    }

    public static PlayerControllerB GetPlayerScript(int playerWhoHit)
    {
        try
        {
            return StartOfRound.Instance.allPlayerScripts[playerWhoHit];
        }
        catch { }

        return null;
    }
}
