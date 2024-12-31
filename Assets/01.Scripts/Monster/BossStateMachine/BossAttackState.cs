using UnityEngine;

public class BossAttackState : IState
{
    private BossMonsterController bossMonsterController;

    private bool isGround;


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
        bossMonsterController.animator.BossDefalutAttack(true);

    }

    public void Execute()
    {
        if(!bossMonsterController.InAttackRange())
        {
            bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.patrollState);
        }
    }

    public void Exit()
    {
        bossMonsterController.animator.BossDefalutAttack(false);
    }
}
