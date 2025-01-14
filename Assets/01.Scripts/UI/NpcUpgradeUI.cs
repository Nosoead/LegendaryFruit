using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcUpgradeUI : UIBase
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private GameObject skill1LightImage;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI countText;
    private CurrencySystem currencySystem;

    private int gradeUpgrade; //맥스
    private int countUpgrade; //맥스
    private int Gradeprobability; //등급확률
    private int Countprobability; //개수확률
    private float currentCurrency = 1000; // saveDataContainer.currencyData.globalCurrency;
    private int countCurrency;
    private int gradeCurrency;

    public override void Open()
    {
        base.Open();
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<NpcUpgradeUI>(true));
        cancelButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
        cancelButton.onClick.AddListener(() => GatherInputManager.Instance.ResetStates());
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => OnUpgradeBtn(1));
        upgradeButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
    }

    private void Start()
    {
        GetEquipAndCurrencyData();
        SetCurrncy(gradeUpgrade,1);
        SetCurrncy(countUpgrade,2);
        SetDialogueTxt();
    }

    private void GetEquipAndCurrencyData()
    {
        if (GameManager.Instance.player.TryGetComponent(out CurrencySystem currency))
        {
            currencySystem = currency;
        }
    }
    private void OnUpgradeBtn(int upgradeType)
    {
        Debug.Log("강화?");
        //if (currentCurrency >= GetRequiredCurrency(upgradeType))
        
            if (upgradeType == 1)
            {
                Debug.Log("강화1");
                GradeUpgrade();
            }

            else if (upgradeType == 2)
            {
                CountUpgrade();
                Debug.Log("강화2");
            }

            SetDialogueTxt();
        //TODO 텍스트에 현재재화/필요재화 
        //TODO 충족 초록    부족 빨강?
        //TODO 버튼에 인덱스번호 부여 가능?
    }

    private int GetRequiredCurrency(int upgradeType)
    {
        if (upgradeType == 1)
        {
            return gradeCurrency;
        }
        else if (upgradeType == 2)
        {
            return countCurrency;
        }

        return 0;
    }

    

    private void GradeUpgrade()
    {
        if (gradeUpgrade < 4)
        {
            gradeUpgrade++;
            Gradeprobability += 20;
            currentCurrency -= gradeCurrency;
            SetCurrncy(gradeUpgrade,1);
        }
    }

    private void CountUpgrade()
    {
        if (countUpgrade < 4)
        {
            countUpgrade++;
            Countprobability += 20;
            currentCurrency -= countCurrency;
            SetCurrncy(countUpgrade,2);
        }
    }
    private void SetCurrncy(int upgradeLevel, int upgradeType)
    {
        if (currencySystem != null)
        {
            currentCurrency = currencySystem.GetCurrencyData(isGlobalCurrency: true);
        }

        int requiredCurrency = 0;
        switch (upgradeLevel)
        {
            case 0:
                requiredCurrency = 50;
                break;
            case 1:
                requiredCurrency = 100;
                break;
            case 2:
                requiredCurrency = 200;
                break;
            case 3:
                requiredCurrency = 500;
                break;
        }

        if (upgradeType == 1)
        {
            gradeCurrency = requiredCurrency;
        }
        if (upgradeType == 2)
        {
            countCurrency = requiredCurrency;
        }
    }
    private void SetDialogueTxt()
    {
        countText.text = ($"많은 열매가 맺힐 수 있는 확률을 올립니다.\n" +
                          $"개수 확률 : {Gradeprobability}% \t{gradeUpgrade}/4");
        gradeText.text = ($"좋은 열매가 나올수 있는 확률을 올립니다. \n" +
                          $"등급 확률 : {Countprobability}% \t{countUpgrade}/4");
    }
}