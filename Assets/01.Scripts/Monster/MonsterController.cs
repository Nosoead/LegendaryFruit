using System;
using System.Collections;
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
    public GameObject target;
    public Monster Monster => monster; //몬스터 데이터에 접근할수없어서 넣음 캐싱??
    public MonsterStatManager statManager;

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
    private float checkGroundTime = 0.1f;
    private float checkGroundTimer;

   

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
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector3.right,10,playerLayerMask);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector3.left,10,playerLayerMask);
        return rightHit.collider != null || leftHit.collider != null;
    }

    public bool InAttackRange()
    {
        //Debug.Log("InAttackRange");
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector3.right,2,playerLayerMask);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector3.left,2,playerLayerMask);
        return rightHit.collider != null || leftHit.collider != null;
    }

    public void MoveToTarget()
    {
        // 땅이 없으면 바로 뒤로 걷다가 > reverse,move 1초후 다시 추적
        // 땅이 있으면 그냥 추적
        if (monsterGround.GetOnGround())
        {
            var data = monster.Data;
            Transform monsterTransform = monster.transform;
            Transform targetTransform = data.target.transform;
        
            Vector2 direction = (targetTransform.position - monsterTransform.position).normalized;
            monsterTransform.position += (Vector3)(direction * (data.moveSpeed * Time.deltaTime));
        }
    }


    public void OnDrawGizmos()
    {
        /*Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position , Vector3.right * 3);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position , Vector3.left * 3);*/

    }
}
