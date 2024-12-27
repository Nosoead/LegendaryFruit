using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class BossMonsterController : MonoBehaviour 
{
    private BossStateMachine stateMachine;  
    public MonsterAnimationController animator;
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
        animator = GetComponent<MonsterAnimationController>();    
        attributeLogicsDictionary = new MonsterAttributeLogicsDictionary();
        attributeLogicsDictionary.Initialize();
        stateMachine = new BossStateMachine(this);     
    }

    private void OnEnable()
    {
        statManager.OnSubscribeToStatUpdateEvent += OnStatUpdatedEvent;
    }

    private void OnDisable()
    {
        statManager.OnSubscribeToStatUpdateEvent -= OnStatUpdatedEvent;
    }

    private void Start()
    {
        StateMachine.Initialize(StateMachine.patrollState);
    }

    private void Update()
    {
        StateMachine.Excute();
    }

    private void OnStatUpdatedEvent(string statKey, float value)
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

    public void ReverseDirection() // 방향반전
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        lookDirection *= -1f;
        transform.localScale = scale;
    }
    public bool DetectPlayer() // 플레이어 발견
    {
        Vector3 raytDirection = Vector3.right * lookDirection;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raytDirection, chaseRange, playerLayerMask);
        Color rayColor = (hit.collider != null) ? Color.green : Color.red;
        Debug.DrawRay(transform.position, raytDirection * chaseRange, rayColor);
        if (hit.collider != null)
        {
            target = hit.collider.gameObject;
            return true;
        }

        return false;
    }

    public void LookAtPlayer()
    {
        if (target != null)
        {
            float dir = target.transform.position.x - transform.position.x;
            if ((dir > 0 && lookDirection < 0) || (dir < 0 && lookDirection > 0))
            {
                ReverseDirection();
            }
        }
        else
        {
            return;
        }
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

    public void Move()
    {
        Vector3 rayDirection = Vector3.right * lookDirection;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, chaseRange, playerLayerMask);

        if (hit.collider == null || hit.distance > attackDistance)
        {
            transform.position += rayDirection * (moveSpeed * Time.deltaTime);
        }
    }

    public void Attack()
    {
        Vector2 monsterPosition = monsterPosition = transform.position;
        Vector2 boxPosition = monsterPosition + Vector2.right * (1f * lookDirection);
        Vector2 boxSize =  new Vector2(5f, 3f);
        Collider2D player = Physics2D.OverlapBox(boxPosition, boxSize, 0, playerLayerMask);
        if (player == null)
        {
            return;
        }
        monsterAttributeLogics.ApplyAttackLogic(player.gameObject, attackPower, attributeValue, attributeRateTime, attributeStack);
    }

    public void AreaAttack()
    {
        Vector2 monsterPosition = transform.position;
        Vector2 boxPosition = monsterPosition + Vector2.right * (0f * lookDirection);
        Vector2 boxSize = new Vector2(12f, 3f);
        Collider2D player = Physics2D.OverlapBox(boxPosition, boxSize, 0, playerLayerMask);
        if (player == null)
        {
            return;
        }
        monsterAttributeLogics.ApplyAttackLogic(player.gameObject, attackPower + 5, attributeValue, attributeRateTime, attributeStack);
    }

    private void OnDrawGizmos()
    {
        Vector2 boxSize = new Vector2(12f, 3f);        
        Gizmos.color = Color.red;
        Vector2 boxPosition = (Vector2)transform.position + Vector2.right * 0.5f * lookDirection;
        Gizmos.DrawWireCube(boxPosition, boxSize);
        OnDrawGizmos2();
    }
    private void OnDrawGizmos2()
    {
        Vector2 boxSize = new Vector2(5f, 3f);
        Gizmos.color = Color.red;
        Vector2 boxPosition = (Vector2)transform.position + Vector2.right * 1f * lookDirection;
        Gizmos.DrawWireCube(boxPosition, boxSize);
    }

}
