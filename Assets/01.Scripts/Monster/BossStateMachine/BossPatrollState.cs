
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
        bossMonsterController.animator.OnMove();
        Debug.Log("패트롤상태 진입");
        idleTime = Random.Range(5, 10);
        idleTimer = 0f;
    }


    public void Execute()
    {
        if(bossMonsterController.DetectPlayer())
        {
            bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.attackState);
        }
        if(idleTimer >= idleTime)
        {
            bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.idleState);
        }
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

    }

    public void UpdateStat(BossMonsterController monsterController)
    {
        this.bossMonsterController = monsterController;
    }

}