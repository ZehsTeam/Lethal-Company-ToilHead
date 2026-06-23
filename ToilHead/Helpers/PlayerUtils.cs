using GameNetcodeStuff;
using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace com.github.zehsteam.ToilHead.Helpers;

internal static class PlayerUtils
{
    public static PlayerControllerB LocalPlayerScript => GameNetworkManager.Instance?.localPlayerController ?? null;

    public static PlayerControllerB[] AllPlayerScripts => StartOfRound.Instance?.allPlayerScripts ?? [];
    public static PlayerControllerB[] ConnectedPlayerScripts => [.. AllPlayerScripts.Where(IsConnected)];
    public static PlayerControllerB[] AlivePlayerScripts => [.. ConnectedPlayerScripts.Where(x => !x.isPlayerDead)];
    public static PlayerControllerB[] DeadPlayerScripts => [.. ConnectedPlayerScripts.Where(x => x.isPlayerDead)];

    public static bool TryGetLocalPlayerScript(out PlayerControllerB playerScript)
    {
        playerScript = LocalPlayerScript;
        return playerScript != null;
    }

    public static bool IsLocalPlayer(PlayerControllerB playerScript)
    {
        if (playerScript == null)
            return false;

        return playerScript == LocalPlayerScript;
    }

    public static bool IsConnected(PlayerControllerB playerScript)
    {
        if (playerScript == null)
            return false;

        return playerScript.isPlayerControlled || playerScript.isPlayerDead;
    }

    #region Get by
    // Client ID
    public static PlayerControllerB GetPlayerScriptByClientId(ulong clientId)
    {
        return ConnectedPlayerScripts.FirstOrDefault(playerScript => playerScript.actualClientId == clientId);
    }

    public static bool TryGetPlayerScriptByClientId(ulong clientId, out PlayerControllerB playerScript)
    {
        playerScript = GetPlayerScriptByClientId(clientId);
        return playerScript != null;
    }

    // Player Index
    public static PlayerControllerB GetPlayerScriptByPlayerId(int playerId)
    {
        if (playerId < 0 || playerId > ConnectedPlayerScripts.Length - 1)
            return null;

        return ConnectedPlayerScripts[playerId];
    }

    public static bool TryGetPlayerScriptByPlayerId(int playerId, out PlayerControllerB playerScript)
    {
        playerScript = GetPlayerScriptByPlayerId(playerId);
        return playerScript != null;
    }

    // Username
    public static PlayerControllerB GetPlayerScriptByUsername(string username)
    {
        PlayerControllerB[] playerScripts = [.. ConnectedPlayerScripts.OrderBy(x => x.playerUsername.Length)];

        PlayerControllerB targetPlayerScript = playerScripts.FirstOrDefault(x => x.playerUsername.Equals(username, StringComparison.OrdinalIgnoreCase));
        targetPlayerScript ??= playerScripts.FirstOrDefault(x => x.playerUsername.StartsWith(username, StringComparison.OrdinalIgnoreCase));
        targetPlayerScript ??= playerScripts.FirstOrDefault(x => x.playerUsername.Contains(username, StringComparison.OrdinalIgnoreCase));
        return targetPlayerScript;
    }

    public static bool TryGetPlayerScriptByUsername(string username, out PlayerControllerB playerScript)
    {
        playerScript = GetPlayerScriptByUsername(username);
        return playerScript != null;
    }
    #endregion

    // Random
    public static PlayerControllerB GetRandomPlayerScript(PlayerControllerB[] playerScripts, bool excludeLocal = false)
    {
        if (playerScripts == null || playerScripts.Length == 0)
            return null;

        PlayerControllerB[] filteredPlayerScripts = [.. playerScripts.Where(playerScript =>
        {
            if (!excludeLocal)
                return true;

            return !IsLocalPlayer(playerScript);
        })];

        if (filteredPlayerScripts.Length == 0)
            return null;

        return filteredPlayerScripts[Random.Range(0, filteredPlayerScripts.Length)];
    }

    public static bool TryGetRandomPlayerScript(PlayerControllerB[] playerScripts, out PlayerControllerB playerScript, bool excludeLocal = false)
    {
        playerScript = GetRandomPlayerScript(playerScripts, excludeLocal);
        return playerScript != null;
    }
}
