using UnityEngine;
using UnityEngine.Events;

public class Jetpack : BaseMovementBloxState
{
    [Header("Jetpack Settings")]
    public float heightIncrement = 1f;
    public float verticalSpeed = 5f;
    public float planarSpeed = 20f;
    public float smoothing = 0.1f;
    public float currentFlyHeight;

    [Header("Jetpack Events")]
    public UnityEvent onJetpackStarted;
    public UnityEvent onJetpackUpdate;
    public UnityEvent onJetpackEnded;

    public bool canJetpack = false;

    private bool isFlying = false;
    private float targetHeight = 0f;
    private float currentVerticalSpeed = 0f;

    protected Vector3 _moveInputVector;
    protected Vector3 _lookInputVector;
    public float BrakingSharpness = 25f;
    public float StableMovementSharpness = 15f;
    public float LowSpeedAccelerationSharpness = 0.5f;
    public float LowSpeedThreshold = 1.0f;

    public float MaxLeanAngle = 30f;
    public float LeanSharpness = 8f;
    public float OrientationSharpness = 10f;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _lookInputVector = Vector3.zero;
        _moveInputVector = Vector3.zero;


        isFlying = true;
        currentFlyHeight = 0f;
        targetHeight = 0f;
        currentVerticalSpeed = 0f;

        Motor.ForceUnground();
        onJetpackStarted?.Invoke();
    }

    public override void OnStateExit()
    {
        isFlying = false;
        currentVerticalSpeed = 0f;
        currentFlyHeight = 0f;
        targetHeight = 0f;

        onJetpackEnded?.Invoke();
    }

    public void ResetJetpack()
    {
        isFlying = false;
        currentVerticalSpeed = 0f;
        currentFlyHeight = 0f;
        targetHeight = 0f;
        canJetpack = false;
    }

    public bool CanEnterJetpack()
    {
        return canJetpack;
    }

    public bool CheckExitJetpack()
    {
        return !canJetpack;
    }

    public void OnStateFixUpdate(float dt)
    {
        if (canJetpack)
        {
            isFlying = true;

            float heightDifference = targetHeight - currentFlyHeight;

            if (Mathf.Abs(heightDifference) > 0.01f)
            {
                currentVerticalSpeed = Mathf.Sign(heightDifference) * verticalSpeed;
                currentFlyHeight += currentVerticalSpeed * dt;
            }
            else
            {
                currentFlyHeight = targetHeight;
                currentVerticalSpeed = 0f;
            }

            if (Motor.GroundingStatus.IsStableOnGround)
            {
                Motor.ForceUnground();
            }
        }
        else
        {
            targetHeight = 0f;
            currentFlyHeight -= verticalSpeed * dt;
            currentVerticalSpeed = -verticalSpeed;

            if (Motor.GroundingStatus.IsStableOnGround || currentFlyHeight <= 0f)
            {
                currentFlyHeight = 0f;
                currentVerticalSpeed = 0f;
                targetHeight = 0f;
                isFlying = false;
            }
        }
    }

    public void OnJetpackUpButtonPressed()
    {
        if (!isFlying || !canJetpack) return;

        targetHeight += heightIncrement;
    }

    public void OnJetpackDownButtonPressed()
    {
        if (!isFlying || !canJetpack) return;

        targetHeight -= heightIncrement;
    }

    public void FixedUpdate()
    {
        OnStateFixUpdate(Time.deltaTime);
    }

    public override void OnStateUpdate(float dt)
    {
        onJetpackUpdate?.Invoke();

        Quaternion cameraPlanarRotation = Quaternion.LookRotation(InputHolder.ExternalPlanarDirection, Motor.CharacterUp);
        _moveInputVector = cameraPlanarRotation * InputHolder.moveInputVector;
        _lookInputVector = _moveInputVector.normalized;
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (!isFlying)
        {
            return;
        }

        float currentVelocityMagnitude = currentVelocity.magnitude;

        Vector3 effectiveGroundNormal = Vector3.up;

        currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

        Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
        Vector3 inp = Vector3.Cross(effectiveGroundNormal, inputRight);
        Vector3 reorientedInput = inp.normalized * _moveInputVector.magnitude;

        Vector3 targetMovementVelocity = reorientedInput * planarSpeed;

        float finalSharpness = StableMovementSharpness;

        if (targetMovementVelocity.sqrMagnitude < 0.01f)
        {
            finalSharpness = BrakingSharpness;
        }

        currentVelocity = Vector3.Lerp(
            currentVelocity,
            targetMovementVelocity,
            1f - Mathf.Exp(-finalSharpness * deltaTime)
        );

        Vector3 verticalVelocity = currentVerticalSpeed * Motor.CharacterUp;
        currentVelocity = currentVelocity + verticalVelocity;
    }

    public string IsGroundedParameter = "Grounded";
    public float BonusOrientationSharpness = 10f;

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (_lookInputVector.sqrMagnitude > 0.1f && OrientationSharpness > 0f)
        {
            Vector3 smoothedLookInputDirection = Vector3.Slerp(m_blackboard.Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;
            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, m_blackboard.Motor.CharacterUp);
        }

        Vector3 currentUp = currentRotation * Vector3.up;
        Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
        m_blackboard.Animator.SetBool(IsGroundedParameter, m_blackboard.Motor.GroundingStatus.IsStableOnGround);
    }

    public override void AfterCharacterUpdate(float deltaTime)
    {
    }
}
