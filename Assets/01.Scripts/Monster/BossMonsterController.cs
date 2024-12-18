using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class BossMonsterController : MonoBehaviour 
{
    // TODO: 일단은 복제해서 쓰는데 나중에 몬스터와 보스몬스터 리펙토링 필요
    private BossStateMachine stateMachine;
    public BossStateMachine StateMachine => stateMachine;
    private MonsterAttributeLogics monsterAttributeLogics = null;
    private MonsterAttributeLogics burnAttributeLogics = null;
    private MonsterAttributeLogicsDictionary attributeLogicsDictionary;
    public MonsterGround monsterGround;
    [SerializeField] private LayerMask playerLayerMask;
    public GameObject target;
    public MonsterStatManager statManager;
    private float lookDirection = 1f;

    private float attackPower;
    private float moveSpeed;
    private float attackDistance;
    private float chaseRange;
    private AttributeType type;
    private float attributeValue;
    private float attributeRateTime;
    private float attributeStack;

    private void Awake()
    {
        attributeLogicsDictionary = new MonsterAttributeLogicsDictionary();
        attributeLogicsDictionary.Initialize();
        stateMachine = new BossStateMachine(this);       //new AttributeLogics(); // 추상화클래스는 new를 할수없음
    }

    private void Start()
    {
        statManager.SubscribeToStatUpdateEvent(UpdateStat);
        statManager.SetInitStat();
        StateMachine.Initialize(StateMachine.patrollState);
    }

    private void OnDisable()
    {
        statManager.UnsubscribeToStatUpdateEvent(UpdateStat);
    }
    private void Update()
    {
        StateMachine.Excute();
    }

    private void UpdateStat(string statKey, float value)
    {
        switch (statKey)
        {
            case "CurrentAttackPower":
                attackPower = value;
                break;
            case "AttributeType":
                type = (AttributeType)((int)value);
                monsterAttributeLogics = attributeLogicsDictionary.GetAttributeLogic(type);
                break;
            case "MoveSpeed":
                moveSpeed = value;
                break;
            case "ChaseRange":
                chaseRange = value;
                break;
            case "AttackDistance":
                attackDistance = value;
                break;
            case "AttributeValue":
                attributeValue = value;
                break;
            case "AttributeRateTime":
                attributeRateTime = value;
                break;
            case "AttributeStack":
                attributeStack = value;
                break;
        }
    }

    public void StatToStateMachine()
    {
        stateMachine.UpdateStat(this);  
    }

    public void ReverseDirection() // 방향 반전
    {
        //x축 반전
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        lookDirection *= -1f;
        transform.localScale = scale;
    }
    public bool DetectPlayer() //플레이어 찾는 레이
    {
        Vector3 raytDirection = Vector3.right * lookDirection;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raytDirection, chaseRange, playerLayerMask);

        if (hit.collider != null)
        {
            target = hit.collider.gameObject;
            return true;
        }

        return false;
    }

    public bool InAttackRange()
    {
        Vector3 rayDirection = Vector3.right * lookDirection;
        Color rayColor = Color.red;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, attackDistance, playerLayerMask);
        if (hit.collider != null)
        {
            rayColor = Color.green;
            target = hit.collider.gameObject;
            return true;
        }
        Debug.DrawRay(transform.position, rayDirection * attackDistance, rayColor, 0.1f);
        return false;
    }

    //public bool InAttackRange() // 플레이어가 사거리 안에 오면 활성화
    //{
    //    Vector3 raytDirection = Vector3.right * lookDirection;
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, raytDirection, attackDistance, playerLayerMask);
    //    if (hit.collider != null)
    //    {
    //        target = hit.collider;
    //        return true;
    //    }

    //    return false;
    //}
    public void Move()
    {
        Vector3 rayDirection = Vector3.right * lookDirection;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, chaseRange, playerLayerMask);

        if (hit.collider == null || hit.distance > 0.1f)
        {
            transform.position += rayDirection * (moveSpeed * Time.deltaTime);
        }
        else
        {
            ReverseDirection();
        }
    }

    public void Attack(bool isNormalAttack)
    {
        Vector2 monsterPosition;
        Vector2 boxPosition;
        Vector2 boxSize;
        if (isNormalAttack)
        {
            monsterPosition = transform.position;
            boxPosition = monsterPosition + Vector2.right * (1f * lookDirection);
            boxSize = new Vector2(1f,1f);
        }
        else
        {
            monsterPosition = transform.position;
            boxPosition = monsterPosition + Vector2.right * (1f * lookDirection);
            boxSize = new Vector2(4f,4f);
        }
        Collider2D player = Physics2D.OverlapBox(boxPosition, boxSize, 0, playerLayerMask);
        if (player == null)
        {
            return;
        }
        if(isNormalAttack)
        {
            monsterAttributeLogics.ApplyAttackLogic(player.gameObject, attackPower, attributeValue, attributeRateTime, attributeStack);
        }
        else
        {
            monsterAttributeLogics.ApplyAttackLogic(player.gameObject, attackPower + 2, attributeValue,attributeRateTime,attributeStack);
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 boxSize = new Vector2(1f, 1f);        
        Gizmos.color = Color.red;
        Vector2 boxPosition = (Vector2)transform.position + Vector2.right * 4f * lookDirection;
        Gizmos.DrawWireCube(boxPosition, boxSize);
    }

}
