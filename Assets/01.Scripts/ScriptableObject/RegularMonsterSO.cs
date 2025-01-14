using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegularMonsterSO", menuName = "ScriptableObject/RegularMonsterSO", order = 1)]
public class RegularMonsterSO : MonsterSO
{
    public List<RegularPatternData> patterns;
    public AnimationClip defalutAttackClip;
}

[System.Serializable]
public class RegularPatternData
{
    public AttributeType patternAttributeType;
    public float patternDamage;
    public AnimationClip[] pattrenAttackClip;
    public float patternAttackChance;
}

