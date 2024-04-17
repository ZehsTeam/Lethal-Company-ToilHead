using GameNetcodeStuff;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

public class ToilHeadTurretBehaviour : NetworkBehaviour
{
    [Header("Audio Sources")]
    [Space(3f)]
    public AudioSource mainAudio;
    public AudioSource farAudio;
    public AudioSource bulletCollisionAudio;
    public AudioSource berserkAudio;

    [Header("Audio Clips")]
    [Space(3f)]
    public AudioClip firingSFX;
    public AudioClip chargingSFX;
    public AudioClip detectPlayerSFX;
    public AudioClip firingFarSFX;
    public AudioClip bulletsHitWallSFX;
    public AudioClip turretActivate;
    public AudioClip turretDeactivate;

    [Header("Other")]
    [Space(3f)]
    public ParticleSystem bulletParticles;
    public Animator turretAnimator;
    [HideInInspector] public bool turretActive = true;
    [HideInInspector] public TurretMode turretMode = TurretMode.Detection;
    [HideInInspector] public PlayerControllerB targetPlayerWithRotation;
    [HideInInspector] public float targetRotation;
    [HideInInspector] public float rotationSpeed = 28f;
    [HideInInspector] public float rotationRange = 75f;

    [Header("Transforms")]
    [Space(3f)]
    public Transform turretRod;
    public Transform turnTowardsObjectCompass;
    public Transform forwardFacingPos;
    public Transform aimPoint;
    public Transform aimTurretCenterPoint;
    public Transform centerPoint;
    public Transform tempTransform;
    [HideInInspector] public Transform targetTransform;

    [Header("Line Of Sight")]
    public float LOSRange = 30f;
    public float LOSDistance = 30f;
    public int LOSVerticalRays = 1;
    public int LOSHorizontalRays = 7;
    public float LOSXRotationOffsetStart = 0f;
    public float LOSXRotationOffsetAmountPerRay = 0f;

    #region Private Variables
    private TurretMode turretModeLastFrame;
    private bool targetingDeadPlayer;
    private bool rotatingRight;
    private float switchRotationTimer;
    private bool hasLineOfSight;
    private float lostLOSTimer;
    private RaycastHit hit;
    private bool wasTargetingPlayerLastFrame;
    private float turretInterval;
    private bool rotatingSmoothly = true;
    private Ray shootRay;
    private Coroutine fadeBulletAudioCoroutine;
    private bool rotatingClockwise;
    private float berserkTimer;
    private bool enteringBerserkMode;
    #endregion

    #region Custom Variables
    // Turret Settings
    [HideInInspector] public float lostLOSDuration = 1f;

    // Turret Detection Settings
    [HideInInspector] public bool detectionRotation = false;
    [HideInInspector] public float detectionRotationSpeed = 28f;

    // Turret Charging Settings
    [HideInInspector] public float chargingDuration = 2f;
    [HideInInspector] public float chargingRotationSpeed = 95f;

    // Turret Firing Settings
    [HideInInspector] public float firingRotationSpeed = 95f;

    // Turret Berserk Settings
    [HideInInspector] public float berserkDuration = 9f;
    [HideInInspector] public float berserkRotationSpeed = 77f;
    #endregion

    private void Start()
    {
        SetCustomVariables();

        if (Plugin.IsHostOrServer)
        {
            SetObjectCodeOnServer();
        }
    }

    private void SetCustomVariables()
    {
        SyncedConfigManager configManager = Plugin.Instance.ConfigManager;

        // Turret Settings
        lostLOSDuration = configManager.TurretLostLOSDuration;
        rotationRange = Mathf.Abs(configManager.TurretRotationRange);

        // Turret Detection Settings
        detectionRotation = configManager.TurretDetectionRotation;
        detectionRotationSpeed = configManager.TurretDetectionRotationSpeed;

        // Turret Charging Settings
        chargingDuration = configManager.TurretChargingDuration;
        chargingRotationSpeed = configManager.TurretChargingRotationSpeed;

        // Turret Firing Settings
        firingRotationSpeed = configManager.TurretFiringRotationSpeed;

        // Turret Berserk Settings
        berserkDuration = configManager.TurretBerserkDuration;
        berserkRotationSpeed = configManager.TurretBerserkRotationSpeed;

        rotationSpeed = detectionRotationSpeed;
    }

