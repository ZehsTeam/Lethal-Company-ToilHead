﻿using Unity.Netcode;

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
    public void SetToilHeadClientRpc(NetworkObjectReference enemyReference, bool isSlayer)
    {
        if (IsHost || IsServer) return;

        if (enemyReference.TryGet(out NetworkObject targetObject))
        {
            Plugin.Instance.SetToilHeadOnLocalClient(targetObject, isSlayer);
        }
        else
        {
            // Target not found on server, likely because it already has been destroyed/despawned.
            Plugin.Instance.SetToilHeadOnLocalClient(null, isSlayer);
        }
    }

    [ClientRpc]
    public void SetMantiToilClientRpc(NetworkObjectReference enemyReference, bool isSlayer)
    {
        if (IsHost || IsServer) return;

        if (enemyReference.TryGet(out NetworkObject targetObject))
        {
            Plugin.Instance.SetMantiToilOnLocalClient(targetObject, isSlayer);
        }
        else
        {
            // Target not found on server, likely because it already has been destroyed/despawned.
            Plugin.Instance.SetMantiToilOnLocalClient(null, isSlayer);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetToilHeadPlayerRagdollServerRpc(int fromPlayerId, bool isSlayer)
    {
        Plugin.Instance.SetToilHeadPlayerRagdoll(fromPlayerId, isSlayer);
    }

    [ClientRpc]
    public void SetToilHeadPlayerRagdollClientRpc(NetworkObjectReference ragdollReference, bool realTurret, bool isSlayer)
    {
        if (IsHost || IsServer) return;

        if (ragdollReference.TryGet(out NetworkObject targetObject))
        {
            Plugin.Instance.SetToilHeadPlayerRagdollOnLocalClient(targetObject, realTurret, isSlayer);
        }
        else
        {
            // Target not found on server, likely because it already has been destroyed/despawned.
            Plugin.Instance.SetToilHeadPlayerRagdollOnLocalClient(null, realTurret, isSlayer);
        }
    }

    [ClientRpc]
    public void SetToilPlayerClientRpc(NetworkObjectReference playerReference, bool isSlayer)
    {
        if (IsHost || IsServer) return;

        if (playerReference.TryGet(out NetworkObject targetObject))
        {
            Plugin.Instance.SetToilPlayerOnLocalClient(targetObject, isSlayer);
        }
        else
        {
            // Target not found on server, likely because it already has been destroyed/despawned.
            Plugin.Instance.SetToilPlayerOnLocalClient(null, isSlayer);
        }
    }
}
