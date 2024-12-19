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
        bossMonsterController.animator.OnIdle();
        bossMonsterController.animator.AttackToAttack(true);
        time = Time.deltaTime;
    }

    public void Execute()
    {
        time += Time.deltaTime;
        if(bossMonsterController.InAttackRange())
        {           
            if (time >= 1f)
            {
                time = 0f;
                bossMonsterController.animator.OnAttack();
                attackCount++;
                if (bossMonsterController.DetectPlayer() && attackCount >= 3)
                {
                    attackCount = 0;
                    bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.patternOneState);
                }
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
