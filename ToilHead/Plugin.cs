using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.ToilHead.Patches;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class ToilHeadBase : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static ToilHeadBase Instance;
    internal static ManualLogSource mls;

    internal ConfigManager configManager;

    public GameObject turretPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;

        mls = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        mls.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        harmony.PatchAll(typeof(GameNetworkManagerPatch));
        harmony.PatchAll(typeof(StartOfRoundPatch));
        harmony.PatchAll(typeof(RoundManagerPatch));
        harmony.PatchAll(typeof(EnemyAIPatch));

        configManager = new ConfigManager();

        NetcodePatcherAwake();
    }

    private void NetcodePatcherAwake()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();

        foreach (var type in types)
        {
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);

                if (attributes.Length > 0)
                {
                    method.Invoke(null, null);
                }
            }
        }
    }

    public void OnNewLevelLoaded(int randomMapSeed)
    {
        EnemyAIPatch.Initialize(randomMapSeed);

        Secret.SpawnAsteroid13Secret();
    }

    public void SetToilHeadOnServer(EnemyAI enemyAI)
    {
        if (enemyAI == null)
        {
            mls.LogError("Error: enemy could not be found.");
            return;
        }

        if (turretPrefab == null)
        {
            mls.LogError("Error: turret prefab could not be found.");
            return;
        }

        NetworkObject enemyAINetworkObject = enemyAI.gameObject.GetComponent<NetworkObject>();

        int forwardWeight = configManager.SpawnTurretFacingForwardWeight * 100;
        int backwardWeight = configManager.SpawnTurretFacingBackwardWeight * 100;
        bool turretFacingForward = EnemyAIPatch.random.Next(1, forwardWeight + backwardWeight) <= forwardWeight;

        SpawnTurretOnServer(enemyAI.gameObject.transform);
        PluginNetworkBehaviour.Instance.SetToilHeadClientRpc((int)enemyAINetworkObject.NetworkObjectId, turretFacingForward);
        SetToilHeadOnLocalClient(enemyAI.gameObject, turretFacingForward);
    }

    private GameObject SpawnTurretOnServer(Transform parent)
    {
        GameObject turret = Object.Instantiate(turretPrefab, parent.localPosition, Quaternion.identity, parent);

        NetworkObject turretNetworkObject = turret.GetComponent<NetworkObject>();
        turretNetworkObject.Spawn(destroyWithScene: true);

        turret.transform.SetParent(parent);

        return turret;
    }

    public void SetToilHeadOnLocalClient(GameObject enemy, bool turretFacingForward)
    {
        GameObject turret = enemy.transform.GetChild(1).gameObject;
        Transform head = enemy.transform.GetChild(0).Find("Head");

        turret.transform.localPosition = head.localPosition;

        ParentToTransformBehaviour component = turret.AddComponent<ParentToTransformBehaviour>();
        component.SetParent(head);

        if (turretFacingForward)
        {
            component.SetRotationOffset(new Vector3(90f, 0f, 0f));
        }
        else
        {
            component.SetRotationOffset(new Vector3(-20f, 180f, 0f));
        }

        DisableBoxCollidersForTurret(turret);

        mls.LogInfo("Spawned Toil-Head, Good luck :3");
    }

    private void DisableBoxCollidersForTurret(GameObject turret)
    {
        List<BoxCollider> colliders = turret.GetComponentsInChildren<BoxCollider>().ToList();

        colliders.ForEach(collider =>
        {
            if (collider.gameObject.name == "ScanNode") return;

            collider.enabled = false;
        });

        mls.LogInfo("Removed the box colliders from the turret on the Coil-Head.");
    }
}
