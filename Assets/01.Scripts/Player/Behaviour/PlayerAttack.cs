using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    public UnityAction<bool> OnAttackingEvent;
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerEquipment equipment;
    [SerializeField] private PlayerStatManager statManager;
    [SerializeField] private PlayerMovementHandler movementHandler;

    private PlayerAttributeLogicsDictionary attributeLogics;
    private PlayerAttributeLogics attributeLogic = null;
    private WeaponSO weaponData;
    private float totalAttackPower;
    private float currentAttackPower;
    private float weaponAttackPower;
    private LayerMask monsterLayer;
    private AttributeType attributeType;

    //private LayerMask monsterLayer = LayerMask.GetMask("Monster");
    private Vector2 boxSize = new Vector2(2.5f, 3f); //TODO 무기에따라 변경
    private bool isAttacking = false;
    private float lookDirection = 1f;
    private float attakRange = 1.5f;
    private WaitForSeconds attackRateTime = new WaitForSeconds(0.5f);
    //TODO : 공격속도에 따라 waitforseconds가 변하도록

    //기즈모용
    private float attackLookDirection = 1f;

    private void Awake()
    {
        attributeLogics = new PlayerAttributeLogicsDictionary();
        attributeLogics.Initialize();
        monsterLayer = LayerMask.GetMask("Monster");
        EnsureComponents();
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
        SfxType randomSfx = (Random.Range(0,2) == 0) ? SfxType.PlayerAttack1 : SfxType.PlayerAttack2;
        SoundManagers.Instance.PlaySFX(randomSfx);
        //SoundManagers.Instance.PlaySFX(SfxType.PlayerAttack2);
        isAttacking = true;
        attackLookDirection = lookDirection;//기즈모용
        OnAttackingEvent?.Invoke(isAttacking);
        //TODO 여러마리 공격할 수 있도록 변경 -> onecycle 이후
        Vector2 playerPosition = (Vector2)transform.position;
        Vector2 boxPosition = playerPosition + Vector2.right * attakRange * lookDirection;
        Collider2D monster = Physics2D.OverlapBox(boxPosition, boxSize, 0f, monsterLayer);
        if (monster == null)
        {
            yield return attackRateTime;
        }
        else
        {
            attributeLogic.ApplyAttackLogic(monster.gameObject, totalAttackPower, weaponData.attributeAttackValue, weaponData.attributeAttackRateTime, weaponData.arrtibuteStatck);
            yield return attackRateTime;
        }
        isAttacking = false;
        OnAttackingEvent?.Invoke(isAttacking);
    }
    private void OnEquipWeaponChanged(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
        weaponAttackPower = weaponData.attackPower;
        attributeLogic = attributeLogics.GetAttributeLogic(weaponData.type);
        SetTotalAttackPower();
    }
    #endregion

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
}
