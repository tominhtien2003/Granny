using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRun : BaseMovementBloxState
{
    public float m_runSpeed = 3f;
    public Vector3 m_runDir;
    public override void AfterCharacterUpdate(float deltaTime)
    {

    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public override void OnStateExit()
    {
        m_blackboard.Animator.SetBool("SpaceRunning", false);
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Motor.BaseVelocity = m_runDir * m_runSpeed;
        m_blackboard.Animator.SetBool("SpaceRunning", true); 
        m_blackboard.Animator.SetBool("Grounded", true); 
    }
    public void SetRunDirection(Vector3 dir)
    {
        m_runDir = Vector3.ProjectOnPlane(dir, Vector3.up).normalized;
    }
    public override void OnStateUpdate(float dt)
    {

    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        //currentVelocity = m_runDir * m_runSpeed * deltaTime;
    }
}