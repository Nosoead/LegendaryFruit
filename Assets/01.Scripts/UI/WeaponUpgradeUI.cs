using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponUpgradeUI : UIBase
{
    [SerializeField] private TextMeshProUGUI inGameMoneyText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cancelButton;
    private SaveDataContainer saveDataContainer;

    private void Update()
    {
        if (saveDataContainer != null) GetDataToText();
    }
    public override void Open()
    {
        if (saveDataContainer == null) GetStatData();
        base.Open();
        cancelButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<WeaponUpgradeUI>(true));
    }

    private void GetStatData()
   {
       saveDataContainer = PlayerInfoManager.Instance.GetSaveData();
   }
   private void GetDataToText()
   {
       inGameMoneyText.text = ($"보유 껍질 개수 : {saveDataContainer.currencyData.inGameCurrency.ToString()}, 필요 껍질 개수 :");
   }
}