    private IEnumerator FadeBulletAudio()
    {
        float initialVolume = bulletCollisionAudio.volume;
        for (int i = 0; i <= 30; i++)
        {
            yield return new WaitForSeconds(0.012f);
            bulletCollisionAudio.volume = Mathf.Lerp(initialVolume, 0f, (float)i / 30f);
        }
        bulletCollisionAudio.Stop();
    }

    private void Update()
    {
        if (!OtherUpdateStuff()) return;
        if (!TurretModeLogic()) return;
        if (!RotateTurretRod()) return;
    }

    private bool OtherUpdateStuff()
    {
        if (!turretActive)
        {
            wasTargetingPlayerLastFrame = false;
            turretMode = TurretMode.Detection;
            targetPlayerWithRotation = null;
            return false;
        }

        if (targetPlayerWithRotation != null)
        {
            if (!wasTargetingPlayerLastFrame)
            {
                wasTargetingPlayerLastFrame = true;
                if (turretMode == TurretMode.Detection)
                {
                    turretMode = TurretMode.Charging;
                }
            }
            SetTargetToPlayerBody();
            TurnTowardsTargetIfHasLOS();
        }
        else if (wasTargetingPlayerLastFrame)
        {
            wasTargetingPlayerLastFrame = false;
            turretMode = TurretMode.Detection;
        }

        return true;
    }

    #region TurretMode Logic
    private bool TurretModeLogic()
    {
        switch (turretMode)
        {
            case TurretMode.Detection:
                TurretModeDetectionLogic();
                break;
            case TurretMode.Charging:
                TurretModeChargingLogic();
                break;
            case TurretMode.Firing:
                TurretModeFiringLogic();
                break;
            case TurretMode.Berserk:
                TurretModeBerserkLogic();
                break;
        }

        return true;
    }

    private void TurretModeDetectionLogic()
    {
        if (turretModeLastFrame != TurretMode.Detection)
        {
            turretModeLastFrame = TurretMode.Detection;
            rotatingClockwise = false;
            mainAudio.Stop();
            farAudio.Stop();
            berserkAudio.Stop();
            if (fadeBulletAudioCoroutine != null)
            {
                StopCoroutine(fadeBulletAudioCoroutine);
            }
            fadeBulletAudioCoroutine = StartCoroutine(FadeBulletAudio());
            bulletParticles.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
            rotationSpeed = detectionRotationSpeed;
            rotatingSmoothly = true;
            turretAnimator.SetInteger("TurretMode", 0);
            turretInterval = Random.Range(0f, 0.15f);
        }
        if (!IsServer)
        {
            return;
        }
        if (switchRotationTimer >= 7f)
        {
            switchRotationTimer = 0f;
            bool setRotateRight = !rotatingRight;
            SwitchRotationClientRpc(setRotateRight);
        }
        else
        {
            switchRotationTimer += Time.deltaTime;
        }
        if (turretInterval >= 0.25f)
        {
            turretInterval = 0f;
            PlayerControllerB playerControllerB = CheckForPlayersInLineOfSight(angleRangeCheck: true);
            if (playerControllerB != null && !playerControllerB.isPlayerDead)
            {
                targetPlayerWithRotation = playerControllerB;
                SwitchTurretMode(1);
                SwitchTargetedPlayerClientRpc((int)playerControllerB.playerClientId, setModeToCharging: true);
            }
        }
        else
        {
            turretInterval += Time.deltaTime;
        }
    }

