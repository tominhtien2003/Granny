using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.Events;
namespace BloxLikeBasic
{
    public class NormalMovement : BaseMovementBloxState
    {
        [Header("Stable Movement")]
        public float MaxStableMoveSpeed = 10f;
        public float StableMovementSharpness = 15f;
        public float OrientationSharpness = 10f;
        public OrientationMethod OrientationMethod = OrientationMethod.TowardsCamera;

        [Header("Air Movement")]
        public float MaxAirMoveSpeed = 15f;
        public float AirAccelerationSpeed = 15f;
        public float Drag = 0.1f;

        [Header("Jumping")]
        public bool AllowJumpingWhenSliding = false;
        public float JumpUpSpeed = 10f;
        public float JumpScalableForwardSpeed = 10f;
        public float JumpPreGroundingGraceTime = 0f;
        public float JumpPostGroundingGraceTime = 0f;

        [Header("Misc")]

        public BonusOrientationMethod BonusOrientationMethod = BonusOrientationMethod.None;
        public float BonusOrientationSharpness = 10f;
        public Vector3 Gravity = new Vector3(0, -30f, 0);
        public float CrouchedCapsuleHeight = 1f;

        protected Vector3 _moveInputVector;
        protected Vector3 _lookInputVector;
        protected bool _jumpRequested = false;
        protected bool _jumpConsumed = false;
        protected bool _jumpedThisFrame = false;
        protected float _timeSinceJumpRequested = Mathf.Infinity;
        protected float _timeSinceLastAbleToJump = 0f;
        private Vector3 _internalVelocityAdd = Vector3.zero;


        [Header("Animator Parameters")]
        public string PlanarSpeedParameter = "PlanarSpeed";
        public string VerticalSpeedParameter = "VerticalSpeed";
        public string IsGroundedParameter = "Grounded";

        public UnityAction OnJumpPerformed;

        private bool _canMove = true;
        public void SetControl(bool move) => _canMove = move;
        public override void BeforeCharacterUpdate(float deltaTime)
        {
            m_blackboard.Animator.SetBool(IsGroundedParameter, m_blackboard.Motor.GroundingStatus.IsStableOnGround);
        }
        public override void AfterCharacterUpdate(float deltaTime)
        {
            // Handle jump-related values
            {
                // Handle jumping pre-ground grace period
                if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
                {
                    _jumpRequested = false;
                }

                if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
                {
                    // If we're on a ground surface, reset jumping values
                    if (!_jumpedThisFrame)
                    {
                        _jumpConsumed = false;
                    }
                    _timeSinceLastAbleToJump = 0f;
                }
                else
                {
                    // Keep track of time since we were last able to jump (for grace period)
                    _timeSinceLastAbleToJump += deltaTime;
                }
            }

            m_blackboard.Animator.SetFloat(VerticalSpeedParameter, Motor.BaseVelocity.y);
            m_blackboard.Animator.SetFloat(PlanarSpeedParameter, Motor.BaseVelocity.magnitude);

        }

        public override void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AddVelocity(Vector3 velocity)
        {

            _internalVelocityAdd += velocity;

        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _lookInputVector = Vector3.zero;
            UpdateInput();
        }
        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate(float dt)
        {
            UpdateInput();
        }
        void UpdateInput()
        {
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(InputHolder.ExternalPlanarDirection.normalized, Motor.CharacterUp);
            Vector3 m = InputHolder.moveInputVector;
            m.y = 0;
            _moveInputVector = cameraPlanarRotation * m;
            //Debug.Log("Move Input: " + _moveInputVector);
            switch (OrientationMethod)
            {
                case OrientationMethod.TowardsCamera:
                    _lookInputVector = InputHolder.ExternalPlanarDirection;
                    break;
                case OrientationMethod.TowardsMovement:
                    _lookInputVector = _moveInputVector.normalized;
                    break;
            }
            _lookInputVector = Vector3.ProjectOnPlane(_lookInputVector, Vector3.up);

            // Jumping input
            if (InputHolder.JumpDown)
            {
                _timeSinceJumpRequested = 0f;
                _jumpRequested = true;
                InputHolder.JumpDown = false;
            }
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_lookInputVector.sqrMagnitude > 0.1f && OrientationSharpness > 0f)
            {
                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(m_blackboard.Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, m_blackboard.Motor.CharacterUp);
            }

            Vector3 currentUp = currentRotation * Vector3.up;
            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            // Ground movement
            _moveInputVector = Vector3.ClampMagnitude(_moveInputVector, 1f);
           
            

            if (Motor.GroundingStatus.IsStableOnGround)
            {
                float currentVelocityMagnitude = currentVelocity.magnitude;

                Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                // Reorient velocity on slope
                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                Vector3 targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-StableMovementSharpness * deltaTime));
                //Debug.Log("target movement " + targetMovementVelocity + " " + _moveInputVector);
            }
            // Air movement
            else
            {
                // Add move input

                /*                if (_moveInputVector.sqrMagnitude > 0f)
                                {
                                    Vector3 addedVelocity = _moveInputVector * AirAccelerationSpeed * deltaTime;

                                    Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                                    // Limit air velocity from inputs
                                    if (currentVelocityOnInputsPlane.magnitude < MaxAirMoveSpeed)
                                    {
                                        // clamp addedVel to make total vel not exceed max vel on inputs plane
                                        Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, MaxAirMoveSpeed);
                                        addedVelocity = newTotal - currentVelocityOnInputsPlane;
                                    }
                                    else
                                    {
                                        // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                                        if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                                        {
                                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                                        }
                                    }

                                    // Prevent air-climbing sloped walls
                                    if (Motor.GroundingStatus.FoundAnyGround)
                                    {
                                        if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                                        {
                                            Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                                        }
                                    }

                                    // Apply added velocity
                                    currentVelocity += addedVelocity;


                                }

                                // Gravity
                                currentVelocity += Gravity * deltaTime;

                                // Drag
                                currentVelocity *= (1f / (1f + (Drag * deltaTime)));*/

                //V2
                Vector3 targetMovementVelocity = Vector3.zero;
                float y = currentVelocity.y;
                currentVelocity.y = 0;
                targetMovementVelocity = _moveInputVector * MaxAirMoveSpeed;          
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-AirAccelerationSpeed * deltaTime));

                //Gravity
                y += Gravity.y * deltaTime;
                currentVelocity.y = y;
            }
            
            // Handle jumping
            _jumpedThisFrame = false;
            _timeSinceJumpRequested += deltaTime;
            if (_jumpRequested)
            {
                // See if we actually are allowed to jump
                if (!_jumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime))
                {
                    // Calculate jump direction before ungrounding
                    Vector3 jumpDirection = Motor.CharacterUp;
                    if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                    {
                        jumpDirection = Motor.GroundingStatus.GroundNormal;
                    }

                    // Makes the character skip ground probing/snapping on its next update. 
                    // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                    Motor.ForceUnground();
                    OnJumpPerformed?.Invoke();
                    // Add to the return velocity and reset jump state
                    currentVelocity += (jumpDirection * JumpUpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                    currentVelocity += (_moveInputVector * JumpScalableForwardSpeed);
                    _jumpRequested = false;
                    _jumpConsumed = true;
                    _jumpedThisFrame = true;
                }
            }

            // Take into account additive velocity
            if (_internalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += _internalVelocityAdd;
                //Debug.Log("adding " + _internalVelocityAdd);
                _internalVelocityAdd = Vector3.zero;
            }
            //Debug.Log("Velocity: " + Motor.BaseVelocity);
        }
       


    }
}
