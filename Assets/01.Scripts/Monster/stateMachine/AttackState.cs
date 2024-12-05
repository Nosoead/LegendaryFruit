using UnityEngine;

public class AttackState :IMonster
{
    private MonsterStateMachine stateMachine;

    public AttackState(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("AttackState Enter");
    }

    public void Excute()
    {
        //플레이어와의 거리 확인
        float distanceToPlayer = Vector2.Distance(
            stateMachine.monster.transform.position,
            stateMachine.monster.Data.target.transform.position
            );
        // ChaseRange 넘으면 다시 Patroll로
        if (distanceToPlayer > stateMachine.monster.Data.chaseRange)
        {
            stateMachine.TransitionToState(stateMachine.patrollState);
            return;
        }
        // currentDistance <AttackDistance-->때리기
        if (distanceToPlayer <= stateMachine.monster.Data.attackDistance)
        {
            // if-> 때리는 애니메이션 ->애니메이션에서 OnHit
        }
        stateMachine.MoveTowardsTarget();
    }

    public void Exit()
    {
        Debug.Log("AttackState Exit");
    }
}
