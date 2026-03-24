
public interface ITransition<T>
{
    T To { get; }
    IPredicate Predicate { get; }
}



