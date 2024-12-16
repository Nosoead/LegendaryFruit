using UnityEngine;

public class AttackState :IState
{
    private MonsterController monsterController;

    private bool isGround;
    //private MonsterAttributeLogicsDictionary monsterAttributeLogicsDictionary;
    float testTime = 0f;

    public AttackState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
        InitializeAttributeLogicsDictionary();
    }

    private void InitializeAttributeLogicsDictionary()
    {
        //monsterAttributeLogicsDictionary = new MonsterAttributeLogicsDictionary();
        //monsterAttributeLogicsDictionary.Initialize();
    }
    public void Enter()
    {
    }

    public void Excute()
    {
        // currentDistance <AttackDistance-->때리기
        if (monsterController.InAttackRange())
        {
            
            testTime += Time.deltaTime;
            if (testTime >= 1f)
            {
                testTime = 0f;
                monsterController.Attack();
            }

            // if-> 때리는 애니메이션 ->애니메이션에서 OnHit
            monsterController.animationController.OnAttack();
            return;
        }
        // player 놓치면 idleState 
        if (!monsterController.DetectPlayer())
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.idleState);
        }
        // player 발견 && 땅이 있으면 이동
        if (monsterController.DetectPlayer())
        {
            if (monsterController.monsterGround.GetOnGround())
            {
                monsterController.Move();
            }
        }
        if(!monsterController.monsterGround.GetOnGround())
        {
            monsterController.ReverseDirection();
        }
    }

    public void Exit()
    {
    }
    
    public void UpdateStat(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

   
}
