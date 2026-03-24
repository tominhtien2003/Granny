public class FuncPredicate : IPredicate
{
    private System.Func<bool> _func;
    public FuncPredicate(System.Func<bool> func)
    {
        _func = func;
    }
    public bool Evaluate()
    {
        return _func();
    }
}
