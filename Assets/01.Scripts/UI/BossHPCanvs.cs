using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHPCanvs : UIBase
{
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private Image bossImage;
    [SerializeField] private Image healthBar;
    
    private void Init()
    {
        bossNameText.text = "숲의 수호자";
    }
    private void OnHealthUpdateEvent(float healthFillAmount, float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = healthFillAmount;
    }

}
