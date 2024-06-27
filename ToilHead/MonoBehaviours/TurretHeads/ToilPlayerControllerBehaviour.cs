using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ToilPlayerControllerBehaviour : TurretHeadControllerBehaviour
{
    protected override Transform GetHeadTransform()
    {
        return transform.parent.Find("ScavengerModel").Find("metarig").GetChild(0).GetChild(0).GetChild(0).GetChild(0).Find("spine.004").Find("HeadPoint");
    }

    protected override void OnFinishedSetup()
    {
        if (IsServer && !Plugin.ConfigManager.SpawnRealToiledPlayerRagdolls.Value)
        {
            TurretBehaviour.SetCanTargetPlayersClientRpc(false);
        }

        if (PlayerUtils.IsLocalPlayer(GetPlayerScript()))
        {
            Utils.DisableRenderers(TurretBehaviour.gameObject);
        }

        TurretHeadManager.AddPlayerTurretHeadControllerPair(GetPlayerScript(), this);
        TurretHeadManager.AddToPlayerSpawnCount();
    }
}
