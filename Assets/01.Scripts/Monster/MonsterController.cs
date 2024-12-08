using System;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    
    private MonsterStateMachine stateMachine;
    public MonsterStateMachine StateMachine => stateMachine;
    private AttributeLogics attributeLogics;
    private AttributeLogicsDictionary attributeLogicsDictionary;
    [SerializeField]private Monster monster;
    public Monster Monster => monster; //몬스터 데이터에 접근할수없어서 넣음 캐싱??

    private float maxHealth;
    private float attackPower;
    private float defense;
    private float MoveSpeed;
    private float attackDistance;
    private float chaseRange;
    private AttributeType type;
    private float attributeValue;
    private float inGameMoney;
    private Sprite sprite;
    private Animation animation;

    public GameObject target;
    /*public void SetMonster(Monster _monster)
    {
        monster = _monster;
    } */
    
    private void Awake()
    {
        //monster = GetComponent<Monster>();
        stateMachine = new MonsterStateMachine(this);
        attributeLogics = new NormalLogic(); //new AttributeLogics(); // 추상화클래스는 new를 할수없음
    }

    private void Start()
    {
        StateMachine.Initialize(StateMachine.patrollState);
    }

    private void Update()
    {
        StateMachine.Excute();
    }

    private void OnHit()
    {
        //어트리뷰트에서 데미지계산후 딕셔너리에 저장후 꺼내옴
        attributeLogicsDictionary.GetAttributeLogic(monster.Data.type);
        Debug.Log($"Hit, type : {monster.Data.type}");
        //애니메이션
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnHit();
        }
    }
}
