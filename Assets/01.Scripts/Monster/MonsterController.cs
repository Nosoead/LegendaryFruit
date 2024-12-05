/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : Monster
{
    public MonsterStateMachine StatMachine => stateMachine;
    private AttributeLogicState attributeLogicState;

    private void Awake()
    {
        MonstersStateMachine = new MonsterStateMachine(this);
        attributeLogicState = new AttributeLogicState();
    }

    private void Start()
    {
        MonstersStateMachine.Inisialize(MonsterStateMachine.patrollState);
    }

    private void Update()
    {
        MonstersStateMachine.Execute();
    }

    private void OnHit()
    {
        //애니메이션에서 이벤트로 불러와짐.
    }
}
*/
