using UnityEngine;
using UnityEngine.AI;

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

        minion.agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        minion.transform.rotation = minion.FaceTarget(GetNextPathPoint());


        if (minion.agent.remainingDistance <= minion.agent.stoppingDistance + 0.05f)
        {
            stateMachine.ChangeState(minion.idleState);
        }
    }

    public Vector3 GetNextPathPoint()
    {
        NavMeshAgent agent = minion.agent;
        NavMeshPath path = agent.path;

        if (path.corners.Length < 2)
        {
            return agent.destination;
        }

        for (int i = 0; i < path.corners.Length; i++)
        {
            if (Vector3.Distance(agent.transform.position, path.corners[i]) < 1)
            {
                return path.corners[i];
            }
        }

        return agent.destination;
    }
}
