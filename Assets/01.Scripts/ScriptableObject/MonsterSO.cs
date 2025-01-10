using UnityEngine;

public class MonsterSO : GameSO
{
    // ex ) 0001
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
    public RangedAttackData monsterRagnedAttackData;
    public SpriteRenderer sprite; // 몬스터 이미지
    public AnimatorOverrideController animatorOverrideController; // 몬스터 애니메이션
}

