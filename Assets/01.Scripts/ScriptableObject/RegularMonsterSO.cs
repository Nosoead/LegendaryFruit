using UnityEngine;

[CreateAssetMenu(fileName = "RegularMonsterSO", menuName = "ScriptableObject/RegularMonsterSO", order = 3)]
public class RegularMonsterSO : MonsterSO
{
    public AnimationClip defalutAttackClip;
    public AnimationClip[] pattrenAttackClip;
    public float patternAttackChance;
}

