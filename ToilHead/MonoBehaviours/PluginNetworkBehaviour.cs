using GameNetcodeStuff;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class PluginNetworkBehaviour : NetworkBehaviour
{
    public static PluginNetworkBehaviour Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    [ClientRpc]
    internal void SendConfigToPlayerClientRpc(SyncedConfigData syncedConfigData, ClientRpcParams clientRpcParams = default)
    {
        if (IsHost || IsServer) return;

        Plugin.logger.LogInfo("Syncing config with host.");
        Plugin.ConfigManager.SetHostConfigData(syncedConfigData);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetToilHeadPlayerRagdollServerRpc(int fromPlayerId, bool isSlayer)
    {
        TurretHeadManager.SetDeadBodyTurretHeadOnServer(PlayerUtils.GetPlayerScript(fromPlayerId), isSlayer);
    }
}
