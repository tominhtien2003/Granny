using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure_BloxAIFollowWaypoint : BloxAI_FollowWaypoints
{
    [SerializeField] WaypointScript escalatorWaypoint;
    public override void OnStateUpdate(float dt)
    {
        if (m_currentWaypoint == null) return;
        if (!m_currentWaypoint.CanMove)
        {
            if (m_currentWaypoint != escalatorWaypoint)
            {
                Finished = true;
            }
            return;
        }
        m_waitComponent.UpdateWait(dt);
        if (m_waitComponent.IsWaiting) return;

        if (SetMoveTowards(m_currentDestination, m_margin)) OnDestinationReached();
        SetJumpAction();
    }
}
