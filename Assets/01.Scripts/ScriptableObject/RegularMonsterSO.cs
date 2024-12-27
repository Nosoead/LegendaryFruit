using UnityEngine;

[CreateAssetMenu(fileName = "RegularMonsterSO", menuName = "ScriptableObject/RegularMonsterSO", order = 3)]
public class RegularMonsterSO : MonsterSO
{
    public AnimationClip defalutAttackClip; // 기본공격 모션
    public AnimationClip[] pattrenAttackClip; // 패턴공격 모션
    public float patternAttackChance;
}

