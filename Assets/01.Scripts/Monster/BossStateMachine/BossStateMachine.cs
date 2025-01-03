using UnityEngine.Events;

public class BossStateMachine
{
    protected IState currentState { get; private set; }
    public BossMonsterController bossMonsterController { get; private set; }
    public BossIdleState idleState { get; private set; }
    public BossAttackState attackState { get; private set; }
    public BossPatrollState patrollState { get; private set; }
    public BossMonsterPattrenState pattrenState { get; private set; }
    public UnityAction<BossMonsterController> OnStatsUpdated { get; private set; }
    public BossStateMachine(BossMonsterController bossMonsterController)
    {
        this.bossMonsterController = bossMonsterController;
        this.idleState = new BossIdleState(bossMonsterController);
        this.attackState = new BossAttackState(bossMonsterController);
        this.patrollState = new BossPatrollState(bossMonsterController);
        this.pattrenState = new BossMonsterPattrenState(bossMonsterController);
        OnStatsUpdated += UpdateStat;
    }

    public void UpdateStat(BossMonsterController bossMonsterController)
    {
        idleState.UpdateStat(bossMonsterController);
        patrollState.UpdateStat(bossMonsterController);
    }

    public void Initialize(IState monsterState)
    {
        currentState = monsterState;
        monsterState.Enter();
    }

    public void TransitionToState(IState nextState)
    {
        currentState?.Exit();
        currentState = nextState;
        currentState?.Enter();
    }

    public void Excute()
    {
        if(currentState != null)
        {
            currentState.Execute();
        }
    }
}
