using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerEquipment equipment;
    [SerializeField] private PlayerStatManager statManager;

    private PlayerAttributeLogicsDictionary attributeLogics;
    private PlayerAttributeLogics attributeLogic = null;
    private WeaponSO weaponData;
    private float totalAttackPower;
    private float currentAttackPower;
    private float weaponAttackPower;
    private LayerMask monsterLayer;
    private AttributeType attributeType;

    //private LayerMask monsterLayer = LayerMask.GetMask("Monster");
    private Vector2 boxSize = new Vector2(1f, 1f); //TODO 무기에따라 변경
    private float lookDirection = 1f;
    private float attakRange = 1f;

    private void Awake()
    {
        attributeLogics = new PlayerAttributeLogicsDictionary();
        attributeLogics.Initialize();
        monsterLayer = LayerMask.GetMask("Monster");
        EnsureComponents();
    }

    private void OnEnable()
    {
        controller.OnDirectionEvent += OnDirectionEvent;
        controller.OnAttackEvent += OnAttackEvent;
        equipment.OnEquipWeaponChanged += OnEquipWeaponChanged;
    }

    private void OnDisable()
    {
        controller.OnDirectionEvent -= OnDirectionEvent;
        controller.OnAttackEvent -= OnAttackEvent;
        equipment.OnEquipWeaponChanged += OnEquipWeaponChanged;
        statManager.UnsubscribeToStatUpdateEvent(attackStats);
    }
    private void Start()
    {
        statManager.SubscribeToStatUpdateEvent(attackStats);
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

    private void attackStats(string statKey, float value)
    {
        switch (statKey)
        {
            case "CurrentAttackPower":
                currentAttackPower = value;
                break;
        }
        SetTotalAttackPower();
    }

    private void OnDirectionEvent(float directionValue)
    {
        lookDirection = directionValue;
    }

    private void OnAttackEvent()
    {
        //TODO 여러마리 공격할 수 있도록 변경 -> onecycle 이후
        Vector2 playerPosition = (Vector2)transform.position;
        Vector2 boxPosition = playerPosition + Vector2.right * attakRange * lookDirection;
        Collider2D monster = Physics2D.OverlapBox(boxPosition, boxSize, 0f, monsterLayer);
        if (monster == null)
        {
            return;
        }
        Debug.Log(monster.ToString() + " 때림");
        attributeLogic.ApplyAttackLogic(monster.gameObject, totalAttackPower, weaponData.attributeAttackValue, weaponData.attributeAttackRateTime, weaponData.arrtibuteStatck);
    }

    private void OnEquipWeaponChanged(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
        weaponAttackPower = weaponData.attackPower;
        attributeLogic = attributeLogics.GetAttributeLogic(weaponData.type);
        SetTotalAttackPower();
    }

    private void SetTotalAttackPower()
    {
        totalAttackPower = currentAttackPower + weaponAttackPower;
        Debug.Log(totalAttackPower);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 boxPosition = (Vector2)transform.position + Vector2.right * attakRange * lookDirection;
        Gizmos.DrawWireCube(boxPosition, boxSize);
    }
}
