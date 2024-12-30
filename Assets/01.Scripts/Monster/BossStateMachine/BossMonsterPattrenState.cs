using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterPattrenState : IState
{
    private BossMonsterController bossMonsterController;

    private float attacklate;
    private float time = 0;

    
    public BossMonsterPattrenState(BossMonsterController bossMonsterController)
    {
        this.bossMonsterController = bossMonsterController;
        InitializeAttributeLogicsDictionary();
    }
    private void InitializeAttributeLogicsDictionary()
    {
        //monsterAttributeLogicsDictionary = new MonsterAttributeLogicsDictionary();
        //monsterAttributeLogicsDictionary.Initialize();
    }

    public void Enter()
    {
        Debug.Log("패턴공격진입");
    }

    public void Execute()
    {
        //if (bossMonsterController.animator.OnAreaAttackCheck())
        //{
        //    if (bossMonsterController.InAttackRange())
        //    {
        //        bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.attackState);
        //    }
        //    else
        //    {
        //        bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.idleState);
        //    }
        //}
    }

    public void Exit()
    {

    }
}
