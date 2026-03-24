using BloxLikeBasic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlideStateMachineInitializer : MonoBehaviour
{
    public BloxStateController StateController;

    public NormalMovement m_normalMovement;
    public SlideClimb m_slideClimb;
    public SlideDown m_slideDown;

    private BloxStateBlackboard m_blackboard;
    public UnityAction OnGroundHitEvent;

    public bool StartClimb = false;
    public bool StartSlide = false;

    public Transform m_startClimb;

    public SlideData m_slideData;
    

    public Vector3 m_dir;
    public Vector3 m_slideDir;
    public float m_climbMultiplier = 1f;
    private void Awake()
    {
        m_blackboard = StateController.m_blackboard;
        m_dir = m_slideData.GetClimbDirection();
        m_slideDir = m_slideData.GetSlideDirection();

        m_slideClimb.m_bottomPos = m_startClimb.position.y + .2f;
        m_slideClimb.m_climbDir = m_dir;
        m_slideDown.m_slideDir = m_slideDir;

        m_slideDown.m_arrivePos = m_slideData.m_slideLandPoint.position;
        m_slideClimb.m_topPos = m_slideData.m_climbEnd.position.y;


        StateController.AddTransition(m_normalMovement, m_slideClimb, new FuncPredicate(StartClimbCheck));
        StateController.AddTransition(m_slideClimb, m_normalMovement, new FuncPredicate(FinishClimb));


        StateController.AddTransition(m_slideClimb, m_slideDown, new FuncPredicate(StartFallCheck));
        StateController.AddTransition(m_normalMovement, m_slideDown, new FuncPredicate(StartFallCheck));

        StateController.AddTransition(m_slideDown, m_normalMovement, new FuncPredicate(m_slideDown.IsDoneSliding));

    }
    bool FinishClimb()
    {
        return m_slideClimb.IsAtTop();
    }
    bool StartClimbCheck()
    {
        if (StartClimb)
        {
            StartClimb = false;
            m_blackboard.Motor.SetTransientPosition(m_startClimb.position);
            m_slideClimb.SetTemporalMultiplier(m_climbMultiplier);
            return true;
        }
        else return false;
    }

   bool StartFallCheck()
    {
        if (StartSlide)
        {
            StartSlide = false;
            m_blackboard.Motor.SetPosition(m_slideData.GetClosestPointOnSlide(m_blackboard.Motor.TransientPosition) + Vector3.up * .2f);
           /* m_blackboard.Motor.SetPosition(new Vector3(m_blackboard.Motor.TransientPosition.x, 
                Mathf.Min(m_endMeasure.position.y, m_blackboard.Motor.TransientPosition.y) + .1f, m_endSlide.position.z));*/
            return true;
        }
        else return false;
    }
}
