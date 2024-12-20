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

    private SaveDataContainer saveDataContainer;

    private void Awake()
    {
        if (statManager == null)
        {
            statManager = GetComponentInParent<PlayerStatManager>();
        }
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
}
