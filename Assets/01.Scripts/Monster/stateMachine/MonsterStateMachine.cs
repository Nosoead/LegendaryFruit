using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterStateMachine
{
    protected IMonster currentState { get; private set; }

    public GameObject target { get; set; }

    public Monster monster{ get; private set; }
    public PatrollState patrollState{ get; private set; }
    public AttackState attackState{ get; private set; }

    public void Update()
    {
        Excute();
    }

    public MonsterStateMachine(Monster monster)
    {
        this.monster = monster;
        
        this.patrollState = new PatrollState(this);
        this.attackState = new AttackState(this);
    }
    public void Initialize(IMonster monsterState) // 초기화
    {
        currentState = monsterState;
    }

    public void TransitionToState(IMonster nextState)
    {
        currentState?.Exit();
        currentState = nextState; // 다음 스테이트로
        currentState?.Enter();
    }

    public void MoveTowardsTarget()
    {
        Transform monsterTransform = monster.transform;
        Transform targetTransform = target.transform;
        var data = monster.Data;
        Vector2 direction = (targetTransform.position - monsterTransform.position).normalized;
        monsterTransform.position += (Vector3)(direction * data.moveSpeed * Time.deltaTime);
    }
    public void Excute()
    {
        if (currentState != null)
        {
            currentState?.Excute();
        }
    }
}
