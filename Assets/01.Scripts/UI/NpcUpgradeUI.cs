using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class UpgradeDataContainer
{
    public int countUpgrade;
    public int gradeUpgrade;
    public int gradeProbability;
    public int currencyProbability;
    public int weaponProbability;
}
public class NpcUpgradeUI : UIBase
{
    public UpgradeDataContainer upgradeData;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button[] countButton;
    [SerializeField] private Button[] gradeButton;
    [SerializeField] private List<GameObject> countLightImage;
    [SerializeField] private List<GameObject> gradeLightImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI currencyText;
    private CurrencySystem currencySystem;
    private PlayerInput input;


    private SkillType selectSkillType;
    private int requiredCurrency; // 지불비용
    private int countUpgrade; //등급상태
    private int gradeUpgrade; //강화정보
    private int currencyProbability; // 재화
    private int gradeProbability; //등급확률
    private int weaponProbability; //무기
    private float currentCurrency;// saveDataContainer.currencyData.globalCurrency;
    private int countUpgradeCost; // 업그레이드비용
    private int gradeUpgradeCost;

    public override void Open()
    {
        base.Open();
        this.input = GatherInputManager.Instance.input;
        input.Player.Disable();
        input.Changer.Disable();
        input.UI.Enable();
    }

    private void Start()
    {
        //player GameManager로 접근
        GetCurrencyData(); //캐싱
        InitButton();
        LoadUpgradeData();
        CountButtonState();
        GradeButtonState();
        upgradeButton.interactable = false;
    }

    private void GetCurrencyData()
    {
        if (GameManager.Instance.player.TryGetComponent(out CurrencySystem currency))
        {
            currencySystem = currency;
        }
        currentCurrency = currencySystem.GetCurrencyData(isGlobalCurrency: true);
    }

