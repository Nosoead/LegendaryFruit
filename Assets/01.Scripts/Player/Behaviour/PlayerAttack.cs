using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class PlayerAttack : MonoBehaviour, IProjectTileShooter
{
    public UnityAction<bool> OnAttackingEvent;
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerEquipment equipment;
    [SerializeField] private PlayerStatManager statManager;
    [SerializeField] private PlayerMovementHandler movementHandler;

    [SerializeField] private Transform shootPosition;
    private PlayerAttributeLogicsDictionary attributeLogics;
    private PlayerAttributeLogics attributeLogic = null;
    private WeaponSO weaponData;
    private RangedAttackData rangedAttackData = null;
    private float totalAttackPower;
    private float currentAttackPower;
    private float weaponAttackPower;
    private LayerMask monsterLayer;
    private AttributeType attributeType;

    private Vector2 boxSize = new Vector2(2.5f, 3f); //TODO 무기에따라 변경
    private bool isAttacking = false;
    private float lookDirection = 1f;
    private float attakRange = 1.5f;
    private WaitForSeconds attackRateTime = new WaitForSeconds(0.5f);
    //TODO : 공격속도에 따라 waitforseconds가 변하도록

    private IObjectPool<PooledProjectile> projectile;

    //기즈모
    private float attackLookDirection = 1f;

    private void Awake()
    {
        attributeLogics = new PlayerAttributeLogicsDictionary();
        attributeLogics.Initialize();
        monsterLayer = LayerMask.GetMask("Monster");
        EnsureComponents();
        CachedProjectile();
    }

    private void OnEnable()
    {
        statManager.OnSubscribeToStatUpdateEvent += OnStatUpdatedEvent;
        controller.OnDirectionEvent += OnDirectionEvent;
        controller.OnAttackEvent += OnAttackEvent;
        equipment.OnEquipWeaponChanged += OnEquipWeaponChanged;
    }

    private void OnDisable()
    {
        statManager.OnSubscribeToStatUpdateEvent -= OnStatUpdatedEvent;
        controller.OnDirectionEvent -= OnDirectionEvent;
        controller.OnAttackEvent -= OnAttackEvent;
        equipment.OnEquipWeaponChanged += OnEquipWeaponChanged;
    }

    private void EnsureComponents()
    {
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }
        if (equipment == null)
        {
            equipment = GetComponentInChildren<PlayerEquipment>();
        }
        if (statManager == null)
        {
            statManager = GetComponent<PlayerStatManager>();
        }
    }

    #region /subscribeMethod
    private void OnStatUpdatedEvent(string statKey, float value)
    {
        switch (statKey)
        {
            case "CurrentAttackPower":
                currentAttackPower = value;
                SetTotalAttackPower();
                break;
        }
    }

    private void OnDirectionEvent(float directionValue)
    {
        lookDirection = directionValue;
    }

    private void OnAttackEvent()
    {
        if (isAttacking)
        {
            return;
        }
        StartCoroutine(attackRoutine());
    }

    private IEnumerator attackRoutine()
    {
        SfxType randomSfx = (Random.Range(0, 2) == 0) ? SfxType.PlayerAttack1 : SfxType.PlayerAttack2;
        SoundManagers.Instance.PlaySFX(randomSfx);
        isAttacking = true;
        attackLookDirection = lookDirection;//기즈모
        OnAttackingEvent?.Invoke(isAttacking);
        Vector2 playerPosition = (Vector2)transform.position;
        Vector2 boxPosition = playerPosition + Vector2.right * attakRange * lookDirection;
        Collider2D[] monster = Physics2D.OverlapBoxAll(boxPosition, boxSize, 0f, monsterLayer);
        if (monster == null)
        {
            yield return attackRateTime;
        }
        else if (rangedAttackData != null)
        {
            RagnedAttack();
            yield return attackRateTime;
        }
        else
        {
            foreach (Collider2D collider in monster)
            {
                float randomAttackPower = GetRandomDamageInRange(totalAttackPower);
                attributeLogic.ApplyAttackLogic(collider.gameObject, randomAttackPower, weaponData.attributeAttackValue, weaponData.attributeAttackRateTime, weaponData.arrtibuteStatck, lookDirection);
            }
            yield return attackRateTime;
        }
        isAttacking = false;
        OnAttackingEvent?.Invoke(isAttacking);
    }

    private float GetRandomDamageInRange(float totalAttackPower)
    {
        float randomNum = Random.Range(0.8f, 1.21f);
        int randomint = (int)(randomNum*totalAttackPower);
        return randomint;
    }

    private void OnEquipWeaponChanged(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
        rangedAttackData = null;
        weaponAttackPower = weaponData.attackPower;
        attributeLogic = attributeLogics.GetAttributeLogic(weaponData.type);
        SetTotalAttackPower();
        CachedRagnedAttackData();
    }
    #endregion

    private void CachedProjectile()
    {
        projectile = PoolManager.Instance.GetObjectFromPool<PooledProjectile>(PoolType.PooledProjectile);
    }

    private void CachedRagnedAttackData()
    {
        if (weaponData.rangedAttackData == null) return;
        for (int i = 0; i < weaponData.rangedAttackData.Count; i++)
        {
            rangedAttackData = weaponData.rangedAttackData[i];
        }
    }

    private void SetTotalAttackPower()
    {
        totalAttackPower = currentAttackPower + weaponAttackPower;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float drawDireiction = isAttacking ? attackLookDirection : lookDirection;
        Vector2 boxPosition = (Vector2)transform.position + Vector2.right * attakRange * drawDireiction;
        Gizmos.DrawWireCube(boxPosition, boxSize);
    }

    public void Shoot(PooledProjectile projectTile)
    {
        Vector3 look = Vector3.right * lookDirection;
        projectTile.transform.position = shootPosition.position;
        projectTile.SetData(rangedAttackData, rangedAttackData.rangedAttackPower + currentAttackPower);
        projectTile.SetAttirbuteData(rangedAttackData);
        projectTile.ProjectTileShoot(look);
    }

    public void RagnedAttack()
    {
        PooledProjectile attackProjectile = projectile.Get();
        Shoot(attackProjectile);
    }
}
