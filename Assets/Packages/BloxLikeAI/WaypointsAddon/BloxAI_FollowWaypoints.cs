
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BloxAI_FollowWaypoints : BaseAIBloxState,IOnStateUpdate
{
    public WaypointScript m_currentWaypoint;
 
    private float m_currentWaitDuration;
    private float m_currentMargins;
    private Vector3 m_currentOffset;
    protected Vector3 m_currentDestination;
    public bool Finished = false;
    [Header("Moving stats")]
    public float m_minWaitTime = .3f;
    public float m_maxWaitTime = .6f;
    public float m_margin = 1f;

    public WaitComponent m_waitComponent = new();
    public JumpCheckComponent m_jumpComponent = new();
    public TriggerBox m_triggerBox;

    public System.Action<float> OnStateUpdateAction { get ; set; }

    void SetCheck(Collider coll)
    {
        if (!coll.CompareTag("Kill")) return;
        m_jumpComponent.Triggered = true;
    }
    protected void OnDestinationReached()
    {
        if (m_currentWaypoint.m_nextWaypoints.Count == 0)
        {
            m_waitComponent.IsWaiting = true;
            m_currentWaypoint = null;
            SetMovementAction(Vector3.zero);
            Finished = true;
            return;
        }
        else
        {
            SetMovementAction(Vector3.zero);
            m_waitComponent.StartWait(m_currentWaypoint.m_overrideWait > 0 ? m_currentWaypoint.m_overrideWait : Random.Range(m_minWaitTime, m_maxWaitTime));
            GetNextWaypoint();
        }
    }
   
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Finished = false;
        if (m_currentWaypoint != null) AssignWaypoint(m_currentWaypoint);
        if (m_triggerBox != null) m_triggerBox.onTriggerEnter += SetCheck;
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        if (m_triggerBox != null) m_triggerBox.onTriggerEnter -= SetCheck;
    }
    public void AssignWaypoint(WaypointScript wp)
    {
        m_currentWaypoint = wp;
        if (m_currentWaypoint.m_overrideMargins >= 0) m_currentMargins = m_currentWaypoint.m_overrideMargins;
        else m_currentMargins = m_margin;
        float rand = m_currentWaypoint.GetRandomRadius();
        m_currentOffset = WaypointUtils.CenterToTargetDir() * Random.Range(0, rand);
        m_currentDestination = m_currentWaypoint.transform.position + m_currentOffset;
    }
    protected virtual void GetNextWaypoint()
    {
        if (m_currentWaypoint != null)
        {
            List<Transform> nexts = m_currentWaypoint.m_nextWaypoints;
            if (nexts.Count == 0) return;
            int indx = Random.Range(0, nexts.Count);
            if (nexts[indx] == null)
            {
                Debug.LogError("Next waypoint is null", m_currentWaypoint);
                return;
            }
            AssignWaypoint(nexts[indx].GetComponent<WaypointScript>());
        }
    }

    public override void OnStateUpdate(float dt)
    {
        base.OnStateUpdate(dt);
        OnStateUpdateAction?.Invoke(dt);
        if (m_currentWaypoint == null) return;
        if (!m_currentWaypoint.CanMove) return;
        m_waitComponent.UpdateWait(dt);
        if (m_waitComponent.IsWaiting) return;
        
        if (SetMoveTowards(m_currentDestination, m_margin)) OnDestinationReached();
        SetJumpAction();
    }
    protected override void SetJumpAction()
    {
        base.SetJumpAction();
        if (!m_blackboard.InputHolder.JumpDown && m_jumpComponent.Triggered)
        {
            m_jumpComponent.Triggered = false;
            m_blackboard.InputHolder.JumpDown = true;
        }
    }
    
}
