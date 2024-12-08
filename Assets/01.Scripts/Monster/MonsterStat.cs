using UnityEngine;

public class MonsterStat : Stat
{

    public float maxHealth {get; set; }
    public float attackPower{get; set; }
    public float defense {get; set; }
    public float MoveSpeed{get; set; }
    public float attackDistance{get; set; }
    public float chaseRange{get; set; }
    public AttributeType type{get; set; }
    public float attributeValue{get; set; }
    public float inGameMoney{get; set; }
    public Sprite sprite{get; set; }
    public Animation animation{get; set; }
    public GameObject target{get; set; }
    public override void InitStat(GameSO gameData)
    {
        if(gameData is MonsterSO monsterData)
        {
            maxHealth = monsterData.maxHealth;
            attackPower = monsterData.attackPower;
            defense = monsterData.defense;
            MoveSpeed = monsterData.moveSpeed;
            attackDistance = monsterData.attackDistance;
            chaseRange = monsterData.chaseRange;
            type = monsterData.type;
            attributeValue = monsterData.attributeValue;
            inGameMoney = monsterData.inGameMoney;
            sprite = monsterData.sprite;
            animation = monsterData.animation;
            target = monsterData.target;
        }
    }
}
