using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
public enum LadderState
{
    CLIMBING,
    LEAVING_TOP,
    LEAVING_BOTTOM,
    LEAVING_NORMAL
}
public class TrussClimbing : BaseMovementBloxState
{
    private Vector3 _moveInputVector;
    public float m_climbSpeed;
    public float m_verticalAcceleration;
    private LadderState m_ladderState;
    private float m_prevY;
    private RaycastHit m_hit;
    private float m_debugCos;
    public string VerticalSpeedParameter = "VerticalSpeed";
    public string IsClimbingParameter = "IsClimbing";
    private bool _jumpRequested;

    public BoxCollider CurrentTruss { get; set; }
    public override void AfterCharacterUpdate(float deltaTime)
    {
        m_blackboard.Animator.SetFloat(VerticalSpeedParameter, Motor.BaseVelocity.y);
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
        
    }

    public override void OnStateExit()
    {
        if (_jumpRequested)
        {
            Motor.BaseVelocity = -Motor.CharacterForward * 2f;
            _jumpRequested = false;
        }
        else if (m_ladderState == LadderState.LEAVING_TOP)
        {
            Vector3 pos = Motor.TransientPosition;
            pos.y = CurrentTruss.ClosestPoint(pos).y + .01f;
            Motor.SetPosition(pos + Motor.CharacterForward * .2f);
            Motor.BaseVelocity = Motor.CharacterForward * 1f;
        }

        else Motor.BaseVelocity = Vector3.zero;
            //Debug.Log(m_ladderState + " " + Motor.BaseVelocity);
        //Motor.Capsule.enabled = true;
        m_blackboard.Animator.SetBool(IsClimbingParameter, false);
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        //Debug.LogError("Entering...");
        //Motor.Capsule.enabled = false;
        m_debugCos = 1;
        Motor.IsWalled = false;
        Motor.ForceUnground();
        m_blackboard.Animator.SetBool(IsClimbingParameter, true);
    }
    public override void OnStateUpdate(float dt)
    {
        Quaternion cameraPlanarRotation = Quaternion.LookRotation(InputHolder.ExternalPlanarDirection, Motor.CharacterUp);
        _moveInputVector = cameraPlanarRotation * InputHolder.moveInputVector;
        if (InputHolder.JumpDown)
        {
            _jumpRequested = true;
            InputHolder.JumpDown = false;
        }

    }
    public bool CheckExit()
    {
        bool isLeaving = false;
        if (_jumpRequested)
        {
            m_ladderState = LadderState.LEAVING_NORMAL;
            isLeaving = true;
        }
        else
        {
            Ray bottomRay = new Ray(Motor.TransientPosition + Vector3.up * .03f, Motor.CharacterForward);
            Ray topRay = new Ray(Motor.TransientPosition + Motor.CharacterTransformToCapsuleTop + Vector3.up * .03f, Motor.CharacterForward);
            if (!CurrentTruss.Raycast(bottomRay, out m_hit, Motor.Capsule.radius + .1f) &&
            !CurrentTruss.Raycast(topRay, out m_hit, Motor.Capsule.radius + .1f))
            {
                if (Physics.Raycast(bottomRay, out m_hit, Motor.Capsule.radius + .1f, m_blackboard.m_trussLayer)
                    || Physics.Raycast(topRay, out m_hit, Motor.Capsule.radius + .1f, m_blackboard.m_trussLayer))
                {
                    CurrentTruss = m_hit.collider as BoxCollider;
                    m_prevY = Motor.TransientPosition.y;
                    return false;
                }
                isLeaving = true;
                m_ladderState = (Motor.TransientPosition.y > m_prevY) ? LadderState.LEAVING_TOP : LadderState.LEAVING_BOTTOM;
            }
            else if (Motor.GroundingStatus.IsStableOnGround && m_debugCos > 125f)
            {
                //Debug.Log("EL");
                m_ladderState = LadderState.LEAVING_BOTTOM;
                isLeaving = true;
            }
        }
        if (!isLeaving) return false;
        
        m_prevY = Motor.TransientPosition.y;
       
        return true;
    }

   

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        Vector3 input = _moveInputVector;
        Vector3 face = Motor.CharacterForward;
        face.y = 0;
        float angl = Vector3.SignedAngle(face, input, Vector3.up);
        float vel = Mathf.Cos(angl * Mathf.Deg2Rad);
        m_debugCos = Vector3.Angle(face, input);
        if (input.magnitude <= 0.01f)
        {
            vel = 0;
            m_debugCos = 1;
        }
        if (vel > 0.01f && Motor.GroundingStatus.FoundAnyGround) Motor.ForceUnground();
        Vector3 targetVerticalVelocity = vel * m_climbSpeed * Motor.CharacterUp;

        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVerticalVelocity, m_verticalAcceleration * deltaTime);
    }

}
