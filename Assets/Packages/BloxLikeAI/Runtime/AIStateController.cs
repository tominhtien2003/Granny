using UnityEngine;

public class AIStateController : GenericStateMachine<BaseAIBloxState>
{
    public BloxBrain m_brain;
    private BloxStateBlackboard m_blackboard;
    public BloxStateController m_movementStateController;
    private void Start()
    {
        m_currentNode = GetOrAddNode(m_currentState);
        m_blackboard = m_movementStateController.m_blackboard;
        foreach (BaseAIBloxState state in GetComponents<BaseAIBloxState>())
        {
            if (state == null) continue;
            state.m_blackboard = m_blackboard;
            state.m_brain = m_brain;
        }
        m_currentState.OnStateEnter();
    }
    public void ForceState(BaseAIBloxState state)
    {
        if (m_currentState == state) return;
        SwitchToState(state);
    }
    private void Update()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            SwitchToState(transition.To);
            return;
        }
        m_currentState.OnStateUpdate(Time.deltaTime);

    }
}
