using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private List<Image> weaponIcons;
    [SerializeField] private List<Image> weaponOverlays;
    private Sprite itemSprite1;
    private SaveDataContainer saveDataContainer;
    private WeaponSO currentEquippedWeapon;
    private string colorCode;

    public override void Open()
    {
        base.Open();
        if (saveDataContainer == null) GetStatData();
        GetDataToText();
    }

    private void GetStatData()
    {
        saveDataContainer = PlayerInfoManager.Instance.GetSaveData();
    }

    private void GetDataToText()
    {
        SetWeaponDataToUI();
        UpdateColorToText();

        maxHealthText.text = ($"최대체력 : {saveDataContainer.playerStatData.maxHealth.ToString()}");
        
        attackPowerText.text = $"공격력 : {saveDataContainer.playerStatData.currentAttackPower} + " +
                               $"<color={colorCode}>{currentEquippedWeapon.attackPower}</color>";

        defenseText.text = ($" 방어력 : {saveDataContainer.playerStatData.currentDefense.ToString()}");
        attackSpeedText.text = ($" 공격속도 : {saveDataContainer.playerStatData.attackSpeed.ToString()}");
        moveSpeedText.text = ($" 이동속도 : {saveDataContainer.playerStatData.moveSpeed.ToString()}");
    }

    private void SetWeaponDataToUI()
    {
        var weaponDataList = saveDataContainer.weaponData.equippedWeapons;
        int currentEquipIndex = saveDataContainer.weaponData.currentEquipWeaponIndex;

        for (int i = 0; i < weaponDataList.Count && i < itemNameText.Count; i++)
        {
            if (i < weaponDataList.Count)
            {
                WeaponSO weapon = weaponDataList[i];

                if (i == currentEquipIndex)
                {
                    currentEquippedWeapon = weapon;
                }

                itemNameText[i].text = $"{weapon.weaponName}";
                itemDescription[i].text = $"{weapon.description}";
                itemAttackPower[i].text = $"공격력: {weapon.attackPower}";
                itmeType[i].text = $"속성: {weapon.type}";
                itemGrade[i].text = $"등급: {weapon.gradeType}";
                itemEatValue[i].text = $"섭취 값: {weapon.eatValue}";

                weaponIcons[i].sprite = weapon.weaponSprite;
                weaponIcons[i].color = Color.white;

                weaponOverlays[i].color =
                    i == currentEquipIndex ? new Color(0, 0, 0, 0) : new Color(0.0f, 0f, 0f, 0.5f);
            }
            else
            {
                itemNameText[i].text = "";
                itemDescription[i].text = "";
                itemAttackPower[i].text = "";
                itmeType[i].text = "";
                itemGrade[i].text = "";
                itemEatValue[i].text = "";

                weaponIcons[i].sprite = null;
                weaponIcons[i].color = new Color(0f, 0f, 0f, 0f);
                weaponOverlays[i].color = new Color(0f, 0f, 0f, 0f);
            }
        }
    }

    private void UpdateColorToText()
    {
        if (currentEquippedWeapon.type == AttributeType.Burn)
        {
            colorCode = "#FF0000";
        }
        else if (currentEquippedWeapon.type == AttributeType.SlowDown)
        {
            colorCode = "#0000FF";
        }
        else if (currentEquippedWeapon.type == AttributeType.Knockback)
        {
            colorCode = "#FFFF00";
        }
        else
        {
            colorCode = "#594126";
        }
    }
}