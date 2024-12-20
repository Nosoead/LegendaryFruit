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
    private SaveDataContainer saveDataContainer;

    private void Update()
    {
        if (saveDataContainer != null)
        {
            GetDataToText();
            healthBar.fillAmount = saveDataContainer.playerStatData.currentHealth;
        }
    }

    public override void Open()
    {
        if (saveDataContainer == null)
        {
            GetStatData();
            healthBar = GetComponent<Image>();
        }
        base.Open();
    }
    private void GetStatData()
    {
        saveDataContainer = PlayerInfoManager.Instance.GetSaveData();
    }

    private void GetDataToText()
    {
        currentHealthText.text = ($"{saveDataContainer.playerStatData.maxHealth.ToString()}/{saveDataContainer.playerStatData.currentHealth.ToString()}");
        //inGameMoneyText.text = 
        //lobbyMoneyText.text =
    }
}
