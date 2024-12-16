using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MonsterController : MonoBehaviour
{
    public MonsterAnimationController animationController;
    private MonsterStateMachine stateMachine;
    public MonsterStateMachine StateMachine => stateMachine;
    private MonsterAttributeLogics monsterAttributeLogics = null;
    private MonsterAttributeLogicsDictionary attributeLogicsDictionary;
    public MonsterGround monsterGround;
    [SerializeField] private LayerMask playerLayerMask;
    public GameObject target;
    public MonsterStatManager statManager;
    private Vector2 boxSize = new Vector2(1f, 1f);
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
        stateMachine = new MonsterStateMachine(this);
        monsterAttributeLogics = new MonsterNormal(); //new AttributeLogics(); // 추상화클래스는 new를 할수없음
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

    public bool InAttackRange() // 플레이어가 사거리 안에 오면 활성화
    {
        Vector3 raytDirection = Vector3.right * lookDirection;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raytDirection, attackDistance, playerLayerMask);
        if (hit.collider != null)
        {
            target = hit.collider.gameObject;
            return true;
        }

        return false;
    }
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

    public void Attack()
    {
        //어트리뷰트에서 데미지계산후 딕셔너리에 저장후 꺼내옴
        // scale.x가 0보다 크면 우 작으면 좌
        Vector2 monsterPosition = transform.position;
        Vector2 boxPostion = monsterPosition + Vector2.right * (0.5f * lookDirection);
        Collider2D player = Physics2D.OverlapBox(boxPostion, boxSize, 0, playerLayerMask);
        if (player == null)
        {
            return;
        }
        Debug.Log(type);
        monsterAttributeLogics.ApplyAttackLogic(player.gameObject, attackPower, attributeValue, attributeRateTime, attributeStack);

        //애니메이션
        animationController.OnAttack();
    }

    /*public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position , Vector3.right * 10);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position , Vector3.left * 10);
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 boxPosition = (Vector2)transform.position + Vector2.right * 0.5f * lookDirection;
        Gizmos.DrawWireCube(boxPosition, boxSize);
    }
}
