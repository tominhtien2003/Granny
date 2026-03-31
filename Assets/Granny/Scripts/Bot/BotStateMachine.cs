public class BotStateMachine
{
    private Gr_IState currentState;

    public Gr_IState CurrentState => currentState;

    public void ChangeState(Gr_IState newState)
    {
        if(newState == null || currentState == newState)
            return;
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Execute();
    }
}
