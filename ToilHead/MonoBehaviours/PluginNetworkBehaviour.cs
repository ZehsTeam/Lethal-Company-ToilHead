using Unity.Netcode;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

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
        if (Plugin.IsHostOrServer) return;

        Plugin.logger.LogInfo("Syncing config with host.");
        Plugin.Instance.ConfigManager.SetHostConfigData(syncedConfigData);
    }

    [ClientRpc]
    public void SetToilHeadClientRpc(int enemyNetworkObjectId)
    {
        if (Plugin.IsHostOrServer) return;

        NetworkObject enemyNetworkObject = NetworkUtils.GetNetworkObject(enemyNetworkObjectId);

        Plugin.Instance.SetToilHeadOnLocalClient(enemyNetworkObject.gameObject);
    }
}
