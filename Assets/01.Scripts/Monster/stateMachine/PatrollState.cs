using UnityEngine;

public class PatrollState : IState
{
    private MonsterController monsterController;
    private float idleTime;
    private float idleTimer = 0f;
    private float checkGroundTime = 1f;
    private float checkGroundTimer;
    private float moveSpeed;
    private bool isGround;
    public PatrollState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

    /// <summary>
    /// 이 함수는 머시기입니다
    /// </summary>
    
    public void Enter()
    {
        monsterController.animationController.OnMove(true);
        idleTime = Random.Range(5, 10);
        idleTimer = 0f;
    }

    public void Execute()
    {
        idleTimer += Time.deltaTime; //타이머
         // chaseRange보다 거리가 멀면 그냥 돌아댕기게
        //Debug.Log($"Detector : {monsterController.DetectPlayer()}");
        // 조건따라 플레이어 서치 어택
        if (monsterController.DetectPlayer())
        {
            monsterController.Move();
            if(monsterController.InAttackRange())
            {
                monsterController.StateMachine.TransitionToState(monsterController.StateMachine.attackState);
            }
            else
            {
                return;
            }
        }

        // n초동안 걷다가 idle 상태로 전환
        if (idleTimer >= idleTime)
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.idleState);
        }

        if (monsterController.monsterGround.GetOnGround())
        {
            isGround = false;
            monsterController.Move();
        }
        else
        {
            if (!isGround)
            {
                monsterController.ReverseDirection();
                isGround = true;
            }
            monsterController.Move();
        }
    }
    public void Exit()
    {
        monsterController.animationController.OnMove(false);
    }
   

    public void UpdateStat(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }  
}
