using Unity.Netcode;

namespace com.github.zehsteam.ToilHead;

internal class PluginNetworkBehaviour : NetworkBehaviour
{
    public static PluginNetworkBehaviour Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    [ClientRpc]
    public void SetToilHeadClientRpc(int networkObjectId, bool turretFacingForward)
    {
        if (IsHost || IsServer) return;

        NetworkObject enemyNetworkObject = GetNetworkObjectById(networkObjectId);
        ToilHeadBase.Instance.SetToilHeadOnLocalClient(enemyNetworkObject.gameObject, turretFacingForward);
    }

    public NetworkObject GetNetworkObjectById(int networkObjectId)
    {
        return NetworkManager.Singleton.SpawnManager.SpawnedObjects[(ulong)networkObjectId];
    }
}