    private void TurretModeChargingLogic()
    {
        if (turretModeLastFrame != TurretMode.Charging)
        {
            turretModeLastFrame = TurretMode.Charging;
            rotatingClockwise = false;
            mainAudio.PlayOneShot(detectPlayerSFX);
            berserkAudio.Stop();
            WalkieTalkie.TransmitOneShotAudio(mainAudio, detectPlayerSFX);
            rotationSpeed = chargingRotationSpeed;
            rotatingSmoothly = false;
            lostLOSTimer = 0f;
            turretAnimator.SetInteger("TurretMode", 1);
        }
        if (!IsServer)
        {
            return;
        }
        if (turretInterval >= chargingDuration)
        {
            turretInterval = 0f;
            Plugin.logger.LogInfo("Charging timer is up, setting to firing mode");
            if (!hasLineOfSight)
            {
                Plugin.logger.LogInfo("hasLineOfSight is false");
                targetPlayerWithRotation = null;
                RemoveTargetedPlayerClientRpc();
            }
            else
            {
                SwitchTurretMode(2);
                SetToModeClientRpc(2);
            }
        }
        else
        {
            turretInterval += Time.deltaTime;
        }
    }

    private void TurretModeFiringLogic()
    {
        if (turretModeLastFrame != TurretMode.Firing)
        {
            turretModeLastFrame = TurretMode.Firing;
            berserkAudio.Stop();
            mainAudio.clip = firingSFX;
            mainAudio.Play();
            farAudio.clip = firingFarSFX;
            farAudio.Play();
            bulletParticles.Play(withChildren: true);
            bulletCollisionAudio.Play();
            if (fadeBulletAudioCoroutine != null)
            {
                StopCoroutine(fadeBulletAudioCoroutine);
            }
            bulletCollisionAudio.volume = 1f;
            rotationSpeed = firingRotationSpeed;
            rotatingSmoothly = false;
            lostLOSTimer = 0f;
            turretAnimator.SetInteger("TurretMode", 2);
        }
        if (turretInterval >= 0.21f)
        {
            turretInterval = 0f;
            if (CheckForPlayersInLineOfSight(range: 5, verticalRays: 1) == GameNetworkManager.Instance.localPlayerController)
            {
                if (GameNetworkManager.Instance.localPlayerController.health > 50)
                {
                    GameNetworkManager.Instance.localPlayerController.DamagePlayer(50, hasDamageSFX: true, callRPC: true, CauseOfDeath.Gunshots);
                }
                else
                {
                    GameNetworkManager.Instance.localPlayerController.KillPlayer(aimPoint.forward * 40f, spawnBody: true, CauseOfDeath.Gunshots);
                }
            }
            shootRay = new Ray(aimPoint.position, aimPoint.forward);
            if (Physics.Raycast(shootRay, out hit, 30f, StartOfRound.Instance.collidersAndRoomMask, QueryTriggerInteraction.Ignore))
            {
                bulletCollisionAudio.transform.position = shootRay.GetPoint(hit.distance - 0.5f);
            }
        }
        else
        {
            turretInterval += Time.deltaTime;
        }
    }

