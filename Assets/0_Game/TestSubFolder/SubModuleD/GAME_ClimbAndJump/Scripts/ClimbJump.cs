using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbJump : BaseMovementBloxState
{
    private Vector3 m_reverseDirection;
    public float m_jumpOutSpeed = 10f;
    public float m_jumpUpSpeed = 3f;
    public float m_fallSpeed = 30f;
    public float m_turnSpeed = 15f;
    public override void AfterCharacterUpdate(float deltaTime)
    {
       
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
        
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        AudioManager.Instance.PlayJumpSound();
        m_reverseDirection = -m_blackboard.Motor.CharacterForward;
        Motor.BaseVelocity = Vector3.up * m_jumpUpSpeed;
        m_blackboard.Animator.SetBool("Grounded", false);
    }
    public override void OnStateUpdate(float dt)
    {
       
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        Vector3 smoothedLookInputDirection = Vector3.Slerp(m_blackboard.Motor.CharacterForward, m_reverseDirection, 1 - Mathf.Exp(-m_turnSpeed * deltaTime)).normalized;

        currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, m_blackboard.Motor.CharacterUp);
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        currentVelocity += (Vector3.down * m_fallSpeed + m_reverseDirection * m_jumpOutSpeed) * deltaTime;
    }
}
