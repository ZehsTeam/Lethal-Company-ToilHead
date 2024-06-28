using GameNetcodeStuff;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ToilPlayerControllerBehaviour : TurretHeadControllerBehaviour
{
    protected override Transform GetHeadTransform()
    {
        return transform.parent.Find("ScavengerModel").Find("metarig").GetChild(0).GetChild(0).GetChild(0).GetChild(0).Find("spine.004");
    }

    protected override void OnFinishedSetup()
    {
        if (IsServer && !Plugin.ConfigManager.SpawnRealToiledPlayerRagdolls.Value)
        {
            TurretBehaviour.SetCanTargetPlayersClientRpc(false);
        }

        PlayerControllerB playerScript = GetPlayerScript();

        if (PlayerUtils.IsLocalPlayer(playerScript))
        {
            Utils.DisableRenderers(TurretBehaviour.gameObject);
        }

        if (IsServer)
        {
            TurretHeadManager.AddPlayerTurretHeadControllerPair(playerScript, this);
            TurretHeadManager.AddToPlayerSpawnCount();
        }
    }
}
