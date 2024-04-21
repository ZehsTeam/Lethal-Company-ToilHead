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
        if (IsHost || IsServer) return;

        Plugin.logger.LogInfo("Syncing config with host.");
        Plugin.Instance.ConfigManager.SetHostConfigData(syncedConfigData);
    }

    [ClientRpc]
    public void SetToilHeadClientRpc(NetworkObjectReference enemyReference)
    {
        if (IsHost || IsServer) return;

        if (enemyReference.TryGet(out NetworkObject targetObject))
        {
            Plugin.Instance.SetToilHeadOnLocalClient(targetObject);
        }
        else
        {
            // Target not found on server, likely because it already has been destroyed/despawned.
            Plugin.Instance.SetToilHeadOnLocalClient(null);
        }
    }
}
