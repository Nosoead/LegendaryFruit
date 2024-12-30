using UnityEngine;

[CreateAssetMenu(fileName = "BossMonsterSO", menuName = "ScriptableObject/BossMonsterSO", order = 4)]
public class BossMonsterSO : MonsterSO
{
    [Header("Pattren")]
    public float patternDamage;
    public AnimationClip pattrenAttack;
    public float pattrenRange;
    public Vector2 overlapBoxSize;
}

