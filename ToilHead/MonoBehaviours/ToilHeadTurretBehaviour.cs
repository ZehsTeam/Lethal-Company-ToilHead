using GameNetcodeStuff;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ToilHeadTurretBehaviour : NetworkBehaviour
{
    [Header("Audio Sources")]
    [Space(5f)]
    public AudioSource mainAudio = null;
    public AudioSource farAudio = null;
    public AudioSource bulletCollisionAudio = null;
    public AudioSource berserkAudio = null;

    [Header("Audio Clips")]
    [Space(5f)]
    public AudioClip firingSFX = null;
    public AudioClip chargingSFX = null;
    public AudioClip detectPlayerSFX = null;
    public AudioClip firingFarSFX = null;
    public AudioClip bulletsHitWallSFX = null;
    public AudioClip turretActivate = null;
    public AudioClip turretDeactivate = null;

    [Header("Transforms")]
    [Space(5f)]
    public Transform turretRod = null;
    public Transform turnTowardsObjectCompass = null;
    public Transform forwardFacingPos = null;
    public Transform aimPoint = null;
    public Transform aimTurretCenterPoint = null;
    public Transform centerPoint = null;
    public Transform tempTransform = null;
    [HideInInspector] public Transform targetTransform;

    [Header("Turret Properties")]
    [Space(5f)]
    public Animator turretAnimator = null;
    public ParticleSystem bulletParticles = null;
    public float rotationRange = 75f;
    public float chargingDelay = 0.5f;
    [HideInInspector] public TurretMode turretMode = TurretMode.Detection;
    [HideInInspector] public bool turretActive = true;
    [HideInInspector] public float targetRotation;
    [HideInInspector] public float rotationSpeed = 28f;
    [HideInInspector] public PlayerControllerB targetPlayerWithRotation;

    [Header("Line Of Sight")]
    [Space(5f)]
    public float LOSRange = 30f;
    public float LOSDistance = 30f;
    public int LOSVerticalRays = 3;
    public int LOSHorizontalRays = 7;
    public float LOSXRotationOffsetStart = -5;
    public float LOSXRotationOffsetAmountPerRay = 5f;

    [Header("Turrer Head Properties")]
    [Space(5f)]
    public bool IsMinigun = false;
    public Transform SyncToHeadTransform = null;

    #region Private Variables
    private TurretMode _turretModeLastFrame;
    private bool _targetingDeadPlayer;
    private bool _rotatingRight;
    private float _switchRotationTimer;
    private bool _hasLineOfSight;
    private float _lostLOSTimer;
    private RaycastHit _hit;
    private bool _wasTargetingPlayerLastFrame;
    private float _turretInterval;
    private bool _rotatingSmoothly = true;
    private Ray _shootRay;
    private Coroutine _fadeBulletAudioCoroutine;
    private bool _rotatingClockwise;
    private float _berserkTimer;
    private bool _enteringBerserkMode;
    private float _chargingTimer = 0f;
    private bool _playedChargingSFX = false;
    private int _damage = 25;
    private float _damageRate = 0.21f;
    #endregion

    #region Custom Variables
    [HideInInspector] public bool CanTargetPlayers = true;
    [HideInInspector] public bool UseMantiToilSettings = false;

    // Turret Settings
    [HideInInspector] public float LostLOSDuration = 0.75f;

    // Turret Detection Settings
    [HideInInspector] public bool DetectionRotation = false;
    [HideInInspector] public float DetectionRotationSpeed = 28f;

    // Turret Charging Settings
    [HideInInspector] public float ChargingDuration = 2f;
    [HideInInspector] public float ChargingRotationSpeed = 95f;

    // Turret Firing Settings
    [HideInInspector] public float FiringRotationSpeed = 95f;

    // Turret Berserk Settings
    [HideInInspector] public float BerserkDuration = 9f;
    [HideInInspector] public float BerserkRotationSpeed = 77f;
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
        SyncedConfigManager configManager = Plugin.ConfigManager;

        // Turret Settings
        LostLOSDuration = configManager.TurretLostLOSDuration.Value;
        rotationRange = Mathf.Abs(configManager.TurretRotationRange.Value);

        if (UseMantiToilSettings)
        {
            LostLOSDuration = 5f;
        }

        if (IsMinigun && !UseMantiToilSettings)
        {
            LostLOSDuration = 3f;
        }

        // Turret Detection Settings
        DetectionRotation = configManager.TurretDetectionRotation.Value;
        DetectionRotationSpeed = configManager.TurretDetectionRotationSpeed.Value;

        // Turret Charging Settings
        ChargingDuration = configManager.TurretChargingDuration.Value;
        ChargingRotationSpeed = configManager.TurretChargingRotationSpeed.Value;

        if (IsMinigun)
        {
            ChargingDuration = detectPlayerSFX.length + chargingDelay + chargingSFX.length;
            ChargingRotationSpeed *= 0.5f;
        }

        // Turret Firing Settings
        FiringRotationSpeed = configManager.TurretFiringRotationSpeed.Value;

        if (IsMinigun)
        {
            FiringRotationSpeed *= 0.5f;
        }

        _damage = IsMinigun ? 15 : 25;
        _damageRate = IsMinigun ? 0.105f : 0.21f;

        // Turret Berserk Settings
        BerserkDuration = configManager.TurretBerserkDuration.Value;
        BerserkRotationSpeed = configManager.TurretBerserkRotationSpeed.Value;

        if (IsMinigun)
        {
            BerserkDuration *= 2f;
            BerserkRotationSpeed *= 0.5f;
        }

        rotationSpeed = DetectionRotationSpeed;

        if (UseMantiToilSettings && !IsMinigun)
        {
            LOSVerticalRays = 6;
        }

        if (IsMinigun && !IsOnEnemy())
        {
            LOSRange = 30f;
            LOSVerticalRays = 3;
            LOSXRotationOffsetStart = -5f;
            LOSXRotationOffsetAmountPerRay = 5f;
        }
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
        if (!turretActive || !CanTargetPlayers)
        {
            _wasTargetingPlayerLastFrame = false;
            turretMode = TurretMode.Detection;
            targetPlayerWithRotation = null;
            return false;
        }

        if (targetPlayerWithRotation != null)
        {
            if (!_wasTargetingPlayerLastFrame)
            {
                _wasTargetingPlayerLastFrame = true;
                if (turretMode == TurretMode.Detection)
                {
                    turretMode = TurretMode.Charging;
                }
            }
            SetTargetToPlayerBody();
            TurnTowardsTargetIfHasLOS();
        }
        else if (_wasTargetingPlayerLastFrame)
        {
            _wasTargetingPlayerLastFrame = false;
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
        if (_turretModeLastFrame != TurretMode.Detection)
        {
            _turretModeLastFrame = TurretMode.Detection;
            _rotatingClockwise = false;
            mainAudio.Stop();
            farAudio.Stop();
            berserkAudio.Stop();
            if (_fadeBulletAudioCoroutine != null)
            {
                StopCoroutine(_fadeBulletAudioCoroutine);
            }
            _fadeBulletAudioCoroutine = StartCoroutine(FadeBulletAudio());
            bulletParticles.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
            rotationSpeed = DetectionRotationSpeed;
            _rotatingSmoothly = true;
            if (turretAnimator != null)
            {
                turretAnimator.SetInteger("TurretMode", 0);
            }
            _turretInterval = Random.Range(0f, 0.15f);
        }
        if (!IsServer)
        {
            return;
        }
        if (_switchRotationTimer >= 7f)
        {
            _switchRotationTimer = 0f;
            bool setRotateRight = !_rotatingRight;
            SwitchRotationClientRpc(setRotateRight);
        }
        else
        {
            _switchRotationTimer += Time.deltaTime;
        }
        if (_turretInterval >= 0.25f)
        {
            _turretInterval = 0f;
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
            _turretInterval += Time.deltaTime;
        }
    }

    private void TurretModeChargingLogic()
    {
        if (_turretModeLastFrame != TurretMode.Charging)
        {
            _turretModeLastFrame = TurretMode.Charging;
            _rotatingClockwise = false;
            mainAudio.PlayOneShot(detectPlayerSFX);
            berserkAudio.Stop();
            WalkieTalkie.TransmitOneShotAudio(mainAudio, detectPlayerSFX);
            rotationSpeed = ChargingRotationSpeed;
            _rotatingSmoothly = false;
            _lostLOSTimer = 0f;
            if (turretAnimator != null)
            {
                turretAnimator.SetInteger("TurretMode", 1);
            }
            _chargingTimer = 0f;
            _playedChargingSFX = false;
        }

        if (IsMinigun && _chargingTimer > detectPlayerSFX.length && !_playedChargingSFX)
        {
            mainAudio.PlayOneShot(chargingSFX);
            WalkieTalkie.TransmitOneShotAudio(mainAudio, chargingSFX);
            _playedChargingSFX = true;
        }

        _chargingTimer += Time.deltaTime;

        if (!IsServer)
        {
            return;
        }
        if (_turretInterval >= ChargingDuration)
        {
            _turretInterval = 0f;
            Plugin.Instance.LogInfoExtended("Charging timer is up, setting to firing mode.");
            if (!_hasLineOfSight)
            {
                Plugin.Instance.LogInfoExtended("hasLineOfSight is false");
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
            _turretInterval += Time.deltaTime;
        }
    }

    private void TurretModeFiringLogic()
    {
        if (_turretModeLastFrame != TurretMode.Firing)
        {
            _turretModeLastFrame = TurretMode.Firing;
            berserkAudio.Stop();
            mainAudio.clip = firingSFX;
            mainAudio.Play();
            farAudio.clip = firingFarSFX;
            farAudio.Play();
            bulletParticles.Play(withChildren: true);
            bulletCollisionAudio.Play();
            if (_fadeBulletAudioCoroutine != null)
            {
                StopCoroutine(_fadeBulletAudioCoroutine);
            }
            bulletCollisionAudio.volume = 1f;
            rotationSpeed = FiringRotationSpeed;
            _rotatingSmoothly = false;
            _lostLOSTimer = 0f;
            if (turretAnimator != null)
            {
                turretAnimator.SetInteger("TurretMode", 2);
            }
        }

        if (_turretInterval >= _damageRate)
        {
            PlayerControllerB localPlayerScript = PlayerUtils.GetLocalPlayerScript();

            _turretInterval = 0f;
            if (CheckForPlayersInLineOfSight(range: 5, verticalRays: 1) == localPlayerScript)
            {
                if (localPlayerScript.health > _damage)
                {
                    localPlayerScript.DamagePlayer(_damage, hasDamageSFX: true, callRPC: true, CauseOfDeath.Gunshots, 2);
                }
                else
                {
                    localPlayerScript.KillPlayer(aimPoint.forward * 40f, spawnBody: true, CauseOfDeath.Gunshots, 2);

                    TurretHeadManager.SetDeadBodyTurretHead(localPlayerScript, IsMinigun);
                }
            }
            _shootRay = new Ray(aimPoint.position, aimPoint.forward);
            if (Physics.Raycast(_shootRay, out _hit, 30f, StartOfRound.Instance.collidersAndRoomMask, QueryTriggerInteraction.Ignore))
            {
                bulletCollisionAudio.transform.position = _shootRay.GetPoint(_hit.distance - 0.5f);
            }
        }
        else
        {
            _turretInterval += Time.deltaTime;
        }
    }

    private void TurretModeBerserkLogic()
    {
        if (_turretModeLastFrame != TurretMode.Berserk)
        {
            _turretModeLastFrame = TurretMode.Berserk;
            if (turretAnimator != null)
            {
                turretAnimator.SetInteger("TurretMode", 1);
            }
            _berserkTimer = 1.3f;
            berserkAudio.Play();
            rotationSpeed = BerserkRotationSpeed;
            _enteringBerserkMode = true;
            _rotatingSmoothly = true;
            _lostLOSTimer = 0f;
            _wasTargetingPlayerLastFrame = false;
            targetPlayerWithRotation = null;
        }
        if (_enteringBerserkMode)
        {
            _berserkTimer -= Time.deltaTime;
            if (_berserkTimer <= 0f)
            {
                _enteringBerserkMode = false;
                _rotatingClockwise = true;
                _berserkTimer = BerserkDuration;
                if (turretAnimator != null)
                {
                    turretAnimator.SetInteger("TurretMode", 2);
                }
                mainAudio.clip = firingSFX;
                mainAudio.Play();
                farAudio.clip = firingFarSFX;
                farAudio.Play();
                bulletParticles.Play(withChildren: true);
                bulletCollisionAudio.Play();
                if (_fadeBulletAudioCoroutine != null)
                {
                    StopCoroutine(_fadeBulletAudioCoroutine);
                }
                bulletCollisionAudio.volume = 1f;
            }
            return;
        }
        if (_turretInterval >= _damageRate)
        {
            PlayerControllerB localPlayerScript = PlayerUtils.GetLocalPlayerScript();

            _turretInterval = 0f;
            if (CheckForPlayersInLineOfSight(range: 5, verticalRays: 1) == localPlayerScript)
            {
                if (localPlayerScript.health > _damage)
                {
                    localPlayerScript.DamagePlayer(_damage, hasDamageSFX: true, callRPC: true, CauseOfDeath.Gunshots, 2);
                }
                else
                {
                    localPlayerScript.KillPlayer(aimPoint.forward * 40f, spawnBody: true, CauseOfDeath.Gunshots, 2);

                    TurretHeadManager.SetDeadBodyTurretHead(localPlayerScript, IsMinigun);
                }
            }
            _shootRay = new Ray(aimPoint.position, aimPoint.forward);
            if (Physics.Raycast(_shootRay, out _hit, 30f, StartOfRound.Instance.collidersAndRoomMask, QueryTriggerInteraction.Ignore))
            {
                bulletCollisionAudio.transform.position = _shootRay.GetPoint(_hit.distance - 0.5f);
            }
        }
        else
        {
            _turretInterval += Time.deltaTime;
        }
        if (IsServer)
        {
            _berserkTimer -= Time.deltaTime;
            if (_berserkTimer <= 0f || !turretActive)
            {
                SwitchTurretMode(0);
                SetToModeClientRpc(0);
            }
        }
    }
    #endregion

    private bool RotateTurretRod()
    {
        if (_rotatingClockwise)
        {
            turnTowardsObjectCompass.localEulerAngles = new Vector3(25f, turretRod.localEulerAngles.y - Time.deltaTime * rotationSpeed, 0f);
            turretRod.localRotation = Quaternion.RotateTowards(turretRod.localRotation, turnTowardsObjectCompass.localRotation, rotationSpeed * Time.deltaTime);
            return false;
        }

        if (_rotatingSmoothly)
        {
            if (DetectionRotation)
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
            if (!_targetingDeadPlayer)
            {
                _targetingDeadPlayer = true;
            }
            if (targetPlayerWithRotation.deadBody != null)
            {
                targetTransform = targetPlayerWithRotation.deadBody.bodyParts[5].transform;
            }
        }
        else
        {
            _targetingDeadPlayer = false;
            targetTransform = targetPlayerWithRotation.gameplayCamera.transform;
        }
    }

    private void TurnTowardsTargetIfHasLOS()
    {
        bool flag = true;
        if (_targetingDeadPlayer || Vector3.Angle(targetTransform.position - centerPoint.position, forwardFacingPos.forward) > rotationRange)
        {
            flag = false;
        }
        if (Physics.Linecast(aimPoint.position, targetTransform.position, StartOfRound.Instance.collidersAndRoomMask, QueryTriggerInteraction.Ignore))
        {
            flag = false;
        }

        if (flag)
        {
            _hasLineOfSight = true;
            _lostLOSTimer = 0f;
            tempTransform.position = targetTransform.position;
            tempTransform.position -= Vector3.up * 0.15f;
            turnTowardsObjectCompass.LookAt(tempTransform);
            return;
        }

        if (_hasLineOfSight)
        {
            _hasLineOfSight = false;
            _lostLOSTimer = 0f;
        }

        if (!IsServer) return;

        _lostLOSTimer += Time.deltaTime;

        if (_lostLOSTimer >= LostLOSDuration)
        {
            _lostLOSTimer = 0f;
            Plugin.Instance.LogInfoExtended("LOS timer ended on server. checking for new player target.");
            PlayerControllerB playerControllerB = CheckForPlayersInLineOfSight();
            if (playerControllerB != null)
            {
                targetPlayerWithRotation = playerControllerB;
                SwitchTargetedPlayerClientRpc((int)playerControllerB.playerClientId);
                Plugin.Instance.LogInfoExtended("Got new player target.");
            }
            else
            {
                Plugin.Instance.LogInfoExtended("No new player to target; returning to detection mode.");
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
            _shootRay = new Ray(aimTurretCenterPoint.position, forward);

            if (Physics.Raycast(_shootRay, out _hit, LOSDistance, 1051400, QueryTriggerInteraction.Ignore))
            {
                if (_hit.transform.CompareTag("Player"))
                {
                    PlayerControllerB component = _hit.transform.GetComponent<PlayerControllerB>();

                    if (TurretHeadManager.IsTurretHead(component)) return null;

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

                if ((turretMode == TurretMode.Firing || (turretMode == TurretMode.Berserk && !_enteringBerserkMode)) && _hit.transform.tag.StartsWith("PlayerRagdoll"))
                {
                    Rigidbody component2 = _hit.transform.GetComponent<Rigidbody>();

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
        if (_rotatingRight)
        {
            _rotatingRight = false;
            targetRotation = rotationRange;
        }
        else
        {
            _rotatingRight = true;
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
        if (turretAnimator != null)
        {
            turretAnimator.SetBool("turretActive", turretActive);
        }

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
        if (!CanTargetPlayers) return;

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

    [ClientRpc]
    public void SetCanTargetPlayersClientRpc(bool value)
    {
        CanTargetPlayers = value;
    }
    #endregion

    private bool IsOnEnemy()
    {
        return transform.parent.GetComponent<EnemyAI>() != null;
    }

    #region Debugging
    private void OnDrawGizmosSelected()
    {
        DrawLineOfSightLines();
    }

    private void DrawLineOfSightLines()
    {
        if (aimPoint == null || aimTurretCenterPoint == null) return;

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
