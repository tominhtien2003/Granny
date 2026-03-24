using System.Collections.Generic;
using UnityEngine;

public class GenericStateMachine<T> : MonoBehaviour where T: class, IBloxState
{
    protected Dictionary<T, StateNode<T>> m_nodes = new();
    protected StateNode<T> m_currentNode;
    public T m_currentState;
    protected void SwitchToState(T newState)
    {
        if (!m_nodes.ContainsKey(newState))
        {
            Debug.LogError("State does not exist in Dictionary");
            return;
        }
        if (m_currentState == newState) return;
        if (m_currentState != null)
        {
            m_currentState.OnStateExit();
        }


        m_currentState = newState;
        m_currentState.OnStateEnter();

        m_currentNode = m_nodes[m_currentState];
    }

    protected ITransition<T> GetTransition()
    {
        foreach (var transition in m_currentNode.Transitions)
        {
            if (transition.Predicate.Evaluate())
            {
                return transition;
            }
        }
        return null;
    }
    public void AddTransition(T from, T to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(to, condition);
    }
    protected StateNode<T> GetOrAddNode(T state)
    {
        var node = m_nodes.GetValueOrDefault(state);

        if (node == null)
        {
            node = new StateNode<T>();
            m_nodes.Add(state, node);
        }
        return node;
    }
}
