using UnityEngine;

public class PatrollState : IMonster
{
    private MonsterController monstercontroller;
    private float idleTime;
    private float idleTimer = 0f;

    public PatrollState(MonsterController monstercontroller)
    {
        this.monstercontroller = monstercontroller;
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
        // 몬스터의 거리와 타겟(플레이어)의 거리를 계산해 저장
        float distanceToPlayer = Vector2.Distance(
            monstercontroller.transform.position,
            monstercontroller.Monster.Data.target.transform.position
        );
        // chaseRange보다 거리가 가까워진다면 attackState로 전환
        if (distanceToPlayer < monstercontroller.Monster.Data.chaseRange) 
        {
            monstercontroller.StateMachine.TransitionToState(monstercontroller.StateMachine.attackState);
        }
        // chaseRange보다 거리가 멀면 그냥 돌아댕기게
        if (distanceToPlayer > monstercontroller.Monster.Data.chaseRange)
        {
            monstercontroller.StateMachine.Move();
            // n초동안 걷다가 idle 상태로 전환
            if (idleTimer >= idleTime)
            {
                monstercontroller.StateMachine.TransitionToState(monstercontroller.StateMachine.idleState);
            }
            // 플레이어 거리 계산하지말고 그냥 앞으로 가다가 아래 땅이 없으면 돌게
        }
        // 조건따라 플레이어 서치 어택
    }
    public void Exit()
    {
        Debug.Log("PatrollState Exit");
    }
}
