using com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;
using GameNetcodeStuff;

namespace com.github.zehsteam.ToilHead.Extensions;

internal static class PlayerExtensions
{
    public static ulong GetClientId(this PlayerControllerB playerScript)
    {
        return playerScript.actualClientId;
    }

    public static bool IsTurretHead(this PlayerControllerB playerScript)
    {
        return playerScript.GetComponentInChildren<TurretHeadControllerBehaviour>() != null;
    }

    public static bool IsTurretHead(this DeadBodyInfo deadBodyScript)
    {
        return deadBodyScript.GetComponentInChildren<TurretHeadControllerBehaviour>() != null;
    }
}
