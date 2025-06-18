using UnityEngine;

public class MinionState
{
    protected Minion minionBase;
    protected MinionStateMachine stateMachine;

    protected string animBoolName;
    protected float stateTimer;


    public MinionState(Minion minionBase, MinionStateMachine stateMachine, string animBoolName)
    {
        this.minionBase = minionBase;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    
     public virtual void Exit()
    { 

    }
}