    private void TurretModeBerserkLogic()
    {
        if (turretModeLastFrame != TurretMode.Berserk)
        {
            turretModeLastFrame = TurretMode.Berserk;
            turretAnimator.SetInteger("TurretMode", 1);
            berserkTimer = 1.3f;
            berserkAudio.Play();
            rotationSpeed = berserkRotationSpeed;
            enteringBerserkMode = true;
            rotatingSmoothly = true;
            lostLOSTimer = 0f;
            wasTargetingPlayerLastFrame = false;
            targetPlayerWithRotation = null;
        }
        if (enteringBerserkMode)
        {
            berserkTimer -= Time.deltaTime;
            if (berserkTimer <= 0f)
            {
                enteringBerserkMode = false;
                rotatingClockwise = true;
                berserkTimer = berserkDuration;
                turretAnimator.SetInteger("TurretMode", 2);
                mainAudio.clip = firingSFX;
                mainAudio.Play();
                farAudio.clip = firingFarSFX;
                farAudio.Play();
                bulletParticles.Play(withChildren: true);
                bulletCollisionAudio.Play();
                if (fadeBulletAudioCoroutine != null)
                {
                    StopCoroutine(fadeBulletAudioCoroutine);
                }
                bulletCollisionAudio.volume = 1f;
            }
            return;
        }
        if (turretInterval >= 0.21f)
        {
            turretInterval = 0f;
            if (CheckForPlayersInLineOfSight(range: 5, verticalRays: 1) == GameNetworkManager.Instance.localPlayerController)
            {
                if (GameNetworkManager.Instance.localPlayerController.health > 50)
                {
                    GameNetworkManager.Instance.localPlayerController.DamagePlayer(50, hasDamageSFX: true, callRPC: true, CauseOfDeath.Gunshots);
                }
                else
                {
                    GameNetworkManager.Instance.localPlayerController.KillPlayer(aimPoint.forward * 40f, spawnBody: true, CauseOfDeath.Gunshots);
                }
            }
            shootRay = new Ray(aimPoint.position, aimPoint.forward);
            if (Physics.Raycast(shootRay, out hit, 30f, StartOfRound.Instance.collidersAndRoomMask, QueryTriggerInteraction.Ignore))
            {
                bulletCollisionAudio.transform.position = shootRay.GetPoint(hit.distance - 0.5f);
            }
        }
        else
        {
            turretInterval += Time.deltaTime;
        }
        if (IsServer)
        {
            berserkTimer -= Time.deltaTime;
            if (berserkTimer <= 0f || !turretActive)
            {
                SwitchTurretMode(0);
                SetToModeClientRpc(0);
            }
        }
    }
    #endregion

    private bool RotateTurretRod()
    {
        if (rotatingClockwise)
        {
            turnTowardsObjectCompass.localEulerAngles = new Vector3(25f, turretRod.localEulerAngles.y - Time.deltaTime * rotationSpeed, 0f);
            turretRod.localRotation = Quaternion.RotateTowards(turretRod.localRotation, turnTowardsObjectCompass.localRotation, rotationSpeed * Time.deltaTime);
            return false;
        }

        if (rotatingSmoothly)
        {
            if (detectionRotation)
            {
                turnTowardsObjectCompass.localEulerAngles = new Vector3(0f, Mathf.Clamp(targetRotation, 0f - rotationRange, rotationRange), 0f);
            }
            else
            {
                turnTowardsObjectCompass.localEulerAngles = Vector3.zero;
            }
        }

        turretRod.localRotation = Quaternion.RotateTowards(turretRod.localRotation, turnTowardsObjectCompass.localRotation, rotationSpeed * Time.deltaTime);

        return true;
    }

    private void SetTargetToPlayerBody()
    {
        if (targetPlayerWithRotation.isPlayerDead)
        {
            if (!targetingDeadPlayer)
            {
                targetingDeadPlayer = true;
            }
            if (targetPlayerWithRotation.deadBody != null)
            {
                targetTransform = targetPlayerWithRotation.deadBody.bodyParts[5].transform;
            }
        }
        else
        {
            targetingDeadPlayer = false;
            targetTransform = targetPlayerWithRotation.gameplayCamera.transform;
        }
    }

    private void TurnTowardsTargetIfHasLOS()
    {
        bool flag = true;
        if (targetingDeadPlayer || Vector3.Angle(targetTransform.position - centerPoint.position, forwardFacingPos.forward) > rotationRange)
        {
            flag = false;
        }
        if (Physics.Linecast(aimPoint.position, targetTransform.position, StartOfRound.Instance.collidersAndRoomMask, QueryTriggerInteraction.Ignore))
        {
            flag = false;
        }

        if (flag)
        {
            hasLineOfSight = true;
            lostLOSTimer = 0f;
            tempTransform.position = targetTransform.position;
            tempTransform.position -= Vector3.up * 0.15f;
            turnTowardsObjectCompass.LookAt(tempTransform);
            return;
        }

        if (hasLineOfSight)
        {
            hasLineOfSight = false;
            lostLOSTimer = 0f;
        }

        if (!IsServer) return;

        lostLOSTimer += Time.deltaTime;

        if (lostLOSTimer >= lostLOSDuration)
        {
            lostLOSTimer = 0f;
            Plugin.logger.LogInfo("LOS timer ended on server. checking for new player target");
            PlayerControllerB playerControllerB = CheckForPlayersInLineOfSight();
            if (playerControllerB != null)
            {
                targetPlayerWithRotation = playerControllerB;
                SwitchTargetedPlayerClientRpc((int)playerControllerB.playerClientId);
                Plugin.logger.LogInfo("Got new player target");
            }
            else
            {
                Plugin.logger.LogInfo("No new player to target; returning to detection mode.");
                targetPlayerWithRotation = null;
                RemoveTargetedPlayerClientRpc();
            }
        }
    }

