using GameNetcodeStuff;

namespace com.github.zehsteam.ToilHead;

internal class PlayerUtils
{
    #region Player "Takerst"
    public static bool HasPlayerTakerst()
    {
        return HasPlayerTakerst(out PlayerControllerB _);
    }

    public static bool HasPlayerTakerst(out PlayerControllerB targetPlayerScript)
    {
        targetPlayerScript = null;

        foreach (var playerScript in StartOfRound.Instance.allPlayerScripts)
        {
            if (IsPlayerTakerst(playerScript))
            {
                targetPlayerScript = playerScript;
                return true;
            }
        }

        return false;
    }

    public static bool IsLocalPlayerTakerst()
    {
        return IsPlayerTakerst(GetLocalPlayerScript());
    }

    public static bool IsPlayerTakerst(PlayerControllerB playerScript)
    {
        if (playerScript.playerUsername == "Takerst") return true;
        if (playerScript.playerSteamId == 76561197980238122) return true;

        return false;
    }
    #endregion

    public static int GetLocalPlayerId()
    {
        return (int)GetLocalPlayerScript().playerClientId;
    }

    public static PlayerControllerB GetPlayerScript(int playerId)
    {
        try
        {
            return StartOfRound.Instance.allPlayerScripts[playerId];
        }
        catch
        {
            return null;
        }
    }

    public static PlayerControllerB GetLocalPlayerScript()
    {
        return GameNetworkManager.Instance.localPlayerController;
    }
}
