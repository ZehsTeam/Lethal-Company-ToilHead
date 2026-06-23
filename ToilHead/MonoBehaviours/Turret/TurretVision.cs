using com.github.zehsteam.ToilHead.Extensions;
using com.github.zehsteam.ToilHead.Helpers;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.Turret;

public class TurretVision : NetworkBehaviour
{
    public TurretController Controller { get; private set; }
    public TurretTarget? CurrentTarget { get; private set; }

    public UnityEvent<TurretTarget?> OnTargetChanged;

    #region Unity fields
    [SerializeField]
    private bool _targetPlayers = true;

    [SerializeField]
    private bool _targetEnemies = false;

    [SerializeField]
    private float _viewDistance = 30f;

    [SerializeField]
    private float _viewAngleHorizontal = 90f;

    [SerializeField]
    private float _viewAngleVertical = 30f;
    #endregion

    private void Awake()
    {
        Controller = GetComponent<TurretController>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!NetworkUtils.IsServer)
            return;


    }

    #region TurretTarget mutations
    [ClientRpc]
    private void SetTarget_ClientRpc(TurretTarget turretTarget)
    {
        SetTarget_Local(turretTarget);
    }

    [ClientRpc]
    private void RemoveTarget_ClientRpc()
    {
        SetTarget_Local(null);
    }

    private void SetTarget_Local(TurretTarget? turretTarget)
    {
        CurrentTarget = turretTarget;
        OnTargetChanged?.Invoke(turretTarget);
    }
    #endregion
}

public enum TurretTargetType
{
    Player,
    Enemy
}

public struct TurretTarget : INetworkSerializable
{
    public TurretTargetType TargetType;

    // TurretTargetType.Player
    private ulong _targetClientId;

    // TurretTargetType.Enemy
    private NetworkObjectReference _targetEnemyReference;

    public static TurretTarget CreateTargetPlayer(PlayerControllerB playerScript)
    {
        return new TurretTarget
        {
            TargetType = TurretTargetType.Player,
            _targetClientId = playerScript.GetClientId()
        };
    }

    public static TurretTarget CreateTargetEnemy(EnemyAI enemyAI)
    {
        return new TurretTarget
        {
            TargetType = TurretTargetType.Enemy,
            _targetEnemyReference = enemyAI.NetworkObject
        };
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref TargetType);

        switch (TargetType)
        {
            case TurretTargetType.Player:
                serializer.SerializeValue(ref _targetClientId);
                break;
            case TurretTargetType.Enemy:
                serializer.SerializeNetworkSerializable(ref _targetEnemyReference);
                break;
        }
    }

    public bool TryGetPlayer(out PlayerControllerB playerScript)
    {
        if (TargetType != TurretTargetType.Player)
        {
            playerScript = null;
            return false;
        }

        return PlayerUtils.TryGetPlayerScriptByClientId(_targetClientId, out playerScript);
    }

    public bool TryGetEnemy(out EnemyAI enemyAI)
    {
        if (TargetType != TurretTargetType.Enemy)
        {
            enemyAI = null;
            return false;
        }

        if (_targetEnemyReference.TryGet(out NetworkObject networkObject))
        {
            return networkObject.TryGetComponent(out enemyAI);
        }

        enemyAI = null;
        return false;
    }
}
