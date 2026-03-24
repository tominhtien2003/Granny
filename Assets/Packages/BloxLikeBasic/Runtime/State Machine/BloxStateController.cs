using KinematicCharacterController;
using System;
using UnityEngine;
[Serializable]
public class BloxStateBlackboard
{
    public BloxStateBlackboard(BloxInputHolder inputHolder, KinematicCharacterMotor motor, Animator animator)
    {
        InputHolder = inputHolder;
        Motor = motor;
        Animator = animator;
    }
    public BloxInputHolder InputHolder;
    public KinematicCharacterMotor Motor;
    public Animator Animator;

    public LayerMask m_trussLayer;
    public string m_killTag = "Kill";
}
public class BloxStateController : GenericStateMachine<BaseMovementBloxState>
{
    public BloxBrain m_brain;
    public BloxStateBlackboard m_blackboard;

    public virtual void Initialise()
    {
        
    }
    private void Start()
    {
        m_blackboard.InputHolder = m_brain.InputHolder;
        foreach (BaseMovementBloxState state in GetComponents<BaseMovementBloxState>())
        {
            if (state == null) continue;
            state.SetBlackboard(m_blackboard);
        }
        m_currentNode = GetOrAddNode(m_currentState);
        m_currentState.OnStateEnter();
    }
    private void Update()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            SwitchToState(transition.To);
            return;
        }
        m_brain.OnGetInput();
        m_currentState.OnStateUpdate(Time.deltaTime);
        
    }
}
