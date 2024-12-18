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
        Debug.Log("��������");
    }

    public void Execute()
    {
        if (bossMonsterController.monsterGround.GetOnGround())
        {
            time += Time.deltaTime;
            if(time >=  2f)
            {
                time = 0f;
                Debug.Log("ȭ�����!");
                bossMonsterController.Attack(false);
                bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.attackState);
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
