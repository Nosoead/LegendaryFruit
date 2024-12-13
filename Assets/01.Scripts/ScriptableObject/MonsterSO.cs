using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSO", menuName = "ScriptableObject/MonsterSO", order = 3)]
public class MonsterSO : GameSO
{
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
    public Sprite sprite; // 몬스터 이미지
    public Animation animation; // 몬스터 애니메이션
    public GameObject target;
}
