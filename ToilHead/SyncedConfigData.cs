using System;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead;

[Serializable]
public class SyncedConfigData : INetworkSerializable
{
    // Turret Settings
    public bool turretRotationWhenSearching;
    public float turretDetectionRotationSpeed;
    public float turretChargingRotationSpeed;
    public float turretRotationRange;
    public float turretCodeAccessCooldownDuration;

    public SyncedConfigData() { }

    public SyncedConfigData(SyncedConfigManager configManager)
    {
        // Turret Settings
        turretRotationWhenSearching = configManager.TurretRotationWhenSearching;
        turretDetectionRotationSpeed = configManager.TurretDetectionRotationSpeed;
        turretChargingRotationSpeed = configManager.TurretChargingRotationSpeed;
        turretRotationRange = configManager.TurretRotationRange;
        turretCodeAccessCooldownDuration = configManager.TurretCodeAccessCooldownDuration;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        // Turret Settings
        serializer.SerializeValue(ref turretRotationWhenSearching);
        serializer.SerializeValue(ref turretDetectionRotationSpeed);
        serializer.SerializeValue(ref turretChargingRotationSpeed);
        serializer.SerializeValue(ref turretRotationRange);
        serializer.SerializeValue(ref turretCodeAccessCooldownDuration);
    }
}
