using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHPUI : UIBase
{
    [SerializeField] private MonsterStatManager statManager;
    [SerializeField] private TextMeshProUGUI bossNameText; 
    [SerializeField] private Image bossImage; 
    [SerializeField] private Image healthBar;

    private MonsterStat monsterStat; 
    private void OnEnable()
    {
        statManager.OnSubscribeToStatUpdateEvent += UpdateHealthUI;
    }

    private void OnDisable()
    {
        statManager.OnSubscribeToStatUpdateEvent -= UpdateHealthUI;
    }

    private void Start()
    {
        statManager = FindObjectOfType<MonsterStatManager>();
        if (statManager != null)
        {
            monsterStat = statManager.GetComponent<MonsterStatManager>().GetStat();
        }
        
    }
    private void UpdateHealthUI(string statKey, float value)
    {
        if (statKey == "CurrentHealth")
        {
            float maxHealth = monsterStat.GetStatValue("MaxHealth");
            healthBar.fillAmount = (float)value / maxHealth;
           
        }
    }
}