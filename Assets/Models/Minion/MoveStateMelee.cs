using UnityEngine;

public class MoveStateMelee : MinionState
{
    private MinionMelee minion;
    private Vector3 destination;

    public MoveStateMelee(Minion minionBase, MinionStateMachine stateMachine, string animBoolName) : base(minionBase, stateMachine, animBoolName)
    {
        minion = minionBase as MinionMelee;
    }

    public override void Enter()
    {
        base.Enter();

        destination = minion.GetPatrolDestination();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        minion.agent.SetDestination(destination);

        if (minion.agent.remainingDistance <= 0)
        {
            stateMachine.ChangeState(minion.idleState);
        }
    }
}
