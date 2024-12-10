using UnityEngine;

public class PatrollState : IMonster
{
    private MonsterController monsterController;
    private float idleTime;
    private float idleTimer = 0f;
    private float checkGroundTime = 0.1f;
    private float checkGroundTimer;
    public PatrollState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

    /// <summary>
    /// 이 함수는 머시기입니다
    /// </summary>
    
    public void Enter()
    {
        idleTime = Random.Range(5, 10);
        idleTimer = 0f;
       Debug.Log($"PatrollState Enter, idle time: {idleTime}");
    }

    public void Excute()
    {
        idleTimer += Time.deltaTime; //타이머
        monsterController.Move(); // chaseRange보다 거리가 멀면 그냥 돌아댕기게
        //Debug.Log($"Detector : {monsterController.DetectPlayer()}");
        // 조건따라 플레이어 서치 어택
        if (monsterController.DetectPlayer())
        {
            
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.attackState);
        }

        // n초동안 걷다가 idle 상태로 전환
        if (idleTimer >= idleTime)
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.idleState);
        }
        
        // 만약 땅이 없다면 
        if (!monsterController.monsterGround.GetOnGround())
        {
            checkGroundTimer += Time.deltaTime;
            if (checkGroundTimer >= checkGroundTime)
            {
                monsterController.ReverseDirection();
                checkGroundTimer = 0;
            }
        }
    }
    public void Exit()
    {
        Debug.Log("PatrollState Exit");
    }
}
