using BloxLikeBasic;
using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchWall_NormalMovement : BaseMovementBloxState
{
    public Vector3 m_startDir;
    public Vector3 m_endDir;
    public PunchWallScript m_punchWallScript;
    public NormalMovement m_normalMovementStats;

    private Vector3 m_diff;
    public bool AutoPunch = false;

    Vector3 _moveInputVector;
    Vector3 _lookInputVector;
    Vector3 _internalVelocityAdd;

    public float m_finishCooldown = 1f;
    private float m_currentCooldown = 0;
    private bool m_onCooldown = false;
    public CupScript m_cup;
    private void Awake()
    {
        m_cup = FindObjectOfType<CupScript>();
    }
    public override void OnStateUpdate(float dt)
    {
        if (m_onCooldown)
        {
            m_currentCooldown += dt;
            if (m_currentCooldown >= m_finishCooldown)
            {
                m_currentCooldown = 0;
                m_onCooldown = false;
            }
            return;
        }
        _moveInputVector = m_endDir - Motor.TransientPosition;
        _moveInputVector.y = 0;
        _moveInputVector.Normalize();
        _lookInputVector = _moveInputVector;
        if (m_punchWallScript.m_currentWallId < PunchWall_GlobalStatusHolder.Instance.WallHps.Count && 
            Mathf.Abs(Motor.TransientPosition.x - m_punchWallScript.m_wall.transform.position.x) < 1f)
            {
                m_punchWallScript.Punch();
            }
        
        
        
        if (Vector3.SqrMagnitude(m_endDir - Motor.TransientPosition) < 4f)
        {
            m_cup.TakeCup();
            m_punchWallScript.ReturnToLobby();
            Motor.BaseVelocity = Vector3.zero;
            
            m_onCooldown = true;
            return;
        }
    }

    public override void AfterCharacterUpdate(float deltaTime)
    {
        m_blackboard.Animator.SetBool(m_normalMovementStats.IsGroundedParameter, m_blackboard.Motor.GroundingStatus.IsStableOnGround);
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
        m_blackboard.Animator.SetFloat(m_normalMovementStats.VerticalSpeedParameter, Motor.BaseVelocity.y);
        m_blackboard.Animator.SetFloat(m_normalMovementStats.PlanarSpeedParameter, Motor.BaseVelocity.magnitude);
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Motor.SetPosition(Utils.GetClosestPointOnLine(Motor.TransientPosition, m_startDir, m_endDir));
        m_diff = (m_endDir - m_startDir).normalized;
        m_diff.y = 0;
    }
    public override void OnStateExit()
    {
        
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (_lookInputVector.sqrMagnitude > 0.1f && m_normalMovementStats.OrientationSharpness > 0f)
        {
            // Smoothly interpolate from current to target look direction
            Vector3 smoothedLookInputDirection = Vector3.Slerp(m_blackboard.Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-m_normalMovementStats.OrientationSharpness * deltaTime)).normalized;

            // Set the current rotation (which will be used by the KinematicCharacterMotor)
            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Vector3.up);
        }

        Vector3 currentUp = currentRotation * Vector3.up;
        Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-m_normalMovementStats.BonusOrientationSharpness * deltaTime));
        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        // Ground movement
        if (Motor.GroundingStatus.IsStableOnGround)
        {
            float currentVelocityMagnitude = currentVelocity.magnitude;


            Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

            // Reorient velocity on slope
            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

            // Calculate target velocity
            Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveInputVector.magnitude;
            Vector3 targetMovementVelocity = reorientedInput * m_normalMovementStats.MaxStableMoveSpeed;

            // Smooth movement Velocity
            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-m_normalMovementStats.StableMovementSharpness * deltaTime));
        }
        // Air movement
        else
        {
            // Add move input
            if (_moveInputVector.sqrMagnitude > 0f)
            {
                Vector3 addedVelocity = _moveInputVector * m_normalMovementStats.AirAccelerationSpeed * deltaTime;

                Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                // Limit air velocity from inputs
                if (currentVelocityOnInputsPlane.magnitude < m_normalMovementStats.MaxAirMoveSpeed)
                {
                    // clamp addedVel to make total vel not exceed max vel on inputs plane
                    Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, m_normalMovementStats.MaxAirMoveSpeed);
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
            currentVelocity += m_normalMovementStats.Gravity * deltaTime;

            // Drag
            currentVelocity *= (1f / (1f + (m_normalMovementStats.Drag * deltaTime)));
        }
        if (_internalVelocityAdd.sqrMagnitude > 0f)
        {
            currentVelocity += _internalVelocityAdd;
            _internalVelocityAdd = Vector3.zero;
        }
    }

}
