using UnityEngine;

public class BloxAI_WanderAround : BaseAIBloxState
{
    Vector3 m_wanderCenter;
    protected Vector3 m_currentDestination;
    public WaitComponent m_waitComponent = new();

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        m_wanderCenter = m_blackboard.Motor.TransientPosition;
        OnDestinationReached();
    }

    public override void OnStateUpdate(float dt)
    {
        base.OnStateUpdate(dt);
        m_waitComponent.UpdateWait(dt);
        if (m_waitComponent.IsWaiting) return;

        if (SetMoveTowards(m_currentDestination, 1f, 5f)) OnDestinationReached();
        SetJumpAction();
    }
    protected void OnDestinationReached()
    {
        Vector3 g = Random.insideUnitSphere;
        g.y = 0;
        m_currentDestination = m_wanderCenter + g.normalized * Random.Range(2f, 3f);
        SetMovementAction(Vector3.zero);
        m_waitComponent.StartWait(Random.Range(.25f, .5f));
    }
}

