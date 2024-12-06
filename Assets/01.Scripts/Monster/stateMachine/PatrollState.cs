using System;
using UnityEngine;

public class PatrollState : IMonster
{
    private MonsterController monster;

    public PatrollState(MonsterController monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
       Debug.Log("PatrollState Enter");
    }
    public void Excute()
    {
        Debug.Log($"{monster.transform}1");
        if (monster.Monster == null)
        {
            Debug.Log($"{monster.Monster}2");
        }
        
        // 몬스터의 거리와 타겟(플레이어)의 거리를 계산해 저장
        float distanceToPlayer = Vector2.Distance(
            monster.transform.position,
            monster.Monster.Data.target.transform.position
            
        );

        if (distanceToPlayer < 5)
        {
            Debug.Log("가까워짐");
        }
        // chaseRange보다 거리가 가까워진다면 attackState로 전환
        if (distanceToPlayer < monster.Monster.Data.chaseRange) 
        {
            monster.StateMachine.TransitionToState(monster.StateMachine.attackState);
        }
        // chaseRange보다 거리가 멀면 그냥 돌아댕기게
        if (distanceToPlayer > monster.Monster.Data.chaseRange)
        {
            monster.StateMachine.Move();
            // 플레이어 거리 계산하지말고 그냥 앞으로 가다가 아래 땅이 없으면 돌게
        }
        // 조건따라 플레이어 서치 어택
    }
    public void Exit()
    {
        Debug.Log("PatrollState Exit");
    }
}
