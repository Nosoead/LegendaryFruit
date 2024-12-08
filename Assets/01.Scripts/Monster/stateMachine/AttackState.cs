using UnityEngine;

public class AttackState :IMonster
{
    private MonsterController monsterController;

    public AttackState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

    public void Enter()
    {
        Debug.Log("AttackState Enter");
    }

    public void Excute()
    {
        //플레이어와의 거리 확인
        float distanceToPlayer = Vector2.Distance(
            monsterController.transform.position,
            monsterController.Monster.Data.target.transform.position
            );
        // ChaseRange 넘으면 다시 idle로
        if (distanceToPlayer > monsterController.Monster.Data.chaseRange)
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.idleState);
            return;
        }
        // currentDistance <AttackDistance-->때리기
        if (distanceToPlayer <= monsterController.Monster.Data.attackDistance)
        {
            Debug.Log("공격!!");
            // if-> 때리는 애니메이션 ->애니메이션에서 OnHit
        }
        monsterController.StateMachine.MoveTowardsTarget();
    }

    public void Exit()
    {
        Debug.Log("AttackState Exit");
    }
}
