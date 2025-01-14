using System;
using System.Collections;
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
    
    private CurrencySystem currencySystem;
    private SceneCapture sceneCapture;
    private SaveDataContainer saveDataContainer;
    private PlayerInput input;
    private void Awake()
    {
        input = GatherInputManager.Instance.input;
        sceneCapture = GetComponent<SceneCapture>();
    }
    private void Start()
    {
        StartCoroutine(waitForStatData());
    }
    public override void Open()
    {
        base.Open();
        sceneCapture.CaptureScene();
        StartCoroutine(waitForDataToText());
        exitButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<GameEndUI>(false));
        DataManager.Instance.DeleteData<SaveDataContainer>();
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
        input.Player.Disable();
        input.Changer.Disable();
    }

    private IEnumerator waitForStatData()
    {
        while (saveDataContainer == null)
        {
            saveDataContainer = PlayerInfoManager.Instance.GetSaveData();
            if (saveDataContainer == null)
            {
                yield return saveDataContainer != null;
            }
        }
    }

    private IEnumerator waitForDataToText()
    {
        if (saveDataContainer == null)
        {
            yield return saveDataContainer != null;
        }
        GetDataToText();
    }
    private void GetDataToText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time); // 일시정지되면 같이 멈추는 시간
        //TimeSpan timeSpan = TimeSpan.FromSeconds(Time.unscaledTime); //일시정지되도 흘러가는 시간
        playTime.text = timeSpan.ToString(@"hh\:mm\:ss");
        if (  saveDataContainer == null) Debug.Log("null saveDataContainer");
        inGameMoney.text = saveDataContainer.currencyData.inGameCurrency.ToString();
        inGameMoney.text = saveDataContainer.currencyData.globalCurrency.ToString();
    }
}