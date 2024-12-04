using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterStateMachine : MonoBehaviour
{
    protected IMonster CurrentState { get; private set; }

    public PatrollState PatrollState;
    public AttackState AttackState;

    public void Update()
    {
        CurrentState?.Excute();
    }

    public MonsterStateMachine(MonsterStateMachine stateMachine)
    {
        this.PatrollState = new PatrollState(stateMachine);
        this.AttackState = new AttackState(stateMachine);
    }
    public void Initialize(IMonster monster)
    {
        CurrentState = PatrollState;
    }

    public void TransitionToState()
    {
        CurrentState?.Exit();
        CurrentState = AttackState; // 다음 스테이트로
        CurrentState?.Enter();
    }
    
}