    #region LineOfSight
    public PlayerControllerB CheckForPlayersInLineOfSight(float range = -1f, int verticalRays = -1, int horizontalRays = -1, bool angleRangeCheck = false)
    {
        if (range == -1f) range = LOSRange;
        if (verticalRays == -1) verticalRays = LOSVerticalRays;
        if (horizontalRays == -1) horizontalRays = LOSHorizontalRays;

        return CheckForPlayersInLineOfSightVertical(range, verticalRays, horizontalRays, angleRangeCheck);
    }

    private PlayerControllerB CheckForPlayersInLineOfSightVertical(float range, int verticalRays, int horizontalRays, bool angleRangeCheck)
    {
        float xRotationOffset = LOSXRotationOffsetStart;

        if (verticalRays < 2)
        {
            xRotationOffset = 0f;
        }

        for (int i = 0; i < horizontalRays; i++)
        {
            PlayerControllerB playerControllerB = CheckForPlayersInLineOfSightHorizontal(range, horizontalRays, angleRangeCheck, xRotationOffset);
            xRotationOffset += LOSXRotationOffsetAmountPerRay;
            if (playerControllerB is null) continue;

            return playerControllerB;
        }

        return null;
    }

    private PlayerControllerB CheckForPlayersInLineOfSightHorizontal(float range, int horizontalRays, bool angleRangeCheck, float xRotationOffset)
    {
        float yRotationAmount = range;
        float yRotationOffset = 0f;

        if (horizontalRays > 1)
        {
            yRotationAmount = range / (horizontalRays - 1);
            yRotationOffset = 0f - range / 2f;
        }

        for (int i = 0; i <= horizontalRays; i++)
        {
            Vector3 forward = aimPoint.forward;
            forward = Quaternion.Euler(xRotationOffset, yRotationOffset + yRotationAmount * i, 0f) * forward;
            shootRay = new Ray(aimTurretCenterPoint.position, forward);

            if (Physics.Raycast(shootRay, out hit, LOSDistance, 1051400, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    PlayerControllerB component = hit.transform.GetComponent<PlayerControllerB>();
                    
                    if (component is not null)
                    {
                        if (angleRangeCheck && Vector3.Angle(component.transform.position + Vector3.up * 1.75f - centerPoint.position, forwardFacingPos.forward) > rotationRange)
                        {
                            return null;
                        }

                        return component;
                    }

                    continue;
                }

                if ((turretMode == TurretMode.Firing || (turretMode == TurretMode.Berserk && !enteringBerserkMode)) && hit.transform.tag.StartsWith("PlayerRagdoll"))
                {
                    Rigidbody component2 = hit.transform.GetComponent<Rigidbody>();

                    if (component2 is not null)
                    {
                        component2.AddForce(forward.normalized * 42f, ForceMode.Impulse);
                    }
                }
            }
        }

        return null;
    }
    #endregion

    #region Networking
    [ClientRpc]
    public void SwitchRotationClientRpc(bool setRotateRight)
    {
        SwitchRotationOnInterval(setRotateRight);
    }

    public void SwitchRotationOnInterval(bool setRotateRight)
    {
        if (rotatingRight)
        {
            rotatingRight = false;
            targetRotation = rotationRange;
        }
        else
        {
            rotatingRight = true;
            targetRotation = 0f - rotationRange;
        }
    }

