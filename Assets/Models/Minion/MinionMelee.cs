public class MinionMelee : Minion
{

    public IdleStateMelee idleState { get; set; }
    public IdleStateMelee moveState { get; set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleStateMelee(this, stateMachine, "Idle");
        moveState = new IdleStateMelee(this, stateMachine, "Move");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }
}
