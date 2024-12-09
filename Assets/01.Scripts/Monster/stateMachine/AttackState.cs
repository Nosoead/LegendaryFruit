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

        // player 놓치면 idleState
        if (!monsterController.DetectPlayer())
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.idleState);
            return;
        }
        // currentDistance <AttackDistance-->때리기
        if (monsterController.DetectPlayer())
        {
            monsterController.Move();
            Debug.Log("돌진!!");
        }

        if (monsterController.InAttackRange())
        {
            Debug.Log("공격!!");
            // if-> 때리는 애니메이션 ->애니메이션에서 OnHit
        }
        monsterController.Move();
    }

    public void Exit()
    {
        Debug.Log("AttackState Exit");
    }
}
