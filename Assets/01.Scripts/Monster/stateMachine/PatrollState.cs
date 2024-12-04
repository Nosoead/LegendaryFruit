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
    }
    public void Excute()
    {
        // 조건따라 플레이어 서치 어택
    }
    public void Exit()
    {
    }
}
