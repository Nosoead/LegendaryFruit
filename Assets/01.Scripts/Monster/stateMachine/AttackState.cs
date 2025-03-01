using UnityEngine;
using UnityEngine.Events;

public class AttackState :IState
{
    private MonsterController monsterController;

    private bool isGround;

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
        monsterController.GetRandomPattern();
        monsterController.animationController.OnAttack(true);
    }

    public void Execute()
    {
        // 공격범위벗어나면 패트롤상태 
        if (!monsterController.InAttackRange())
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.patrollState);
            return;
        }

        // player 놓치면 idleState 
        if (!monsterController.DetectPlayer())
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.idleState);
            return;
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
        monsterController.animationController.ResetTrigger();
        monsterController.animationController.OnAttack(false);
    }

    public void UpdateStat(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

   
}
