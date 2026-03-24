using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure_BloxAIWanderAround : BloxAI_WanderAround
{
    [SerializeField] BloxAI_FollowWaypoints m_follow;
    [Header("Avoidance")]
    public float wallCheckDistance = 1.2f;
    public float groundCheckDistance = 2f;
    public float forwardCheck = 0.8f;
    public override void OnStateUpdate(float dt)
    {
        m_waitComponent.UpdateWait(dt);
        if (m_waitComponent.IsWaiting) return;

        if (m_follow.m_currentWaypoint != null && m_follow.m_currentWaypoint.CanMove)
        {
            m_follow.Finished = false;
        }

        Vector3 toTarget = m_currentDestination - m_blackboard.Motor.TransientPosition;
        toTarget.y = 0;

        if (toTarget.sqrMagnitude < 1f)
        {
            OnDestinationReached();
            return;
        }

        Vector3 moveDir = toTarget.normalized;

        if (!IsDirectionSafe(moveDir))
        {
            moveDir = GetSafeRandomDirection();
            m_currentDestination =
                m_blackboard.Motor.TransientPosition + moveDir * Random.Range(2f, 3f);
        }

        SetMovementAction(moveDir);
    }
    bool IsDirectionSafe(Vector3 dir)
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        bool wall = Physics.Raycast(origin, dir, wallCheckDistance);
        bool ground = Physics.Raycast(
            origin + dir * forwardCheck,
            Vector3.down,
            groundCheckDistance
        );

        Debug.DrawRay(origin, dir * wallCheckDistance, wall ? Color.red : Color.green);
        Debug.DrawRay(
            origin + dir * forwardCheck,
            Vector3.down * groundCheckDistance,
            ground ? Color.green : Color.red
        );

        return !wall && ground;
    }

    Vector3 GetSafeRandomDirection()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 dir = Random.insideUnitSphere;
            dir.y = 0;
            dir.Normalize();

            if (IsDirectionSafe(dir))
                return dir;
        }

        return -transform.forward;
    }
}
