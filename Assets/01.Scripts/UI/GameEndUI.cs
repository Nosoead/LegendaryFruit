using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : UIBase
{
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI exitTxt;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI playTime;
    [SerializeField] private TextMeshProUGUI inGameMoney;
    [SerializeField] private TextMeshProUGUI globalMoney;
    [SerializeField] private TextMeshProUGUI totalDamage;
    [SerializeField] private TextMeshProUGUI totalEat;
    [SerializeField] private Image lastImage;
    [SerializeField] private List<Image> totalEatImages;

    private SaveDataContainer saveDataContainer;

    public override void Open()
    {
        if (saveDataContainer == null) GetStatData();
        base.Open();
        if (GameManager.Instance.isClear)
        {
            titleTxt.text = "Game Clear!";
            exitButton.onClick.AddListener(() => SceneManagerExtension.Instance.LoadScene(SceneType.TitleScene));
        }
        else
        {
            titleTxt.text = "Game Over..";
            exitButton.onClick.AddListener(() => SceneManagerExtension.Instance.LoadScene(SceneType.OneCycleScene));
        }
        exitTxt.text = "돌아가기";
        exitButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<GameEndUI>(false));
        DataManager.Instance.DeleteData<SaveDataContainer>();
    }

    private void GetStatData()
    {
        saveDataContainer = PlayerInfoManager.Instance.GetSaveData();
    }

    private void GetDataToText()
    {
        playTime.text = Time.deltaTime.ToString();
        inGameMoney.text = saveDataContainer.currencyData.inGameCurrency.ToString();
        inGameMoney.text = saveDataContainer.currencyData.globalCurrency.ToString();
    }
}