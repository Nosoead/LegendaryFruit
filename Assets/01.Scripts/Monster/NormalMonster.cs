using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    private MonsterStat stat;
    private void Awake()
    {
        stat = new MonsterStat();
    }

    private void Start()
    {
        stat.InitStat(MonsterSO);
    }
}
