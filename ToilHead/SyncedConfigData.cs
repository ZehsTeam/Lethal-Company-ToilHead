using System;
using Unity.Netcode;

namespace com.github.zehsteam.ToilHead;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
[Serializable]
public class SyncedConfigData : INetworkSerializable
{
    // Turret Settings
    public float TurretLostLOSDuration;
    public float TurretRotationRange;
    public float TurretCodeAccessCooldownDuration;

    // Turret Detection Settings
    public bool TurretDetectionRotation;
    public float TurretDetectionRotationSpeed;

    // Turret Charging Settings
    public float TurretChargingDuration;
    public float TurretChargingRotationSpeed;

    // Turret Firing Settings
    public float TurretFiringRotationSpeed;

    // Turret Berserk Settings
    public float TurretBerserkDuration;
    public float TurretBerserkRotationSpeed;

    public SyncedConfigData() { }

    public SyncedConfigData(SyncedConfigManager configManager)
    {
        // Turret Settings
        TurretLostLOSDuration = configManager.TurretLostLOSDuration.Value;
        TurretRotationRange = configManager.TurretRotationRange.Value;
        TurretCodeAccessCooldownDuration = configManager.TurretCodeAccessCooldownDuration.Value;

        // Turret Detection Settings
        TurretDetectionRotation = configManager.TurretDetectionRotation.Value;
        TurretDetectionRotationSpeed = configManager.TurretDetectionRotationSpeed.Value;

        // Turret Charging Settings
        TurretChargingDuration = configManager.TurretChargingDuration.Value;
        TurretChargingRotationSpeed = configManager.TurretChargingRotationSpeed.Value;

        // Turret Firing Settings
        TurretFiringRotationSpeed = configManager.TurretFiringRotationSpeed.Value;

        // Turret Berserk Settings
        TurretBerserkDuration = configManager.TurretBerserkDuration.Value;
        TurretBerserkRotationSpeed = configManager.TurretBerserkRotationSpeed.Value;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        // Turret Settings
        serializer.SerializeValue(ref TurretLostLOSDuration);
        serializer.SerializeValue(ref TurretRotationRange);
        serializer.SerializeValue(ref TurretCodeAccessCooldownDuration);

        // Turret Detection Settings
        serializer.SerializeValue(ref TurretDetectionRotation);
        serializer.SerializeValue(ref TurretDetectionRotationSpeed);

        // Turret Charging Settings
        serializer.SerializeValue(ref TurretChargingDuration);
        serializer.SerializeValue(ref TurretChargingRotationSpeed);

        // Turret Firing Settings
        serializer.SerializeValue(ref TurretFiringRotationSpeed);

        // Turret Berserk Settings
        serializer.SerializeValue(ref TurretBerserkDuration);
        serializer.SerializeValue(ref TurretBerserkRotationSpeed);
    }
}
