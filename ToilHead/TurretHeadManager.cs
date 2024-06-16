using com.github.zehsteam.ToilHead.MonoBehaviours;
using GameNetcodeStuff;
using System.Collections.Generic;

namespace com.github.zehsteam.ToilHead;

internal class TurretHeadManager
{
    public static Dictionary<EnemyAI, ToilHeadTurretBehaviour> EnemyTurretPairs { get; private set; } = [];
    public static Dictionary<PlayerControllerB, ToilHeadTurretBehaviour> PlayerTurretPairs { get; private set; } = [];
    public static Dictionary<DeadBodyInfo, ToilHeadTurretBehaviour> ToiledTurretPairs { get; private set; } = [];

    public static void Initialize()
    {

    }

    public static void Reset()
    {

    }
}
