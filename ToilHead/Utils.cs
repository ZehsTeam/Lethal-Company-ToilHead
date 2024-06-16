using com.github.zehsteam.ToilHead.MonoBehaviours;
using GameNetcodeStuff;
using System.Collections;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class Utils
{
    public static bool RandomPercent(int percent)
    {
        if (percent <= 0) return false;
        if (percent >= 100) return true;
        return Random.value <= percent * 0.01f;
    }

    public static void DisableColliders(GameObject gameObject, bool keepScanNodeEnabled = false)
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

        foreach (var collider in colliders)
        {
            if (keepScanNodeEnabled && collider.gameObject.name == "ScanNode")
            {
                continue;
            }

            collider.enabled = false;
        }
    }

    public static bool IsSpring(EnemyAI enemyAI)
    {
        return enemyAI.enemyType.enemyName == "Spring";
    }

    public static bool IsToilHead(EnemyAI enemyAI)
    {
        if (!IsSpring(enemyAI)) return false;

        return enemyAI.GetComponentInChildren<ToilHeadTurretBehaviour>() != null;
    }

    public static bool IsManticoil(EnemyAI enemyAI)
    {
        return enemyAI.enemyType.enemyName == "Manticoil";
    }

    public static bool IsMantiToil(EnemyAI enemyAI)
    {
        if (!IsManticoil(enemyAI)) return false;

        return enemyAI.GetComponentInChildren<ToilHeadTurretBehaviour>() != null;
    }

    public static bool IsTurretHead(EnemyAI enemyAI)
    {
        if (IsToilHead(enemyAI)) return true;
        if (IsMantiToil(enemyAI)) return true;

        return false;
    }

    public static bool IsToilHeadPlayerRagdoll(GameObject gameObject)
    {
        return gameObject.GetComponentInChildren<ToilHeadTurretBehaviour>() != null;
    }

    public static bool IsToilPlayer(PlayerControllerB playerScript)
    {
        return playerScript.GetComponentInChildren<ToilHeadTurretBehaviour>() != null;
    }

    public static ToilHeadTurretBehaviour GetTurretHeadTurretBehaviour(EnemyAI enemyAI)
    {
        return enemyAI.GetComponentInChildren<ToilHeadTurretBehaviour>();
    }

    public static bool TryGetTurretHeadTurretBehaviour(EnemyAI enemyAI, out ToilHeadTurretBehaviour toilHeadTurretBehaviour)
    {
        toilHeadTurretBehaviour = enemyAI.GetComponentInChildren<ToilHeadTurretBehaviour>();
        return toilHeadTurretBehaviour != null;
    }

    public static IEnumerator WaitUntil(System.Func<bool> predicate, float maxDuration = 5f, int iterationsPerSecond = 10)
    {
        float timer = 0f;

        float timePerIteration = 1f / iterationsPerSecond;

        while (timer < maxDuration)
        {
            if (predicate())
            {
                break;
            }

            yield return new WaitForSeconds(timePerIteration);
            timer += Time.deltaTime;
        }
    }

    public static bool IsCurrentMoonToilation()
    {
        if (StartOfRound.Instance == null) return false;

        string planetName = StartOfRound.Instance.currentLevel.PlanetName;

        if (planetName.Equals("69 Toilation", System.StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (planetName.Contains("Toilation", System.StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    public static void DisableRenderers(GameObject obj)
    {
        foreach (var renderer in obj.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }

        foreach (var renderer in obj.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.enabled = false;
        }
    }
}
