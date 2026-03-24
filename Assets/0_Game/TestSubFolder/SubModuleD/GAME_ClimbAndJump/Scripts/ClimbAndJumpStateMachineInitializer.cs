using BloxLikeBasic;
using UnityEngine;
using UnityEngine.Events;

public class ClimbAndJumpStateMachineInitializer : MonoBehaviour
{
    public BloxStateController StateController;

    public NormalMovement m_normalMovement;
    public SimpleTrussClimb m_trussClimbing;
    public ClimbFall m_climbFall;
    public ClimbJump m_climbJump;

    private BloxStateBlackboard m_blackboard;
    private bool m_canClimb = false;

    public float m_fallSpeedThreshold = -10f;
    public UnityAction OnGroundHitEvent;
    private void Awake()
    {
        m_blackboard = StateController.m_blackboard;

        m_normalMovement.OnWallHitEvent += CheckTruss;

        StateController.AddTransition(m_normalMovement, m_climbFall, new FuncPredicate(() => !m_blackboard.Motor.GroundingStatus.FoundAnyGround && m_blackboard.Motor.Velocity.y < m_fallSpeedThreshold));
        StateController.AddTransition(m_climbFall, m_normalMovement, new FuncPredicate(ClimbFallToNormalMovement));

        StateController.AddTransition(m_normalMovement, m_trussClimbing, new FuncPredicate(CanClimb));

        StateController.AddTransition(m_trussClimbing, m_climbJump, new FuncPredicate(()=>m_trussClimbing.JumpRequested));
        StateController.AddTransition(m_trussClimbing, m_normalMovement, new FuncPredicate(() => m_trussClimbing.CheckExit()));

        StateController.AddTransition(m_climbJump, m_normalMovement, new FuncPredicate(ClimbJumpToNormalMovement));

    }
    bool ClimbJumpToNormalMovement()
    {
        if (m_blackboard.Motor.GroundingStatus.FoundAnyGround)
        {
            OnGroundHitEvent?.Invoke();
        }
        else if (m_blackboard.Motor.Velocity.y >= m_fallSpeedThreshold - 1f) return false;
        return true;
    }

    bool ClimbFallToNormalMovement()
    {
        if (m_climbFall.HitGround)
        {
            OnGroundHitEvent?.Invoke();
            return true;
        }
        return false;
    }
    bool CanClimb()
    {
        if (m_canClimb)
        {
            m_canClimb = false;
            return true;
        }
        return false;
    }
    void CheckTruss(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint)
    {
        if (hitNormal.y > 0.5f) { m_canClimb = false; return; } // Ignore upward normals
        if (((1 << hitCollider.gameObject.layer) & m_blackboard.m_trussLayer) > 0)
        {
            Vector3 v = m_blackboard.InputHolder.moveInputVector;
            if (v.z < .1f) { m_canClimb = false; return; }
            hitNormal.y = 0;
            
            if (Vector3.Angle(-hitNormal, m_blackboard.Motor.CharacterForward) < 30f)
            {

                m_trussClimbing.CurrentTruss = hitCollider as BoxCollider;
                m_canClimb = true;
                return;
            }
        }
        m_canClimb = false;
    }
}
