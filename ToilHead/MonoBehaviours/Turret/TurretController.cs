using Unity.Netcode;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.Turret;

public class TurretController : NetworkBehaviour
{
    public TurretVision Vision { get; private set; }
    public TurretMotor Motor { get; private set; }
    public TurretGun Gun { get; private set; }

    private void Awake()
    {
        Vision = GetComponent<TurretVision>();
        Motor = GetComponent<TurretMotor>();
        Gun = GetComponent<TurretGun>();
    }


}
