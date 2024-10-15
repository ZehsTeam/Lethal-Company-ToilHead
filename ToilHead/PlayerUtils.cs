using GameNetcodeStuff;

namespace com.github.zehsteam.ToilHead;

internal static class PlayerUtils
{
    public static bool IsLocalPlayer(PlayerControllerB playerScript)
    {
        return playerScript == GetLocalPlayerScript();
    }

    public static int GetPlayerId(PlayerControllerB playerScript)
    {
        return (int)playerScript.playerClientId;
    }

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
