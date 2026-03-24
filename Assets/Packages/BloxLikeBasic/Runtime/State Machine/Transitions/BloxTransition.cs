public class BloxTransition<T> : ITransition<T>
{
    public T To { get; }

    public IPredicate Predicate { get; }

    public BloxTransition(T to, IPredicate predicate)
    {
        To = to;
        Predicate = predicate;
    }

}
