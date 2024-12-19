using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternOneState : IState
{
    private BossMonsterController bossMonsterController;

    private float attacklate;
    private float time = 0;

    
    public BossPatternOneState(BossMonsterController bossMonsterController)
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
        Debug.Log("패턴진입");
        bossMonsterController.animator.OnAreaAttack(true);
    }

    public void Execute()
    {
        time += Time.deltaTime;
        if (bossMonsterController.monsterGround.GetOnGround())
        {
            bossMonsterController.animator.Delay(true);
            if(time >= 2f)
            {
                time = 0f;

                if (bossMonsterController.animator.OnAreaAttackCheck())
                {
                    bossMonsterController.animator.Delay(false);
                    if(bossMonsterController.DetectPlayer())
                    {
                        bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.attackState);
                    }
                    else
                    {
                        bossMonsterController.animator.AttackToIdle();
                        bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.idleState);
                    }
                }
                else
                {
                    return;
                }
                return;
            }
        }
        else
        {
            bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.patrollState); ;
        }
    }

    public void Exit()
    {

    }
}
