using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasUI : UIBase
{
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI inGameMoneyText;
    [SerializeField] private TextMeshProUGUI lobbyMoneyText;
    [SerializeField] private Image healthBar;
    [SerializeField] private PlayerStatManager statManager;
    
    [SerializeField] private List<Image> weaponIcons;
    [SerializeField] private List<Image> weaponOverlays;

    private SaveDataContainer saveDataContainer;

    private void Update()
    {
        if (saveDataContainer != null) SetWeaponDataToUI();
    }

    private void Awake()
    {
        if (statManager == null)
        {
            statManager = GetComponentInParent<PlayerStatManager>();
        }
    }

    private void Start()
    {
        Invoke(nameof(GetStatData),0.1f);
    }

    private void GetStatData()
    {
        saveDataContainer = PlayerInfoManager.Instance.GetSaveData();
    }
    private void OnEnable()
    {
        statManager.OnHealthDataToUIEvent += OnHealthUpdateEvent;
    }

    private void OnDisable()
    {
        statManager.OnHealthDataToUIEvent -= OnHealthUpdateEvent;
    }

    private void OnHealthUpdateEvent(float healthFillAmount, float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = healthFillAmount;
        currentHealthText.text = $"{currentHealth}/{maxHealth}";
    }
    private void SetWeaponDataToUI()
    {
        var weaponDataList = saveDataContainer.weaponData.equippedWeapons;
        int currentEquipIndex = saveDataContainer.weaponData.currentEquipWeaponIndex;

        if (weaponDataList.Count >= 1)
        {
            WeaponSO leftWeapon = weaponDataList[0];
            weaponIcons[0].sprite = leftWeapon.weaponSprite;
            weaponIcons[0].color = Color.white;
            weaponOverlays[0].color = (currentEquipIndex == 0) ? Color.white : Color.red;

            for (int i = 0; i < weaponDataList.Count; i++)
            {
                if (weaponDataList.Count > 1)
                {
                    weaponIcons[1].sprite = weaponDataList[i].weaponSprite;
                    weaponIcons[1].color = Color.white;
                    weaponOverlays[1].color = (currentEquipIndex == 1) ? Color.white : Color.red;
                }
            }
           
            if (weaponDataList.Count == 1)
            {
                weaponIcons[1].sprite = null;
                weaponIcons[1].color = new Color(0, 0, 0, 0);
                weaponOverlays[1].color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            weaponIcons[0].sprite = null;
            weaponIcons[0].color = new Color(0, 0, 0, 0);
            weaponOverlays[0].color = new Color(0, 0, 0, 0);
            
            weaponIcons[1].sprite = null;
            weaponIcons[1].color = new Color(0, 0, 0, 0);
            weaponOverlays[1].color = new Color(0, 0, 0, 0);
        }
    }
}
