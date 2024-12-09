using UnityEngine;

public class MonsterStateMachine
{
    protected IMonster currentState { get; private set; }
    public MonsterController MonsterController{ get; private set; }
    public IdleState idleState { get; private set; }
    public PatrollState patrollState{ get; private set; }
    public AttackState attackState{ get; private set; }
    public MonsterStateMachine(MonsterController monsterController)
    {
        this.MonsterController = monsterController;
        this.idleState = new IdleState(monsterController);
        this.patrollState = new PatrollState(monsterController);
        this.attackState = new AttackState(monsterController);
    }



    public void Initialize(IMonster monsterState) // 초기화
    {
        currentState = monsterState;
    }

    public void TransitionToState(IMonster nextState)
    {
        currentState?.Exit();
        currentState = nextState; // 다음 스테이트로
        currentState?.Enter();
    }
    public void Excute()
    {
        if (currentState != null)
        {
            currentState?.Excute();
        }
    }
   

}
