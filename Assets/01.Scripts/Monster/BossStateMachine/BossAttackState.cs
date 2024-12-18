using UnityEngine;

public class BossAttackState : IState
{
    private BossMonsterController bossMonsterController;

    private bool isGround;
    float time = 0f; 


    public BossAttackState(BossMonsterController bossMonsterController)
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
        Debug.Log("공격상태 진입!");
        
    }

    public void Execute()
    {
        if(bossMonsterController.InAttackRange())
        {
            time += Time.deltaTime;
            if (time >= 0.8f)
            {
                time = 0f;
                bossMonsterController.Attack(true);
                bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.patternOneState);
            }
            return;
        }
        if(!bossMonsterController.DetectPlayer())
        {
            bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.idleState);
        }
        if (bossMonsterController.DetectPlayer())
        {
            if (bossMonsterController.monsterGround.GetOnGround())
            {
                bossMonsterController.Move();
            }
        }
        if (!bossMonsterController.monsterGround.GetOnGround())
        {
            bossMonsterController.ReverseDirection();
        }
    }

    public void Exit()
    {
        
    }
}
