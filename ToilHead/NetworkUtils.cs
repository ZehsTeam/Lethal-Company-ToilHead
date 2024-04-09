using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

internal class NetworkUtils
{
    public static int GetNetworkObjectId(GameObject gameObject)
    {
        return GetNetworkObjectId(gameObject.GetComponent<NetworkObject>());
    }

    public static int GetNetworkObjectId(NetworkObject networkObject)
    {
        return networkObject == null ? -1 : (int)networkObject.NetworkObjectId;
    }

    public static NetworkObject GetNetworkObject(int networkObjectId)
    {
        NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue((ulong)networkObjectId, out NetworkObject networkObject);
        return networkObject;
    }
}
