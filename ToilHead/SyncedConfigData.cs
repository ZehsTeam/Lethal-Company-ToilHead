using System;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead;

[Serializable]
public class SyncedConfigData : INetworkSerializable
{
    // Turret Settings
    public float turretLostLOSDuration;
    public float turretRotationRange;
    public float turretCodeAccessCooldownDuration;

    // Turret Detection Settings
    public bool turretDetectionRotation;
    public float turretDetectionRotationSpeed;

    // Turret Charging Settings
    public float turretChargingDuration;
    public float turretChargingRotationSpeed;

    // Turret Firing Settings
    public float turretFiringRotationSpeed;

    // Turret Berserk Settings
    public float turretBerserkDuration;
    public float turretBerserkRotationSpeed;

    public SyncedConfigData() { }

    public SyncedConfigData(SyncedConfigManager configManager)
    {
        // Turret Settings
        turretLostLOSDuration = configManager.TurretLostLOSDuration;
        turretRotationRange = configManager.TurretRotationRange;
        turretCodeAccessCooldownDuration = configManager.TurretCodeAccessCooldownDuration;

        // Turret Detection Settings
        turretDetectionRotation = configManager.TurretDetectionRotation;
        turretDetectionRotationSpeed = configManager.TurretDetectionRotationSpeed;

        // Turret Charging Settings
        turretChargingDuration = configManager.TurretChargingDuration;
        turretChargingRotationSpeed = configManager.TurretChargingRotationSpeed;

        // Turret Firing Settings
        turretFiringRotationSpeed = configManager.TurretFiringRotationSpeed;

        // Turret Berserk Settings
        turretBerserkDuration = configManager.TurretBerserkDuration;
        turretBerserkRotationSpeed = configManager.TurretBerserkRotationSpeed;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        // Turret Settings
        serializer.SerializeValue(ref turretLostLOSDuration);
        serializer.SerializeValue(ref turretRotationRange);
        serializer.SerializeValue(ref turretCodeAccessCooldownDuration);

        // Turret Detection Settings
        serializer.SerializeValue(ref turretDetectionRotation);
        serializer.SerializeValue(ref turretDetectionRotationSpeed);

        // Turret Charging Settings
        serializer.SerializeValue(ref turretChargingDuration);
        serializer.SerializeValue(ref turretChargingRotationSpeed);

        // Turret Firing Settings
        serializer.SerializeValue(ref turretFiringRotationSpeed);

        // Turret Berserk Settings
        serializer.SerializeValue(ref turretBerserkDuration);
        serializer.SerializeValue(ref turretBerserkRotationSpeed);
    }
}
