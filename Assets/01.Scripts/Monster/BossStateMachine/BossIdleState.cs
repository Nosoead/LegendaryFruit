using System.Collections;
using UnityEngine;

public class BossIdleState : IState
{
    private BossMonsterController bossMonsterController;
    private float idleTime;
    private float idleTimer = 0f;

    public BossIdleState(BossMonsterController bossMonsterController)
    {
        this.bossMonsterController = bossMonsterController;
    }

    public void Enter()
    {
        bossMonsterController.animator.OnIdle();
        idleTime = Random.Range(1, 5);
        idleTimer = 0f;
    }

    public void Execute()
    {
        idleTimer += Time.deltaTime;

        // n초간 가만히 있다가 패트롤 스테이트로 전환
        if (idleTimer >= idleTime)
        {
            if (Random.value < 0.3f) // 가만히있다가 가끔 방향 전환
            {
                bossMonsterController.ReverseDirection();
            }
            bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.patrollState);
            return;
        }
        //// 가만히 있는데 플레이어가 들어오면 어택스테이트로 변환
        //if (bossMonsterController.DetectPlayer())
        //{
        //    bossMonsterController.StateMachine.TransitionToState(bossMonsterController.StateMachine.attackState);
        //    return;
        //}
    }

    public void Exit()
    {
        
    }

    public void UpdateStat(BossMonsterController bossMonsterController)
    {
        this.bossMonsterController = bossMonsterController;
    }
}
