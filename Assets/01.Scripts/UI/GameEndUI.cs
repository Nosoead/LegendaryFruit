using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
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
    [SerializeField] private TextMeshProUGUI totalEat;
    [SerializeField] private TextMeshProUGUI todoAdd;
    [SerializeField] private Image lastImage;
    [SerializeField] private List<Image> totalEatImages;
    
    private CurrencySystem currencySystem;
    private SceneCapture sceneCapture;
    private SaveDataContainer saveDataContainer;
    private PlayerInput input;
    private void Awake()
    {
        input = GatherInputManager.Instance.input;
        sceneCapture = GetComponent<SceneCapture>();
    }

    public override void Open()
    {
        base.Open();
        sceneCapture.CaptureScene();
        CachingSaveData();
        CachingImageList();
        SetResultData();
        exitButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<GameEndUI>(false));
        exitButton.onClick.AddListener(() => DataManager.Instance.DeleteData<SaveDataContainer>());
        if (GameManager.Instance.GetGameClear())
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
    }


    private void CachingSaveData()
    {
        saveDataContainer = PlayerInfoManager.Instance.GetSaveData();
    }

    private void CachingImageList()
    {
        foreach (Image image in totalEatImages)
        {
            image.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private void SetResultData()
    {
        TimeSpan timeSpan = GameManager.Instance.GetCurrentTimeData();
        playTime.text = timeSpan.ToString(@"hh\:mm\:ss");
        if (  saveDataContainer == null) Debug.Log("null saveDataContainer");
        inGameMoney.text = saveDataContainer.currencyData.inGameCurrency.ToString();
        globalMoney.text = saveDataContainer.currencyData.globalCurrency.ToString();
        totalEat.text = saveDataContainer.weaponData.eatWeaponDataList.Count.ToString();
        for (int i = 0; i < saveDataContainer.weaponData.eatWeaponDataList.Count; i++)
        {
            totalEatImages[i].sprite = saveDataContainer.weaponData.eatWeaponDataList[i].rewardSprite;
            totalEatImages[i].color = new Color(1f, 1f, 1f, 1f);
        }
    }
}