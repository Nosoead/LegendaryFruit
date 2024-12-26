using UnityEngine;

public class BossAttackState : IState
{
    private BossMonsterController bossMonsterController;

    private bool isGround;
    private int attackCount;
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
        time = 1;    
    }

    public void Execute()
    {
        time += Time.deltaTime;
        if(bossMonsterController.InAttackRange())
        {           
            if (time >= 1f)
            {
                time = 0f;
                bossMonsterController.animator.OnAttack(true);
                attackCount++;
                if (bossMonsterController.DetectPlayer() && attackCount >= 3)
                {
                    attackCount = 0;
                    bossMonsterController.animator.Delay(true);
                    bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.patternOneState);
                }
            }
            return;
        }
        if(!bossMonsterController.DetectPlayer())
        {
            bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.patrollState);
        }
    }

    public void Exit()
    {

    }
}
