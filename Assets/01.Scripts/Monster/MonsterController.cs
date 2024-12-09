using System;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    
    private MonsterStateMachine stateMachine;
    public MonsterStateMachine StateMachine => stateMachine;
    private AttributeLogics attributeLogics;
    private AttributeLogicsDictionary attributeLogicsDictionary;
    public MonsterGround monsterGround ;
    [SerializeField]private Monster monster;
    [SerializeField]private LayerMask playerLayerMask;
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
   
    public void Move()
    {
        Transform monsterTransfrom = transform;
        float direction = Mathf.Sign(monsterTransfrom.localScale.x);
        Vector2 moveDirection = new Vector2(direction, 0);
            
        monsterTransfrom.position += (Vector3)(moveDirection * (Monster.Data.moveSpeed * Time.deltaTime));
    }
   
    public void ReverseDirection()
    {
        //x축 반전
        Vector3 scale = transform.localScale;
        scale.x *= -1; 
        transform.localScale = scale;
    }
    public bool DetectPlayer()
    {
        
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right,chaseRange,playerLayerMask);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, -Vector2.right,chaseRange,playerLayerMask);
       Debug.Log(transform.position);
        return rightHit.collider != null || leftHit.collider != null;

    }

    public bool InAttackRange()
    {
        Debug.Log("InAttackRange");
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right,attackDistance,playerLayerMask);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, -Vector2.right,attackDistance,playerLayerMask);


        return rightHit.collider != null || leftHit.collider != null;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position , Vector2.right * chaseRange);
        Gizmos.DrawLine(transform.position , -Vector2.right * chaseRange);

    }
}
