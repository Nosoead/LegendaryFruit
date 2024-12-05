using System;
using UnityEngine;

public class PatrollState : IMonster
{
    private MonsterStateMachine stateMachine;

    public PatrollState(MonsterStateMachine stateMachine)
    {
        this. stateMachine =  stateMachine;
    }

    public void Enter()
    {
       Debug.Log("PatrollState Enter");
    }
    public void Excute()
    {
        // 몬스터의 거리와 타겟(플레이어)의 거리를 계산해 저장
        float distanceToPlayer = Vector2.Distance(
            stateMachine.monster.transform.position,
            stateMachine.monster.Data.target.transform.position
        );

        if (distanceToPlayer < 5)
        {
            Debug.Log("가까워짐");
        }
        // chaseRange보다 거리가 가까워진다면 attackState로 전환
        if (distanceToPlayer < stateMachine.monster.Data.chaseRange) 
        {
            stateMachine.TransitionToState(stateMachine.attackState);
        }
        // chaseRange보다 거리가 멀면 그냥 돌아댕기게
        if (distanceToPlayer > stateMachine.monster.Data.chaseRange)
        {
            stateMachine.Move();
            // 플레이어 거리 계산하지말고 그냥 앞으로 가다가 아래 땅이 없으면 돌게
        }
        // 조건따라 플레이어 서치 어택
    }
    public void Exit()
    {
        Debug.Log("PatrollState Exit");
    }
}
