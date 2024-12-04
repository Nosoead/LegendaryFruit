using System.Collections;
using System.Collections.Generic;
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
        stateMachine.MoveTowardsTarget();
        // 몬스터의 거리와 타겟(플레이어)의 거리를 계산해 저장
        float distanceToPlayer = Vector2.Distance(
            stateMachine.monster.transform.position,
            stateMachine.target.transform.position
        );

        // chaseRange보다 거리가 가까워진다면 attackState로 전환
        if (distanceToPlayer < stateMachine.monster.Data.chaseRange)
        {
            stateMachine.TransitionToState(stateMachine.attackState);
            return;
        }
        stateMachine.MoveTowardsTarget();
        // 조건따라 플레이어 서치 어택
    }
    public void Exit()
    {
        Debug.Log("PatrollState Exit");
    }
}
