using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CannonShotState : BaseMovementBloxState, IStateChangeEvents
{
    public Vector3 m_destination = new Vector3(-12.86f, 5f, 2.53f);
    public Vector3 m_trueDes;
    public Transform m_cannonPos;
    private Transform m_prevParent;
    public float m_moveTime = 50f;
    public float m_height = 30f;
    public bool CannonIn = false;
    public Transform m_graphics;
    public UnityAction<bool> OnStateChanged { get => m_onStateChanged; set => m_onStateChanged = value; }
    public UnityAction<bool> m_onStateChanged;

    public override void AfterCharacterUpdate(float deltaTime)
    {
        
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
        
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        m_prevParent = Motor.transform.parent;
        Motor.SetGroundSolvingActivation(false);
       
        Motor.SetPosition(m_cannonPos.position);
       
        Motor.BaseVelocity = Vector3.zero;
        Vector3 d = m_cannonPos.forward;
        d = d.ProjectOntoPlane(Vector3.up).normalized;
        Motor.SetRotation(Quaternion.LookRotation(d));
        m_graphics.rotation = (Quaternion.LookRotation(m_cannonPos.forward));
        m_blackboard.Animator.CrossFadeInFixedTime("SideDive", .1f);
        IsShot = false;
        m_accDt = 0;
        OnStateChanged?.Invoke(true);
        m_trueDes = m_destination - m_cannonPos.position;
    }
    public bool IsShot = false;
    public void StartShot()
    {
        
        IsShot = true;
        
    }
    public override void OnStateExit()
    {
        Motor.SetGroundSolvingActivation(true);
        m_blackboard.Animator.CrossFadeInFixedTime("NotGrounded", .1f);
        m_blackboard.Animator.SetBool("Grounded", false);
        m_graphics.localRotation = Quaternion.identity;
        
    }

    public override void OnStateUpdate(float dt)
    {
        

    }
    private Vector3 m_currentTargetPos;
    private float m_accDt;
    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (!IsShot)
        {
            m_graphics.rotation = Quaternion.LookRotation(m_cannonPos.forward);
            return;
        }
        m_accDt += deltaTime;
        m_currentTargetPos = Utils.SampleParabola(m_cannonPos.position, m_destination, m_height, m_accDt / m_moveTime, Vector3.up);
        m_graphics.rotation = Quaternion.LookRotation(m_currentTargetPos - Motor.transform.position);
        Vector3 diff = m_currentTargetPos - m_destination;
        diff.y = 0;
        if (diff.sqrMagnitude <= 25f) CannonIn = false;
        
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (!IsShot)
        {
            Motor.SetTransientPosition(m_cannonPos.position);
            return;
        }
        //currentVelocity = (m_currentTargetPos - Motor.transform.position).normalized * m_moveSpeed;
        Motor.SetTransientPosition(m_currentTargetPos);
    }
}
