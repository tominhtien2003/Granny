using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClimbFall : BaseMovementBloxState, IStateChangeEvents
{
    public float m_fallSpeed = 50f;
    public bool HitGround = false;
    public string IsFallingParameter = "IsFallingFast";
    public string GroundedParameter = "Grounded";

    public UnityAction OnGroundHitEvent;
    public UnityAction OnStateEnterEvent;

    public UnityAction<bool> OnStateChanged { get => m_onStateChanged; set => m_onStateChanged = value; }
    public UnityAction<bool> m_onStateChanged;

    public override void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        base.OnGroundHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
        m_blackboard.Animator.SetBool(GroundedParameter, true);
        HitGround = true;
        OnGroundHitEvent?.Invoke();
    }

    public override void AfterCharacterUpdate(float deltaTime)
    {
        
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
        
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        HitGround = false;
        m_blackboard.Animator.SetBool(IsFallingParameter, true);
        Motor.BaseVelocity = Vector3.up * Motor.BaseVelocity.y;
        OnStateEnterEvent?.Invoke();
        OnStateChanged?.Invoke(true);
    }

    public override void OnStateExit()
    {
        m_blackboard.Animator.SetBool(IsFallingParameter, false);
        OnStateChanged?.Invoke(false);
    }

    public override void OnStateUpdate(float dt)
    {
        
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        currentVelocity += Vector3.down * m_fallSpeed * deltaTime; 
    }
}
