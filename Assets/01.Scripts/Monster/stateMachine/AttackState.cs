using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState :IMonster
{
    private MonsterStateMachine stateMachine;

    public AttackState(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        
    }

    public void Excute()
    {
        // ChaseRange 넘으면 Patroll로
        // ChaseRange>AttackDistance -> 따라가기(좌우로만)
        // currentDistance <AttackDistance-->때리기
        // if-> 때리는 애니메이션 ->애니메이션에서 OnHit
    }

    public void Exit()
    {
        
    }
}
