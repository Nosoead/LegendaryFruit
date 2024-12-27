using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSO", menuName = "ScriptableObject/MonsterSO", order = 3)]
public class MonsterSO : GameSO
{
    // ex ) 0001
    public MonsterType monsterType;
    public int monsterID;
    public float maxHealth; // 체력
    public float attackPower; // 공격력
    public float defense; // 방어력
    public float attackSpeed; //공격속도
    public float moveSpeed; // 이동속도
    public float attackDistance; // 공격거리
    public float chaseRange; // 추적거리
    public AttributeType type;  // 몬스터의 속성
    public float attributeValue; // 속성값
    public float attributeRateTime; // 속성데미지의 지속시간
    public float attributeStack;
    public float inGameMoney; // 드랍하는 머니
    public SpriteRenderer sprite; // 몬스터 이미지
    public AnimatorOverrideController animatorOverrideController; // 몬스터 애니메이션
    public AnimationClip defalutAttackClip; // 기본공격 모션
    public AnimationClip [] pattrenAttackClip; // 패턴공격 모션
    public float patternAttackChance;
}

public enum MonsterType
{
    Boss,
    Normal
}
