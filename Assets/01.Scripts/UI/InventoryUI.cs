using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class InventoryUI : UIBase
{
    
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI attackPowerText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    
    [SerializeField] private List<TextMeshProUGUI> itemNameText;
    [SerializeField] private List<TextMeshProUGUI> itemDescription;
    [SerializeField] private List<TextMeshProUGUI> itemAttackPower;
    [SerializeField] private List<TextMeshProUGUI> itmeType;
    [SerializeField] private List<TextMeshProUGUI> itemGrade;
    [SerializeField] private List<TextMeshProUGUI> itemEatValue;
    private Sprite itemSprite1;

    private SaveDataContainer saveDataContainer;


    private void Update()
    {
        if (saveDataContainer != null) GetDataToText();
    }

    public override void Open()
    { 
        if (saveDataContainer == null) GetStatData();
        base.Open();
    }
    private void GetStatData()
    {
        saveDataContainer = PlayerInfoManager.Instance.GetSaveData();
    }

    private void GetDataToText()
    {
        maxHealthText.text = ($"최대체력 : {saveDataContainer.playerStatData.maxHealth.ToString()}");
        attackPowerText.text = ($" 공격력 : {saveDataContainer.playerStatData.currentAttackPower.ToString()}");
        defenseText.text = ($" 방어력 : {saveDataContainer.playerStatData.currentDefense.ToString()}");
        attackSpeedText.text = ($" 공격속도 : {saveDataContainer.playerStatData.attackSpeed.ToString()}");
        moveSpeedText.text = ($" 이동속도 : {saveDataContainer.playerStatData.moveSpeed.ToString()}");
        //maxHealthText.text = saveDataContainer.playerStatData.maxHealth.ToString();
    }
}
