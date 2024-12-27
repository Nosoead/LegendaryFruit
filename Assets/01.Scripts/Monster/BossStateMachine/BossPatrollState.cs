
using UnityEngine;


public class BossPatrollState : IState
{
    private BossMonsterController bossMonsterController;
    private float idleTime;
    private float idleTimer = 0f;
    private bool isGround = false;

    public BossPatrollState(BossMonsterController bossMonsterController)
    {
        this.bossMonsterController = bossMonsterController;
    }

    public void Enter()
    {
        bossMonsterController.animator.OnMove(true);
        Debug.Log("패트롤상태 진입");
    }


    public void Execute()
    {
        if(bossMonsterController.DetectPlayer())
        {
            bossMonsterController.LookAtPlayer();
            return;
        }

        //if(bossMonsterController.InAttackRange())
        //{
        //    bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.attackState);
        //}

        if (bossMonsterController.monsterGround.GetOnGround())
        {
            isGround = false;
            bossMonsterController.Move();
        }
        else
        {
            if (!isGround)
            {
                bossMonsterController.ReverseDirection();
                isGround = true;
            }
            bossMonsterController.Move();
        }

    }

    public void Exit()
    {
        bossMonsterController.animator.OnMove(false);
    }

    public void UpdateStat(BossMonsterController monsterController)
    {
        this.bossMonsterController = monsterController;
    }

}