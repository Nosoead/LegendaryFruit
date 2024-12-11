using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerEquipment equipment;
    [SerializeField] private PlayerStatManager statManager;

    private PlayerAttribueteLogicsDictionary attributeLogics;
    private PlayerAttributeLogics attributeLogic = null;
    private float currentAttackPower;
    private AttributeType attributeType;

    //private LayerMask monsterLayer = LayerMask.GetMask("Monster");
    private Vector2 boxSize = new Vector2(1, 1); //TODO 무기에따라 변경
    private void Awake()
    {
        attributeLogics = new PlayerAttribueteLogicsDictionary();
        attributeLogics.Initialize();
        EnsureComponents();
    }

    private void OnEnable()
    {
        controller.OnAttackEvent += OnAttackEvent;
        equipment.OnEquipWeaponEvent += OnEquipWeaponEvent;
    }

    private void OnDisable()
    {
        controller.OnAttackEvent -= OnAttackEvent;
        equipment.OnEquipWeaponEvent += OnEquipWeaponEvent;
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
            equipment = GetComponent<PlayerEquipment>();
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
    }

    private void OnAttackEvent()
    {
        //TODO 여러마리 공격할 수 있도록 변경 -> onecycle 이후
        Vector2 playerPosition = (Vector2)transform.position;
        Vector2 boxPosition = playerPosition + Vector2.right * 0.5f;
        //Collider2D enemy = Physics2D.OverlapBox()
        //attributeLogic.ApplyAttackLogic(, currentAttackPower);
    }

    private void OnEquipWeaponEvent()
    {
        //TODO 장착한 무기 SO정보 받아서 공격타입 수정
        attributeType = AttributeType.Normal;
        attributeLogic = attributeLogics.GetAttributeLogic(attributeType);
    }
}
