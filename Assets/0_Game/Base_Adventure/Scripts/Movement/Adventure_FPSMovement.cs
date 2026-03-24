using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.Events;

public class Adventure_FPSMovement : BaseMovementBloxState
{
    [Header("Movement Settings")]
    public float MaxMoveSpeed = 7f;
    public float MovementSharpness = 15f;

    [Header("Rotation Settings")]
    public float RotationSharpness = 300f;

    [Header("Jumping")]
    public float JumpUpSpeed = 10f;
    public float JumpPostGroundingGraceTime = 0.2f;

    [Header("Misc")]
    public Vector3 Gravity = new Vector3(0, -30f, 0);
    public float Drag = 0.1f;

    protected Vector3 _moveInputVector;
    protected bool _jumpRequested = false;
    protected bool _jumpConsumed = false;
    protected bool _jumpedThisFrame = false;
    protected float _timeSinceJumpRequested = Mathf.Infinity;
    protected float _timeSinceLastAbleToJump = 0f;
    private Vector3 _internalVelocityAdd = Vector3.zero;

    public string PlanarSpeedParameter = "PlanarSpeed";
    public string VerticalSpeedParameter = "VerticalSpeed";
    public string IsGroundedParameter = "Grounded";
    public UnityAction OnJumpPerformed;

    public CinemachineCameraTouchController camTouch;

    public override void OnStateUpdate(float dt)
    {
        Vector3 cameraForward = Camera.main.transform.forward;

        Vector3 planarDirection = Vector3.ProjectOnPlane(cameraForward, Vector3.up).normalized;

        InputHolder.ExternalPlanarDirection = planarDirection;
        Vector3 rawMoveInput = InputHolder.moveInputVector;
        rawMoveInput.y = 0;
        _moveInputVector = rawMoveInput;


        if (InputHolder.JumpDown)
        {
            _timeSinceJumpRequested = 0f;
            _jumpRequested = true;
            InputHolder.JumpDown = false;
        }
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }
    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        float yawInput = camTouch.GetAxisCustom("Mouse X");

        float rotationAmount = yawInput * RotationSharpness * deltaTime;

        Quaternion yawRotation = Quaternion.Euler(0f, rotationAmount, 0f);

        currentRotation = yawRotation * currentRotation;
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        _moveInputVector = Vector3.ClampMagnitude(_moveInputVector, 1f);

        Vector3 targetMovementDirection = (Motor.CharacterForward * _moveInputVector.z) + (Motor.CharacterRight * _moveInputVector.x);

        if (Motor.GroundingStatus.IsStableOnGround)
        {
            float currentVelocityMagnitude = currentVelocity.magnitude;
            Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

            Vector3 inputRight = Vector3.Cross(targetMovementDirection, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * targetMovementDirection.magnitude;
            Vector3 targetMovementVelocity = reorientedInput * MaxMoveSpeed;

            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-MovementSharpness * deltaTime));
        } 
        else
        {
            if (targetMovementDirection.sqrMagnitude > 0f)
            {
                Vector3 addedVelocity = targetMovementDirection * MovementSharpness * deltaTime;
                Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                if (currentVelocityOnInputsPlane.magnitude < MaxMoveSpeed)
                {
                    Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, MaxMoveSpeed);
                    addedVelocity = newTotal - currentVelocityOnInputsPlane;
                }
                else if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                {
                    addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                }

                currentVelocity += addedVelocity;
            }
            currentVelocity += Gravity * deltaTime;
            currentVelocity *= (1f / (1f + (Drag * deltaTime)));
        }

        _timeSinceJumpRequested += deltaTime;
        if (_jumpRequested && !_jumpConsumed && (Motor.GroundingStatus.IsStableOnGround || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime))
        {
            Vector3 jumpDirection = Motor.CharacterUp;
            Motor.ForceUnground();
            OnJumpPerformed?.Invoke();
            currentVelocity += (jumpDirection * JumpUpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
            _jumpRequested = false;
            _jumpConsumed = true;
            _jumpedThisFrame = true;
        }

        if (_internalVelocityAdd.sqrMagnitude > 0f)
        {
            currentVelocity += _internalVelocityAdd;
            _internalVelocityAdd = Vector3.zero;
        }
    }

    public void AddVelocity(Vector3 velocity)
    { 
        _internalVelocityAdd += velocity;
    }

    public override void AfterCharacterUpdate(float deltaTime)
    {
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
    }

    public override void OnStateExit()
    {
    }
}
    
