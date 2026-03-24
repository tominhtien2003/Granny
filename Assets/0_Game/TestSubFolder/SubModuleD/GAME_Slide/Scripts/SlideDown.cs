using KinematicCharacterController;
using System;
using UnityEngine;
using UnityEngine.Events;

public class SlideDown : BaseMovementBloxState, IStateChangeEvents
{
    public float m_fallSpeed = 50f;
    public float m_doneSlideXOffset = 5f;
    public UnityAction<bool> OnStateChanged { get => m_onStateChanged; set => m_onStateChanged = value; }
    public UnityAction<bool> m_onStateChanged;
    public UnityAction OnEndSlideReached;
    public override void AfterCharacterUpdate(float deltaTime)
    {

    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        
        Motor.BaseVelocity = Vector3.zero;
        OnStateChanged?.Invoke(true);
        
        Motor.SetRotation(Quaternion.Euler(0, 90, 0));
        m_blackboard.Animator.SetBool("IsSliding", true);
        //Motor.SetGroundSolvingActivation(false);
        m_currentFallSpeed = 0;
        m_isDoneFalling = false;
    }

    public override void OnStateExit()
    {
        Motor.SetTransientPosition(new Vector3(Motor.TransientPosition.x, m_arrivePos.y, Motor.TransientPosition.z));
        OnStateChanged?.Invoke(false);
        //Motor.SetGroundSolvingActivation(true);
        m_blackboard.Animator.SetBool("IsSliding", false);
        Motor.BaseVelocity = Vector3.zero;
    }

    public override void OnStateUpdate(float dt)
    {
        if (!m_isDoneFalling && Motor.TransientPosition.y <= m_arrivePos.y + .3f)
        {
            m_isDoneFalling = true;
            OnFallDone?.Invoke();
        }
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {

    }
    public Vector3 m_slideDir;
    public Vector3 m_arrivePos;
    private float m_currentFallSpeed = 0;
    private bool m_isDoneFalling;
    public Action OnFallDone;
    public bool IsDoneSliding()
    {
        
        return Motor.TransientPosition.x - m_arrivePos.x >= m_doneSlideXOffset;
    }
    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        m_currentFallSpeed += m_fallSpeed * deltaTime;
        if (!m_isDoneFalling) currentVelocity = m_currentFallSpeed * m_slideDir;
        else currentVelocity = 20f * Vector3.right;
    }
}
