using System.Collections.Generic;

public class StateNode<T>
{
    public HashSet<ITransition<T>> Transitions { get; }

    public StateNode()
    {
        Transitions = new HashSet<ITransition<T>>();
    }

    public void AddTransition(T to, IPredicate condition)
    {
        Transitions.Add(new BloxTransition<T>(to, condition));
    }
}
