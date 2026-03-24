using UnityEngine;
using UnityEngine.Events;
public class SpaceStand : BaseMovementBloxState
{
    [SerializeField] private float m_respawnTime;
    private float m_currentRespawnTime;
    public bool CanMove = false;

    public Transform m_respawnPoint;
    public override void AfterCharacterUpdate(float deltaTime)
    {

    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public override void OnStateExit()
    {
        Motor.SetTransientPosition(m_respawnPoint.position);
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        m_currentRespawnTime = 0;
        CanMove = false;
        m_blackboard.Animator.SetFloat("PlanarSpeed", 0);
    }
    public override void OnStateUpdate(float dt)
    {
        m_currentRespawnTime += dt;
        if (m_currentRespawnTime >= m_respawnTime) CanMove = true;
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {

    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {

    }

}
