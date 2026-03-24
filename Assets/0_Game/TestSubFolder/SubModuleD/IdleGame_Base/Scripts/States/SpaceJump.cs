using UnityEngine;
using UnityEngine.Events;
public class SpaceJump : BaseMovementBloxState
{
    public float m_jumpUpSpeed = 3f;
    private float m_currentJumpSpeed;
    public float m_gravity = -10f;
    public float m_horizontalSpeed = 5f;
    public UnityAction<bool> OnStateChanged;
    public override void AfterCharacterUpdate(float deltaTime)
    {

    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public override void OnStateExit()
    {
        OnStateChanged.Invoke(false);
    }
    float m_timeLimit = 0;
    bool m_updateHorizontal = true;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        m_currentJumpSpeed = m_jumpUpSpeed;
        
        Motor.BaseVelocity = Motor.BaseVelocity.normalized * 60f;
        Motor.BaseVelocity.y = m_currentJumpSpeed;
        m_blackboard.Animator.SetBool("Grounded", false);
        m_updateHorizontal = true;
        m_timeLimit = 0;
        OnStateChanged.Invoke(true);
    }
    public override void OnStateUpdate(float dt)
    {

    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {

        currentVelocity += deltaTime * m_gravity * Vector3.up;
        if (!m_updateHorizontal) return;
        m_timeLimit += Vector3.ProjectOnPlane(currentVelocity, Vector3.up).magnitude * deltaTime;
        if (m_timeLimit >= 15f)
        {
            currentVelocity.x = currentVelocity.z = 0;
            m_updateHorizontal = false;
        }
    }
}
