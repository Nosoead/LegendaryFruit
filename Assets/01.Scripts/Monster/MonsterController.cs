using System.Threading;
using UnityEngine;
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

    private bool isGround;

    [SerializeField] private Transform shootPoint;
    private IObjectPool<PooledProjectile> pooledProjectTile;
    private MonsterSO monsterData = null;

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
    }

    private void OnDisable()
    {
        statManager.OnSubscribeToStatUpdateEvent -= OnStatUpdatedEvent;
        statManager.OnRangedAttackDataEvent -= GetRagnedAttackStat;
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

    public void GetRagnedAttackStat(MonsterSO data)
    {
        monsterData = data;
    }
    #endregion

    #region // MonsterState
    public void ReverseDirection()
    {
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
        if(monsterGround.GetOnGround())
        {
            isGround = false;
        }
        else
        {
            if(!isGround)
            {
                ReverseDirection();
                isGround = true;
            }
        }

        if(!DetectPlayer())
        {
            Move();
        }
    }

    public void FllowPlayer()
    {
        if (statManager.isDead) return;
        if (target != null)
        {
            float distanceX = target.transform.position.x - transform.position.x;
            float distanceY = Mathf.Abs(target.transform.position.y - transform.position.y);

            if(distanceY > 2f)
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
        Vector3 direction = (Vector3.right * lookDirection).normalized;
        if (statManager.isDead) return;

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
        projectTile.SetData(monsterData.monsterRagnedAttackData, monsterData.monsterRagnedAttackData.rangedAttackPower);
        projectTile.SetAttirbuteData(monsterData.monsterRagnedAttackData);
        projectTile.ProjectTileShoot(look);
    }

    public void Attack()
    {
        if (statManager.isDead) return;
        Vector2 monsterPosition = transform.position;
        Vector2 boxPostion = monsterPosition + Vector2.right * (attackDistance * lookDirection);
        Vector2 boxSize = new Vector2(attackDistance, 1f);

        Collider2D player = Physics2D.OverlapBox(boxPostion, boxSize, 0, playerLayerMask);
        if (player == null)
        {
            return;
        }
        monsterAttributeLogics.ApplyAttackLogic(player.gameObject, attackPower, attributeValue, attributeRateTime, attributeStack, lookDirection);
        SoundManagers.Instance.PlaySFX(SfxType.MonsterAttack);
    }

    public void RangedAttack()
    {
        if (statManager.isDead) return;
        var projecttile = pooledProjectTile.Get();
        Shoot(projecttile);
    }
    #endregion

    private void OnDrawGizmos()
    {

        // OverlapBox�� �߽� ��ġ ���
        Vector3 boxCenter = transform.position;
        Vector2 boxSize = new Vector2(attackDistance + 4, attackDistance);

        // Gizmos ���� ����
        Gizmos.color = Color.red;

        // OverlapBox �׸���
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}


