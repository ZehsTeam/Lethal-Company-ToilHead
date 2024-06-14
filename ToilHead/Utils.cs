﻿using com.github.zehsteam.ToilHead.MonoBehaviours;
using System.Collections;
using UnityEngine;

namespace com.github.zehsteam.ToilHead;

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

    public static ToilHeadTurretBehaviour GetToilHeadTurretBehaviour(EnemyAI enemyAI)
    {
        return enemyAI.GetComponentInChildren<ToilHeadTurretBehaviour>();
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

    public static bool IsOnToilation()
    {
        if (StartOfRound.Instance == null) return false;

        string planetName = StartOfRound.Instance.currentLevel.PlanetName;

        if (planetName == "69 Toilation" || planetName.Contains("Toilation", System.StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }
}
