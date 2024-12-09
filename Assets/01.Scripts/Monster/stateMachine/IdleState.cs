using System.Collections;
using UnityEngine;

public class IdleState :IMonster
{
    private MonsterController monsterController;
    private float idleTime;
    private float idleTimer = 0f;

    public IdleState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

    public void Enter()
    {
        idleTime = Random.Range(1, 5);
        idleTimer = 0f;
        Debug.Log($"Idle Enter, idle Time : {idleTime:N2}");
    }

    public void Excute()
    {
        idleTimer += Time.deltaTime; //타이머
        //플레이어와의 거리 확인
       
        // n초간 가만히 있다가 패트롤 스테이트로 전환
        if (idleTimer >= idleTime)
        {
            if (Random.value < 0.3f) // 가만히있다가 가끔 방향 전환
            {
                monsterController.ReverseDirection(); 
                Debug.Log("방향전환");
            }
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.patrollState);
            return;
        }
        // 가만히 있는데 플레이어가 들어오면 어택스테이트로 변환
        if (monsterController.DetectPlayer())
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.attackState);
            return;
        }
    }

    public void Exit()
    {
        Debug.Log("Idle Exit");
    }
}