    [ClientRpc]
    public void SwitchTargetedPlayerClientRpc(int playerId, bool setModeToCharging = false)
    {
        targetPlayerWithRotation = StartOfRound.Instance.allPlayerScripts[playerId];

        if (setModeToCharging)
        {
            SwitchTurretMode(1);
        }
    }

    [ClientRpc]
    public void RemoveTargetedPlayerClientRpc()
    {
        targetPlayerWithRotation = null;
    }

    [ClientRpc]
    public void SetToModeClientRpc(int mode)
    {
        SwitchTurretMode(mode);
    }

    private void SwitchTurretMode(int mode)
    {
        turretMode = (TurretMode)mode;
    }

    public void ToggleTurretEnabled(bool enabled)
    {
        ToggleTurretEnabledLocalClient(enabled);
        ToggleTurretServerRpc(enabled);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ToggleTurretServerRpc(bool enabled)
    {
        ToggleTurretClientRpc(enabled);
    }

    [ClientRpc]
    public void ToggleTurretClientRpc(bool enabled)
    {
        ToggleTurretEnabledLocalClient(enabled);
    }

    private void ToggleTurretEnabledLocalClient(bool enabled)
    {
        if (turretActive == enabled) return;

        turretActive = enabled;
        turretAnimator.SetBool("turretActive", turretActive);

        if (enabled)
        {
            mainAudio.PlayOneShot(turretActivate);
            WalkieTalkie.TransmitOneShotAudio(mainAudio, turretActivate);
        }
        else
        {
            mainAudio.PlayOneShot(turretDeactivate);
            WalkieTalkie.TransmitOneShotAudio(mainAudio, turretDeactivate);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void EnterBerserkModeServerRpc()
    {
        EnterBerserkModeClientRpc();
    }

    [ClientRpc]
    public void EnterBerserkModeClientRpc()
    {
        SwitchTurretMode(3);
    }

    public void SetObjectCodeOnServer()
    {
        if (!Plugin.IsHostOrServer) return;

        int codeIndex = Random.Range(0, RoundManager.Instance.possibleCodesForBigDoors.Length);
        SetObjectCodeClientRpc(codeIndex);
    }

    [ClientRpc]
    public void SetObjectCodeClientRpc(int codeIndex)
    {
        FollowTerminalAccessibleObjectBehaviour behaviour = gameObject.GetComponentInChildren<FollowTerminalAccessibleObjectBehaviour>();
        if (behaviour == null) return;

        behaviour.InitializeValues();
        behaviour.SetCodeTo(codeIndex);
    }
    #endregion

    #region Debugging
    private void OnDrawGizmosSelected()
    {
        DrawLineOfSightLines();
    }

    private void DrawLineOfSightLines()
    {
        if (aimPoint is null || aimTurretCenterPoint is null) return;

        DrawLineOfSightLinesVertical();
    }

    private void DrawLineOfSightLinesVertical()
    {
        float xRotationOffset = LOSXRotationOffsetStart;

        if (LOSVerticalRays < 2)
        {
            xRotationOffset = 0f;
        }

        for (int i = 0; i < LOSVerticalRays; i++)
        {
            DrawLineOfSightLinesHorizontal(xRotationOffset);
            xRotationOffset += LOSXRotationOffsetAmountPerRay;
        }
    }

    private void DrawLineOfSightLinesHorizontal(float xRotationOffset)
    {
        Gizmos.color = Color.red;

        Vector3 position = aimTurretCenterPoint.position;
        float yRotationAmount = LOSRange;
        float yRotationOffset = 0f;

        if (LOSHorizontalRays > 1)
        {
            yRotationAmount = LOSRange / (LOSHorizontalRays - 1);
            yRotationOffset = 0f - LOSRange / 2f;
        }

        for (int i = 0; i < LOSHorizontalRays; i++)
        {
            Vector3 forward = aimPoint.forward;
            forward = Quaternion.Euler(xRotationOffset, yRotationOffset + yRotationAmount * i, 0f) * forward;

            Gizmos.DrawLine(position, position + new Ray(position, forward).direction * LOSDistance);
        }
    }
    #endregion
}
