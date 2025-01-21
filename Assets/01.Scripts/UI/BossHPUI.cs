using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BossHPUI : UIBase
{
    [SerializeField] private MonsterStatManager statManager;
    [SerializeField] private Image healthBar;

    private MonsterStat monsterStat;

    private void Awake()
    {
        GameObject BossMonster = GameObject.Find("PooledBossMonster(Clone)");
        statManager = BossMonster.GetComponentInChildren<MonsterStatManager>();

        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<Image>();
        }
    }

    private void OnEnable()
    {
        statManager.OnShowHealthBarEvent += OnShowHealthbar;
    }
    
    private void OnDisable()
    {
        statManager.OnShowHealthBarEvent -= OnShowHealthbar;
    }
    
    private void OnShowHealthbar(float healthData,bool isOpen)
    {
        healthBar.DOFillAmount(healthData, 0.2f).SetEase(Ease.OutQuad);
    }

}