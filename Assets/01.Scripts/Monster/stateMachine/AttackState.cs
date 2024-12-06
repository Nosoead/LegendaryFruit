using UnityEngine;

public class AttackState :IMonster
{
    private MonsterController monster;

    public AttackState(MonsterController monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        Debug.Log("AttackState Enter");
    }

    public void Excute()
    {
        //플레이어와의 거리 확인
        float distanceToPlayer = Vector2.Distance(
            monster.transform.position,
            monster.Monster.Data.target.transform.position
            );
        // ChaseRange 넘으면 다시 Patroll로
        if (distanceToPlayer > monster.Monster.Data.chaseRange)
        {
            monster.StateMachine.TransitionToState(monster.StateMachine.patrollState);
            return;
        }
        // currentDistance <AttackDistance-->때리기
        if (distanceToPlayer <= monster.Monster.Data.attackDistance)
        {
            // if-> 때리는 애니메이션 ->애니메이션에서 OnHit
        }
        monster.StateMachine.MoveTowardsTarget();
    }

    public void Exit()
    {
        Debug.Log("AttackState Exit");
    }
}
