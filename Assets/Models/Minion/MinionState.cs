using UnityEngine;

public class MinionState
{
    public Minion minionBase;
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
        minionBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        minionBase.anim.SetBool(animBoolName, false);
    }
}
