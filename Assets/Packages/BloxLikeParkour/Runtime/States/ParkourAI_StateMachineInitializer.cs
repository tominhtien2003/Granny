using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourAI_StateMachineInitializer : MonoBehaviour
{
    public AIStateController StateController;

    public BloxAI_FollowWaypoints m_followWps;
    public BloxAI_WanderAround m_wanderAround;
    public BloxAI_DoNothing m_doNothing;
    public DeathToggler m_deathToggler;

    private void Awake()
    {
        StateController.AddTransition(m_followWps, m_wanderAround, new FuncPredicate(() => m_followWps.Finished));
        StateController.AddTransition(m_wanderAround, m_followWps, new FuncPredicate(() => !m_followWps.Finished));
        StateController.AddTransition(m_followWps, m_doNothing, new FuncPredicate(() => m_deathToggler.IsDead));
        StateController.AddTransition(m_doNothing, m_followWps, new FuncPredicate(() => !m_deathToggler.IsDead));

    }
}
