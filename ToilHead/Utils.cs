using com.github.zehsteam.ToilHead.MonoBehaviours.TurretHeads;
using GameNetcodeStuff;
using System.Collections;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class Utils
{
    public static bool RandomPercent(float percent)
    {
        if (percent <= 0f) return false;
        if (percent >= 100f) return true;
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

    public static void DisableRenderers(GameObject gameObject)
    {
        foreach (var renderer in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }

        foreach (var renderer in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.enabled = false;
        }
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

    public static bool IsValidEnemy(EnemyAI enemyScript)
    {
        if (IsSpring(enemyScript)) return true;
        if (IsManticoil(enemyScript)) return true;
        if (IsMasked(enemyScript)) return true;

        return false;
    }

    public static bool IsSpring(EnemyAI enemyScript)
    {
        return enemyScript.enemyType.enemyName == "Spring";
    }

    public static bool IsManticoil(EnemyAI enemyScript)
    {
        return enemyScript.enemyType.enemyName == "Manticoil";
    }

    public static bool IsMasked(EnemyAI enemyScript)
    {
        return enemyScript.enemyType.enemyName == "Masked";
    }

    public static bool IsTurretHead(EnemyAI enemyScript)
    {
        return enemyScript.GetComponentInChildren<TurretHeadControllerBehaviour>() != null;
    }

    public static bool IsTurretHead(PlayerControllerB playerScript)
    {
        return playerScript.GetComponentInChildren<TurretHeadControllerBehaviour>() != null;
    }

    public static bool IsTurretHead(DeadBodyInfo deadBodyScript)
    {
        return deadBodyScript.GetComponentInChildren<TurretHeadControllerBehaviour>() != null;
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
}
