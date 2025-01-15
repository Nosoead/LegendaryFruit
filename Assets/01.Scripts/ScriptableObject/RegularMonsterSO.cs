using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegularMonsterSO", menuName = "ScriptableObject/RegularMonsterSO", order = 1)]
public class RegularMonsterSO : MonsterSO
{
    public List<RegularPatternData> patterns = new List<RegularPatternData>();
    public AnimationClip defalutAttackClip;
}

[System.Serializable]
public class RegularPatternData
{
    public AttributeType patternAttributeType;
    public float patternDamage;
    public AnimationClip pattrenAttackClip;
    public float patternAttackChance;
    public float patternAttributeValue;
    public float patternAttributeRateTime;
    public float patternAttributeStack;
}

