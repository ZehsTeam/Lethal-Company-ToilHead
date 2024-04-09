using Unity.Netcode;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

internal class PluginNetworkBehaviour : NetworkBehaviour
{
    public static PluginNetworkBehaviour Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    [ClientRpc]
    public void SetToilHeadClientRpc(int enemyNetworkObjectId, bool turretFacingForward, bool hideTurretBody)
    {
        if (Plugin.IsHostOrServer) return;

        NetworkObject enemyNetworkObject = NetworkUtils.GetNetworkObject(enemyNetworkObjectId);

        Plugin.Instance.SetToilHeadOnLocalClient(enemyNetworkObject.gameObject, turretFacingForward, hideTurretBody);
    }
}
