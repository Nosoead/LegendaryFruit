using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InventoryUI : UIBase
{
    
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI attackPowerText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    
    [SerializeField] private TextMeshProUGUI itemNameText1;
    [SerializeField] private TextMeshProUGUI itemDescription1;
    [SerializeField] private TextMeshProUGUI itemAttackPower1;
    [SerializeField] private TextMeshProUGUI itmeType1;
    [SerializeField] private TextMeshProUGUI itemGrade1;
    [SerializeField] private TextMeshProUGUI itemEatValue1;
    private Sprite itmeSprite1;
    
    private PlayerStatManager playerStatManager;
     private PlayerEquipment playerEquipment;

    private void Awake()
    {
        playerStatManager = GameManager.Instance.player.GetComponent<PlayerStatManager>();
        playerEquipment = GameManager.Instance.player.GetComponent<PlayerEquipment>();
    }
    private void OnEnable()
    {
        playerStatManager.OnSubscribeToStatUpdateEvent += UpdateUI;
        //maxHealthText.text= playerStatManager.stat.GetStatValue("MaxHealth").ToString();
        //playerEquipment.OnEquipWeaponChanged += OnEquipWeaponChanged;
    }
    private void OnDisable()
    {
        playerStatManager.OnSubscribeToStatUpdateEvent -= UpdateUI;
        //playerEquipment.OnEquipWeaponChanged -= OnEquipWeaponChanged;
    }
     private void Start()
     {
         
     }

    /*private void OnEquipWeaponChanged(WeaponSO weaponData)
    {
        itemNameText1.text = $"{weaponData.weaponName}";
        Debug.Log(weaponData.weaponName);
        itemDescription1.text = $"{weaponData.description}";
        itemAttackPower1.text = $"공격력 : {weaponData.attackPower}";
        itmeType1.text = $"타입 : {weaponData.type}";
        itemGrade1.text = $"등급 : {weaponData.gradeType}";
        itemEatValue1.text = $"섭취 증가량 : {weaponData.eatValue}";
        itmeSprite1 = weaponData.weaponSprite;
    }*/

    private void UpdateUI(string statKey, float value)
    {
               
        switch (statKey)
        {
            case "MaxHealth":
                maxHealthText.text = $"{"최대체력",-15}: {value,15}";
                Debug.Log(maxHealthText);
                break;
            case "CurrentHealth":
                currentHealthText.text = $"/ {value}";
                break;
            case "CurrentAttackPower":
                attackPowerText.text = $"{"공격력",-17}: {value,15}";
                break;
            case "CurrentDefense":
                defenseText.text = $"{"방어력",-17}: {value,15}";
                break;
            case "AttackSpeed":
                attackSpeedText.text = $"{"공격속도",-15}: {value,15}";
                break;
            case "MoveSpeed":
                moveSpeedText.text = $"{"이동속도",-15}: {value,15}";
                break;
        }
    }

    public override void Open()
    {
        base.Open();

        /*if ()
        {
            itemDescription1.text = "무기 SO에 있는 설명";
        }
        else
        {
            itemDescription2.text = "무기 SO에 있는 설명..";
        }
*/
    }

}
