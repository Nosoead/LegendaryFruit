using UnityEngine;

public class BossMonsterController : MonoBehaviour 
{
    public MonsterAnimationController animator;
    [SerializeField] private MonsterStatManager statManager;
    [SerializeField] private BossPatternTargetDetector detector;
    private BossStateMachine stateMachine;
    public BossStateMachine StateMachine => stateMachine;
    private MonsterAttributeLogics monsterAttributeLogics = null;
    private MonsterAttributeLogicsDictionary attributeLogicsDictionary;
    private PatternData pattren;
    public MonsterGround monsterGround;
    [SerializeField] private LayerMask playerLayerMask;
    public GameObject target;
    private float lookDirection = 1f;

    private float attackPower;
    private float moveSpeed;
    private float attackDistance;
    private float chaseRange;
    private AttributeType type;
    private float attributeValue;
    private float attributeRateTime;
    private float attributeStack;
    //private bool canMove = true;

    // Pattren Info
    [SerializeField] private float damageInterval;
    private Vector2 overlapBoxSize;
    private float patternDamage;
    private PatternData currentPattern;

    #region Lifecycle Functions
    private void Awake()
    {
        if(animator == null) { animator = GetComponent<MonsterAnimationController>(); }
        attributeLogicsDictionary = new MonsterAttributeLogicsDictionary();
        attributeLogicsDictionary.Initialize();
        stateMachine = new BossStateMachine(this);
        target = GameObject.FindWithTag("Player");
    }

    private void OnEnable()
    {
        statManager.OnSubscribeToStatUpdateEvent += OnStatUpdatedEvent;
        statManager.OnPatternTriggered += OnHealthThresholdReached;
        detector.OnPlayerEnterDamageZone += OnPattrenAttack;
    }

    private void OnDisable()
    {
        statManager.OnSubscribeToStatUpdateEvent -= OnStatUpdatedEvent;
        statManager.OnPatternTriggered -= OnHealthThresholdReached;
        detector.OnPlayerEnterDamageZone -= OnPattrenAttack;
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

    public PatternData SetPatternData(PatternData data)
    {
        currentPattern = data;
        animator.SetPatternAnimationData(data);
        patternDamage = data.patternDamage;
        return currentPattern;
    }

    public void OnHealthThresholdReached(PatternData patternData,float damage)
    {
        if(patternData != currentPattern && damage != patternDamage)
        {
            SetPatternData(patternData);
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
        if (statManager.isDead) return;
        Vector2 monsterPosition = transform.position;
        Vector2 boxPosition = monsterPosition + Vector2.right * attackDistance * (1f * lookDirection)
                        + Vector2.down * 1.5f;
        Vector2 boxSize = new Vector2(attackDistance, 4);
        Collider2D player = Physics2D.OverlapBox(boxPosition, boxSize, 0, playerLayerMask);
        if (player == null)
        {
            return;
        }
        float randomAttackPower = GetRandomDamageInRange(attackPower);
        monsterAttributeLogics.ApplyAttackLogic(player.gameObject, randomAttackPower, attributeValue, attributeRateTime, attributeStack, lookDirection);
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

    public void OnPattrenAttack(GameObject player)
    {
        if (statManager.isDead) return;
        if(player == target)
        {
            float randomAttackPower = GetRandomDamageInRange(patternDamage);
            monsterAttributeLogics.ApplyAttackLogic(player.gameObject, randomAttackPower, attributeValue, attributeRateTime, attributeStack, lookDirection);
        }
        detector.ResetDetector();
    }
    #endregion

    #region Movement
    public void Move()
    {
        if (statManager.isDead) return;
        if(animator.HasAttackFinished())
        {
            return;
        }
        Vector3 direction = (Vector3.right * lookDirection).normalized;
        if (Vector2.Distance(transform.position, target.transform.position) >= attackDistance)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
    Vector3 GetRandomizedTargetPosition(Vector3 targetPosition)
    {
        return targetPosition + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }
    #endregion

    public void ChangeLayer()
    {
        int targetLayer = LayerMask.NameToLayer("Invincible");
        this.gameObject.layer = targetLayer;
        if (animator.HasPatternAttackFinished())
        {
            this.gameObject.layer = LayerMask.NameToLayer("Monster");
        }
        else
        {
            return;
        }
    }

    private float GetRandomDamageInRange(float totalAttackPower)
    {
        float randomNum = Random.Range(0.8f, 1.21f);
        int randomint = (int)(randomNum * totalAttackPower);
        return randomint;
    }

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
}
