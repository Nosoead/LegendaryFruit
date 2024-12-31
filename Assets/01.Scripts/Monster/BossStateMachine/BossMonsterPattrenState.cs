using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterPattrenState : IState
{
    private BossMonsterController bossMonsterController;

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
        bossMonsterController.animator.BossPatternAttack(true);
    }

    public void Execute()
    {
        if(bossMonsterController.animator.HasPatternAttackFinished())
        {
            if(bossMonsterController.InAttackRange())
            {
                bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.attackState);
            }
            else
            {
                bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.patrollState);
            }
        }
    }

    public void Exit()
    {
        bossMonsterController.animator.BossPatternAttack(false);
    }
}
