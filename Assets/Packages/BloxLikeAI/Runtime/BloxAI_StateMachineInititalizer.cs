using BloxLikeBasic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloxAI_StateMachineInititalizer : MonoBehaviour
{
    public AIStateController StateController;

    public BloxAI_FollowWaypoints m_followWps;
    public BloxAI_WanderAround m_wanderAround;

    private void Awake()
    {
        StateController.AddTransition(m_followWps, m_wanderAround, new FuncPredicate(() => m_followWps.Finished));
        StateController.AddTransition(m_wanderAround, m_followWps, new FuncPredicate(() => !m_followWps.Finished));

    }

}
