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

    [ClientRpc]
    public void SetMantiToilClientRpc(NetworkObjectReference enemyReference)
    {
        if (IsHost || IsServer) return;

        if (enemyReference.TryGet(out NetworkObject targetObject))
        {
            Plugin.Instance.SetMantiToilOnLocalClient(targetObject);
        }
        else
        {
            // Target not found on server, likely because it already has been destroyed/despawned.
            Plugin.Instance.SetMantiToilOnLocalClient(null);
        }
    }

    [ClientRpc]
    public void SetToilSlayerClientRpc(NetworkObjectReference enemyReference)
    {
        if (IsHost || IsServer) return;

        if (enemyReference.TryGet(out NetworkObject targetObject))
        {
            Plugin.Instance.SetToilSlayerOnLocalClient(targetObject);
        }
        else
        {
            // Target not found on server, likely because it already has been destroyed/despawned.
            Plugin.Instance.SetToilSlayerOnLocalClient(null);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetToilHeadPlayerRagdollServerRpc(int fromPlayerId)
    {
        Plugin.Instance.SetToilHeadPlayerRagdoll(fromPlayerId);
    }

    [ClientRpc]
    public void SetToilHeadPlayerRagdollClientRpc(NetworkObjectReference ragdollReference, bool realTurret)
    {
        if (IsHost || IsServer) return;

        if (ragdollReference.TryGet(out NetworkObject targetObject))
        {
            Plugin.Instance.SetToilHeadPlayerRagdollOnLocalClient(targetObject, realTurret);
        }
        else
        {
            // Target not found on server, likely because it already has been destroyed/despawned.
            Plugin.Instance.SetToilHeadPlayerRagdollOnLocalClient(null, realTurret);
        }
    }
}
