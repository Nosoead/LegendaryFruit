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
    private PatternData pattren;
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
    private bool canMove = true;

    // Pattren Info
    private Vector2 overlapBoxSize;
    private float patternDamage;
    private PatternData currentPattern;

    #region Lifecycle Functions
    private void Awake()
    {
        animator = GetComponent<MonsterAnimationController>();    
        attributeLogicsDictionary = new MonsterAttributeLogicsDictionary();
        attributeLogicsDictionary.Initialize();
        stateMachine = new BossStateMachine(this);
        target = GameObject.FindWithTag("Player");
    }

    private void OnEnable()
    {
        statManager.OnSubscribeToStatUpdateEvent += OnStatUpdatedEvent;
        statManager.OnPatternTriggered += OnHealthThresholdReached;
    }

    private void OnDisable()
    {
        statManager.OnSubscribeToStatUpdateEvent -= OnStatUpdatedEvent;
        statManager.OnPatternTriggered -= OnHealthThresholdReached;
    }

    private void Start()
    {
        StateMachine.Initialize(StateMachine.patrollState);
    }

    private void Update()
    {
        StateMachine.Excute();
    }
    #endregion

    #region BossMonsterStat
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

    public void OnHealthThresholdReached(PatternData patternData,float damage)
    {
        if(patternData != currentPattern && damage != patternDamage)
        {
            currentPattern = patternData;
            animator.GetPatternData(currentPattern);
            patternDamage = damage;
        }
        stateMachine.TransitionToState(stateMachine.pattrenState);
    }    

    public void StatToStateMachine()
    {
        stateMachine.UpdateStat(this);  
    }
    #endregion

    #region Look
    public void ReverseDirection() // 방향반전
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        lookDirection *= -1f;
        transform.localScale = scale;
    }

    public bool LookAtPlayer()
    {
        if (target != null)
        {
            float dir = target.transform.position.x - transform.position.x;
            if ((dir > 0 && lookDirection < 0) || (dir < 0 && lookDirection > 0))
            {
                ReverseDirection();
                return true;
            }
        }
        else
        {
            target = GameObject.FindWithTag("Player");
        }
        return false;
    }
    #endregion

    #region DefaultAttack
    public bool InAttackRange()
    {
        Vector2 monsterPosition = monsterPosition = transform.position;
        Vector2 boxPosition = monsterPosition + Vector2.right * attackDistance * (1f * lookDirection)
                        + Vector2.down * 1.5f;
        Vector2 boxSize = new Vector2(attackDistance, 4);
        Collider2D player = Physics2D.OverlapBox(boxPosition, boxSize, 0, playerLayerMask);
        if (player == null)
        {
            return false;
        }
        return true;
    }
    public void Attack()
    {
        Vector2 monsterPosition = transform.position;
        Vector2 boxPosition = monsterPosition + Vector2.right * attackDistance * (1f * lookDirection)
                        + Vector2.down * 1.5f;
        Vector2 boxSize = new Vector2(attackDistance, 4);
        Collider2D player = Physics2D.OverlapBox(boxPosition, boxSize, 0, playerLayerMask);
        if (player == null)
        {
            return;
        }
        monsterAttributeLogics.ApplyAttackLogic(player.gameObject, attackPower, attributeValue, attributeRateTime, attributeStack);
    }
    #endregion

    #region PattrenAttack

    public bool InPattrenAttackRange()
    {
        Vector2 monsterPosition = transform.position;
        Vector2 boxPosition = monsterPosition + Vector2.right * (1f * lookDirection)
                        + Vector2.down * 1.5f;
        Vector2 boxSize = new Vector2(10f, 10f);
        Collider2D player = Physics2D.OverlapBox(boxPosition, boxSize, 0, playerLayerMask);
        if (player == null)
        {
            return false;
        }
        return true;
    }

    public void PattrenAttack()
    {
        Vector2 monsterPosition = transform.position;
        Vector2 boxPosition = monsterPosition + Vector2.right * attackDistance * (1f * lookDirection)
                        + Vector2.down * 1.5f;
        Collider2D player = Physics2D.OverlapBox(boxPosition, overlapBoxSize, 0, playerLayerMask);
        if (player == null)
        {
            return;
        }
        monsterAttributeLogics.ApplyAttackLogic(player.gameObject, attackPower, attributeValue, attributeRateTime, attributeStack);
    }
    #endregion

    #region Movement
    public void Move()
    {
        var dir = Vector3.Distance(transform.position, target.transform.position);
        if (dir > attackDistance)
        {
            canMove = true;
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += moveDirection * (moveSpeed * Time.deltaTime); 
        }
        else
        {
            canMove = false;
        }
    }
    #endregion 

    private void OnDrawGizmos()
    {
        // 몬스터 위치
        Vector2 monsterPosition = transform.position;

        // 박스 중심 위치
        Vector2 boxPosition = monsterPosition + Vector2.right  * (1 * lookDirection)
            + Vector2.down * 1.5f;

        // 박스 크기
        Vector2 boxSize = new Vector2(10, 10);

        // Gizmos 색상 설정
        Gizmos.color = Color.blue;

        // 박스 그리기
        Gizmos.DrawWireCube(boxPosition, boxSize);
    }
    //private void OnDrawGizmos2()
    //{
    //    Vector2 boxSize = new Vector2(5f, 3f);
    //    Gizmos.color = Color.red;
    //    Vector2 boxPosition = (Vector2)transform.position + Vector2.right * 1f * lookDirection;
    //    //Gizmos.DrawWireCube(boxPosition, boxSize);
    //}

}
