public class MinionStateMachine
{
    public MinionState currentState { get; private set; }

    public void Initialize(MinionState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(MinionState newState)
    { 
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
