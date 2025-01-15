
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class MonsterController : MonoBehaviour, IProjectTileShooter
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
    private float lookDirection = 1f;

    public event UnityAction<RegularPatternData> OnPatternSelected;
    private bool isGround;
    private bool isStopped;

    [SerializeField] private Transform shootPoint;
    private IObjectPool<PooledProjectile> pooledProjectTile;
    private RangedAttackData rangedAttackData = null;

    [Header("RegularMonster_Pattern_Info")]
    private RegularPatternData currentRegularPatternData = null;
    private List<RegularPatternData> regularPatternDatas = new List<RegularPatternData>();

    [Header("Monster_Stat_Info")]
    private float attackPower;
    private float moveSpeed;
    private float attackDistance;
    private float chaseRange;
    private AttributeType type;
    private float attributeValue;
    private float attributeRateTime;
    private float attributeStack;

    #region // LifeCycle
    private void Awake()
    {
        attributeLogicsDictionary = new MonsterAttributeLogicsDictionary();
        attributeLogicsDictionary.Initialize();
        stateMachine = new MonsterStateMachine(this);
        monsterAttributeLogics = new MonsterNormal();
        CachedProjectTile();
    }

    private void OnEnable()
    {
        statManager.OnSubscribeToStatUpdateEvent += OnStatUpdatedEvent;
        statManager.OnRangedAttackDataEvent += GetRagnedAttackStat;
        statManager.OnRegularPatternDataEvent += GetRegularPatternStat;
        OnPatternSelected += animationController.SetChagedPatternAnimation;
    }

    private void OnDisable()
    {
        statManager.OnSubscribeToStatUpdateEvent -= OnStatUpdatedEvent;
        statManager.OnRangedAttackDataEvent -= GetRagnedAttackStat;
        statManager.OnRegularPatternDataEvent -= GetRegularPatternStat;
        OnPatternSelected -= animationController.SetChagedPatternAnimation;
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

    #region // MonsterStat
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

    public void GetRagnedAttackStat(RangedAttackData data)
    {
        rangedAttackData = data;
    }

    public void GetRegularPatternStat(List<RegularPatternData> data)
    {
        regularPatternDatas = data;
    }

    public RegularPatternData GetRandomPattern()
    {
        if (regularPatternDatas.Count == 0) return null;
        var randomValue = Random.Range(0, 100f);
        var randomIndex = UnityEngine.Random.Range(0, regularPatternDatas.Count);
        if(randomValue <= regularPatternDatas[randomIndex].patternAttackChance)
        {
            currentRegularPatternData = regularPatternDatas[randomIndex];
            OnPatternSelected?.Invoke(currentRegularPatternData);
            return currentRegularPatternData;
        }
        currentRegularPatternData = null;
        OnPatternSelected?.Invoke(currentRegularPatternData);
        return currentRegularPatternData;
    }
    #endregion

    #region // MonsterState
    public void ReverseDirection()
    {
        if(animationController.HasAttackFinished())
        {
            return;
        }
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        lookDirection *= -1f;
        transform.localScale = scale;
    }
    public bool DetectPlayer()
    {
        Vector3 raytDirection = Vector3.right * lookDirection;
        Vector2 boxSize = new Vector2(attackDistance + 4f, attackDistance);
        var hit = Physics2D.OverlapBox(transform.position, boxSize, chaseRange, playerLayerMask);
        if (hit != null && monsterGround.GetOnGround())
        {
            target = hit.gameObject;
            return true;
        }

        if (target != null && Vector2.Distance(transform.position, target.transform.position) >= chaseRange)
        {
            target = null;
        }

        return target != null;
    }

    public void MoveWithGroundDetection()
    {
        if (monsterGround.GetOnGround())
        {
            isGround = false;
        }
        else
        {
            if (!isGround)
            {
                ReverseDirection();
                isGround = true;
            }
        }

        if (!DetectPlayer())
        {
            Move();
        }
    }

    public void FllowPlayer()
    {
        if (statManager.isDead) return;
        if (target != null)
        {
            var tartPosition = GetRandomizedTargetPosition(target.transform.position);
            float distanceX = tartPosition.x - transform.position.x;
            float distanceY = Mathf.Abs(tartPosition.y - transform.position.y);

            if (distanceY > 2f)
            {
                Move();
                return;
            }

            if ((distanceX >= 0 && lookDirection <= 0) && animationController.OnAttackComplete()
                || (distanceX < 0 && lookDirection >= 0) && animationController.OnAttackComplete())
            {
                ReverseDirection();
                return;
            }

            Move();
        }
    }

    public bool InAttackRange()
    {
        Vector3 raytDirection = Vector3.right * lookDirection;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raytDirection, attackDistance, playerLayerMask);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    public void Move()
    {
        if (statManager.isDead) return;
        if (animationController.HasAttackFinished())
        {
            return;
        }
        Vector3 direction = (Vector3.right * lookDirection).normalized;

        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.transform.position) >= attackDistance)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
        else
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    Vector3 GetRandomizedTargetPosition(Vector3 targetPosition)
    {
        return targetPosition + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
    }
    #endregion

    #region // MonsterAttack
    private void CachedProjectTile()
    {
        pooledProjectTile = PoolManager.Instance.GetObjectFromPool<PooledProjectile>(PoolType.PooledProjectile);
    }

    public void Shoot(PooledProjectile projectTile)
    {
        Vector3 look = Vector3.right * lookDirection;
        projectTile.transform.position = shootPoint.position;
        projectTile.SetData(rangedAttackData, rangedAttackData.rangedAttackPower);
        projectTile.SetAttirbuteData(rangedAttackData);
        projectTile.ProjectTileShoot(look);
    }

    public void Attack()
    {
       if (statManager.isDead) return;
        Vector2 monsterPosition = transform.position;
        Vector2 boxPostion = monsterPosition + Vector2.right * attackDistance/2 * lookDirection;
        Vector2 boxSize = new Vector2(attackDistance, 1f);
        var randomPatternAttack = currentRegularPatternData;
        Collider2D player = Physics2D.OverlapBox(boxPostion, boxSize, 0, playerLayerMask);
        if (player == null)
        {
            return;
        }
        if(randomPatternAttack != null)
        {
            ApplyPatternAttackLogic(player.gameObject, randomPatternAttack);          
        }
        else if(animationController.CheckedDefalutAttack())
        {
            monsterAttributeLogics.ApplyAttackLogic(player.gameObject, attackPower, attributeValue, attributeRateTime, attributeStack, lookDirection);
        }
        SoundManagers.Instance.PlaySFX(SfxType.MonsterAttack);
    }

    public void RangedAttack()
    {
        Vector3 direction = (Vector3.right * lookDirection).normalized;
        if (statManager.isDead) return;
        var projecttile = pooledProjectTile.Get();
        Shoot(projecttile);
    }
    private void ApplyPatternAttackLogic(GameObject target, RegularPatternData patternData)
    {
        var patternLogic = attributeLogicsDictionary.GetAttributeLogic(patternData.patternAttributeType);
        if(patternLogic != null)
        {
            patternLogic.ApplyAttackLogic(target, patternData.patternDamage, patternData.patternAttributeValue,
                patternData.patternAttributeRateTime, patternData.patternAttributeStack, lookDirection);
        }
    }
    #endregion

    private void OnDrawGizmos()
    {

        Vector2 monsterPosition = transform.position;
        // OverlapBox�� �߽� ��ġ ���
        Vector3 boxCenter = monsterPosition + Vector2.right * attackDistance/2 * lookDirection;
        Vector2 boxSize = new Vector2(attackDistance, 1f);

        // Gizmos ���� ����
        Gizmos.color = Color.red;

        // OverlapBox �׸���
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}


