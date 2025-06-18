using UnityEngine;

public class IdleStateMelee : MinionState
{
    private MinionMelee minion;

    public IdleStateMelee(Minion minionBase, MinionStateMachine stateMachine, string animBoolName) : base(minionBase, stateMachine, animBoolName)
    {
        minion = minionBase as MinionMelee;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = minionBase.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(minion.moveState);
        }
    }
}
