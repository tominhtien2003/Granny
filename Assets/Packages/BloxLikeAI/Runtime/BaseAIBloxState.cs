using UnityEngine;

public abstract class BaseAIBloxState : MonoBehaviour, IBloxState
{
    public BloxBrain m_brain;
    public BloxStateBlackboard m_blackboard;

    public virtual void OnStateEnter()
    {
        
    }

    public virtual void OnStateExit()
    {
        
    }

    public virtual void OnStateUpdate(float dt)
    {
        
    }
    protected void SetMovementAction(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();
        m_brain.InputHolder.moveInputVector = direction;
        
        m_brain.OnGetInput();
        Debug.DrawRay(transform.position + Vector3.up * 2f, m_brain.InputHolder.ExternalPlanarDirection, Color.magenta);
    }
    protected virtual bool SetMoveTowards(Vector3 destination, float margins, float yMargins = 1)
    {
        BloxAIUtils.GetDistance(m_blackboard.Motor.TransientPosition, destination, out float diff, out float disY);
        if (diff <= margins && disY <= yMargins) return true;
        else SetMovementAction(destination - m_blackboard.Motor.TransientPosition);
        return false;
        
    }
    protected virtual void SetJumpAction()
    {
        m_blackboard.InputHolder.JumpDown = false;
        if ((m_blackboard.Motor.IsWalled && m_blackboard.Motor.GroundingStatus.FoundAnyGround) || m_blackboard.Motor.GroundingStatus.IsOnLedge)
        {
            m_brain.InputHolder.JumpDown = true;
        }
            
    }
}
