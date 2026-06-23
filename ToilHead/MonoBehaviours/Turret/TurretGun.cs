using Unity.Netcode;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.Turret;

public class TurretGun : NetworkBehaviour
{
    public TurretController Controller { get; private set; }

    private void Awake()
    {
        Controller = GetComponent<TurretController>();
    }


}
