public class MinionMelee : Minion
{

    public IdleStateMelee idleState { get; set; }
    public MoveStateMelee moveState { get; set; }
    public MoveStateMelee deadState { get; set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleStateMelee(this, stateMachine, "Idle");
        moveState = new MoveStateMelee(this, stateMachine, "Move");
        deadState = new MoveStateMelee(this, stateMachine, "Dead");
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
