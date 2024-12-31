using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossMonsterSO", menuName = "ScriptableObject/BossMonsterSO", order = 4)]
public class BossMonsterSO : MonsterSO
{
    public List<PatternData> pattrens;
}

[Serializable]
public class PatternData
{
    [Header("PattrenInfo")]
    public int pattrenID;
    public float pattrenCoolTime;
    public float patternDamage;
    public AnimationClip pattrenAttackAnimation;
}

