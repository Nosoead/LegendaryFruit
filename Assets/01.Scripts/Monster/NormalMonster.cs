using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{  
    private MonsterSO monsterdata;
    private MonsterStateMachine stateMachine;
    private MonsterStat stat;
    private void Awake()
    {
        stateMachine = new MonsterStateMachine(this);
        stat = new MonsterStat();
    }

    private void Start()
    {
        stateMachine.TransitionToState(stateMachine.patrollState);
        stat.InitStat(MonsterData);
    }

}
