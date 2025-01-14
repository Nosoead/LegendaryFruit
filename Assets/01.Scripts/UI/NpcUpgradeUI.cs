using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcUpgradeUI : UIBase
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button countButton;
    [SerializeField] private Button gradeButton;
    [SerializeField] private GameObject skill1LightImage;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI currencyText;
    private CurrencySystem currencySystem;


    private SkillType selectSkillType;
    private int requiredCurrency; // 지불비용
    private int gradeUpgrade;
    private int countUpgrade; //등급상태
    private int Gradeprobability; //등급확률
    private int Countprobability;
    private float currentCurrency = 1000; // saveDataContainer.currencyData.globalCurrency;
    private int countCurrency; // 업그레이드비용
    private int gradeCurrency;

    public override void Open()
    {
        base.Open();
    }

    private void Start()
    {
        Initialize();
        GetEquipAndCurrencyData();
        SetDialogueTxt();
    }

    private void Initialize()
    {
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<NpcUpgradeUI>(true));
        cancelButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
        cancelButton.onClick.AddListener(() => GatherInputManager.Instance.ResetStates());
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => OnUpgradeBtn());
        upgradeButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));

        countButton.onClick.AddListener(() => SelectSkill(SkillType.Count));
        gradeButton.onClick.AddListener(() => SelectSkill(SkillType.Grade));
    }

    private void SelectSkill(SkillType skillType)
    {
        selectSkillType = skillType;
        SetDialogueTxt();
    }

    private void GetEquipAndCurrencyData()
    {
        if (GameManager.Instance.player.TryGetComponent(out CurrencySystem currency))
        {
            currencySystem = currency;
        }
    }

    private void OnUpgradeBtn()
    {
        if (selectSkillType == SkillType.Grade && currentCurrency >= gradeCurrency)
        {
            GradeUpgrade();
        }

        if (selectSkillType == SkillType.Count && currentCurrency >= countCurrency)
        {
            CountUpgrade();
        }
        SetDialogueTxt();
    }

    private void GradeUpgrade()
    {
        if (gradeUpgrade < 4) //풀강 4
        {
            SetCurrncy(gradeUpgrade, SkillType.Grade);
            if (currentCurrency >= gradeCurrency)
            {
                currentCurrency -= gradeCurrency;
                gradeUpgrade++;
                Gradeprobability += 20;
            }
        }
    }

    private void CountUpgrade()
    {
        if (countUpgrade < 4)
        {
            SetCurrncy(countUpgrade, SkillType.Count);
            if (currentCurrency >= countCurrency)
            {
                currentCurrency -= countCurrency;
                countUpgrade++;
                Countprobability += 20;
            }
        }
    }

    private void SetCurrncy(int upgradeLevel, SkillType skillType)
    {
        if (currencySystem != null)
        {
            //currentCurrency = currencySystem.GetCurrencyData(isGlobalCurrency: true);
        }

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

        if (skillType == SkillType.Grade)
        {
            gradeCurrency = requiredCurrency;
        }

        if (skillType == SkillType.Count)
        {
            countCurrency = requiredCurrency;
        }
    }

    private void SetDialogueTxt()
    {
        if (selectSkillType == SkillType.Count) SetCurrncy(countUpgrade, SkillType.Count);
        if (selectSkillType == SkillType.Grade) SetCurrncy(gradeUpgrade, SkillType.Grade);

        string colorCode = currentCurrency < requiredCurrency ? "<color=#FF0000>" : "<color=#00FF00>";
        currencyText.text = $"{colorCode}{currentCurrency}/{requiredCurrency}</color>";
        countText.text = ($"많은 열매가 맺힐 수 있는 확률을 올립니다.\n" +
                          $"개수 확률 : {Gradeprobability}% \t{gradeUpgrade}/4");
        gradeText.text = ($"좋은 열매가 나올수 있는 확률을 올립니다. \n" +
                          $"등급 확률 : {Countprobability}% \t{countUpgrade}/4");
    }
}