using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSO", menuName = "ScriptableObject/MonsterSO", order = 3)]
public class MonsterSO : GameSO
{
    public float maxHealth;
    public float attackPower;
    public float defense;
    public float attackSpeed;
    public float moveSpeed;
    public float attackDistance;
    public float chaseRange;
    public AttributeType type; 
    public float attributeValue;
    public float attributeRateTime;
    public float inGameMoney;
    public Sprite sprite;
    public Animation animation;
}