    private void InitButton()
    {
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<NpcUpgradeUI>(true));
        cancelButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
        cancelButton.onClick.AddListener(() => GatherInputManager.Instance.ResetStates());
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => OnUpgradeBtn());
        upgradeButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
    }

    #region /OnUpgradeBtnSetting
    private void OnUpgradeBtn()
    {
        Upgrade();
        SetDialogueTxt();
        SaveUpgradeData();
        selectSkillType = SkillType.None;
        upgradeButton.interactable = false;
    }

    private void Upgrade()
    {
        currentCurrency = currencySystem.GetCurrencyData(isGlobalCurrency: true);
        if (selectSkillType == SkillType.Count && countUpgrade < 4)
        {
            SetUpgradeCost(countUpgrade, SkillType.Count);
            if (currentCurrency >= requiredCurrency)
            {
                ToggleLight(countLightImage,countUpgrade);
                currencySystem.UseCurrency(requiredCurrency, isGlobalCurrency: true);
                currentCurrency = currencySystem.GetCurrencyData(isGlobalCurrency: true);
                countUpgrade++;
                if (countUpgrade < 2)
                {
                    currencyProbability += 50;
                }
                else if (countUpgrade >= 2)
                {
                    weaponProbability += 50;
                }
                CountButtonState();
            }
        }
        else if (selectSkillType == SkillType.Grade && gradeUpgrade < 4) //풀강 4
        {
            SetUpgradeCost(gradeUpgrade, SkillType.Grade);
            if (currentCurrency >= requiredCurrency)
            {
                ToggleLight(gradeLightImage,gradeUpgrade);
                currencySystem.UseCurrency(requiredCurrency, isGlobalCurrency: true);
                currentCurrency = currencySystem.GetCurrencyData(isGlobalCurrency: true);
                gradeUpgrade++;
                gradeProbability += 10;

                GradeButtonState();
            }
        }
    }
    #endregion


    #region /SettingButton
    private void CountButtonState()
    {
        UpdateButtonState(countButton, countLightImage, countUpgrade, SkillType.Count);
    }

    private void GradeButtonState()
    {
        UpdateButtonState(gradeButton, gradeLightImage, gradeUpgrade, SkillType.Grade);
    }

    private void UpdateButtonState(Button[] buttons, List<GameObject> lightList, int upgradeLevel, SkillType skillType)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var color = buttons[i].colors;
            color.normalColor = Color.white;
            buttons[i].colors = color;

            if (i > upgradeLevel)
            {
                color.normalColor = new Color32(129, 129, 129, 158);
                buttons[i].colors = color;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(() => SelectSkill(SkillType.None));
            }
            else if (i == upgradeLevel)
            {
                int index = i;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(() => SelectSkill(skillType));
            }
            else
            {
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(() => SelectSkill(SkillType.None));
                ToggleLight(lightList, i);
            }
        }
    }
    #endregion

    #region /SettingEtc
    private void SelectSkill(SkillType skillType)
    {
        selectSkillType = skillType;
        upgradeButton.interactable = selectSkillType != SkillType.None && currentCurrency >= requiredCurrency;
        if (selectSkillType == SkillType.Count)
        {
            SetUpgradeCost(countUpgrade, selectSkillType);
        }
        else if (selectSkillType == SkillType.Grade)
        {
            SetUpgradeCost(gradeUpgrade, selectSkillType);
        }
    }

    private void SetUpgradeCost(int upgradeLevel, SkillType skillType)
    {
        switch (upgradeLevel)
        {
            case 0:
                requiredCurrency = 2;
                break;
            case 1:
                requiredCurrency = 5;
                break;
            case 2:
                requiredCurrency = 10;
                break;
            case 3:
                requiredCurrency = 20;
                break;
            case 4:
                requiredCurrency = 99999;
                break;
        }

        if (skillType == SkillType.Count)
        {
            countUpgradeCost = requiredCurrency;
        }
        else if (skillType == SkillType.Grade)
        {
            gradeUpgradeCost = requiredCurrency;
        }
        
        SetDialogueTxt();
    }
    
    private void ToggleLight(List<GameObject> lights, int upgradeLevel)
    {
        lights[upgradeLevel].SetActive(true);
    }

    private void SetDialogueTxt()
    {
        string colorCode = currentCurrency < requiredCurrency ? "<color=#FF0000>" : "<color=#00FF00>";
        currencyText.text = $"{colorCode}{currentCurrency}/{requiredCurrency}</color>";
        countText.text = ($"좋은 열매가 나올수 있는 확률을 올립니다. \n" +
                          $"등급 확률 : {gradeProbability}% \t{gradeUpgrade}/4");
        gradeText.text = ($"많은 열매가 맺힐 수 있는 확률을 올립니다.\n" +
                          $"재화열매 추가획득 확률 : {currencyProbability}% 무기열매 추가획득 확률 : {weaponProbability}%" +
                          $" \n {countUpgrade}/4");
    }
    #endregion

    #region /Data
    private void SaveUpgradeData()
    {
        upgradeData.countUpgrade = countUpgrade;
        upgradeData.gradeUpgrade = gradeUpgrade;
        upgradeData.currencyProbability = currencyProbability;
        upgradeData.gradeProbability = gradeProbability;
        upgradeData.weaponProbability = weaponProbability;
        DataManager.Instance.SaveData(upgradeData);
    }

    private void LoadUpgradeData()
    {
        if (!DataManager.Instance.GetCanLoad<UpgradeDataContainer>())
        {
            return;
        }
        upgradeData = DataManager.Instance.LoadData<UpgradeDataContainer>();
        countUpgrade = upgradeData.countUpgrade;
        gradeUpgrade = upgradeData.gradeUpgrade;
        currencyProbability = upgradeData.currencyProbability;
        gradeProbability = upgradeData.gradeProbability;
        weaponProbability = upgradeData.weaponProbability;
        CountButtonState();
        GradeButtonState();
    }
    #endregion
}