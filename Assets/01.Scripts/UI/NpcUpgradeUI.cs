using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class UpgradeData
{
    public int gradeUpgrade;
    public int countUpgrade;
    public int gradeProbability;
    public int currencyProbability;
    public int weaponProbability;
}
public class NpcUpgradeUI : UIBase
{
    public UpgradeData upgradeData;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button[] countButton;
    [SerializeField] private Button[] gradeButton;
    [SerializeField] private List<GameObject> countLightImage;
    [SerializeField] private List<GameObject> gradeLightImage;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI currencyText;
    private CurrencySystem currencySystem;


    private SkillType selectSkillType;
    private int requiredCurrency; // 지불비용
    private int gradeUpgrade;
    private int countUpgrade; //등급상태
    private int gradeProbability; //등급확률
    private int currencyProbability; // 재화
    private int weaponProbability; //무기
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
        GradeButtonState();
        CountButtonState();
        LoadUpgradeData();
        upgradeButton.interactable = false;
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
    }
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
        if (selectSkillType == SkillType.Count && countUpgrade < 4)
        {
            SetCurrncy(countUpgrade, SkillType.Count);
            if (currentCurrency >= requiredCurrency)
            {
                currentCurrency -= requiredCurrency;
                countUpgrade++;
                if (countUpgrade <= 2)
                {
                    currencyProbability += 50;
                }
                else if (countUpgrade < 2)
                {
                    weaponProbability += 50;
                }
                CountButtonState();
                ToggleLight(countLightImage,countUpgrade);
            }
        }
        else if (selectSkillType == SkillType.Grade && gradeUpgrade < 4) //풀강 4
        {
            SetCurrncy(gradeUpgrade, SkillType.Count);
            if (currentCurrency >= requiredCurrency)
            {
                currentCurrency -= requiredCurrency;
                gradeUpgrade++;
                gradeProbability += 10;

                GradeButtonState();
                ToggleLight(gradeLightImage,gradeUpgrade);
            }
        }
    }
    
    private void UpdateButtonState(Button[] buttons, int upgradeLevel, SkillType skillType)
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
            else
            {
                int index = i;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(() => SelectSkill(skillType));
            }
        }
    }
    
    private void CountButtonState()
    {
        UpdateButtonState(countButton, countUpgrade, SkillType.Count);
    }

    private void GradeButtonState()
    {
        UpdateButtonState(gradeButton, gradeUpgrade, SkillType.Grade);
    }
    private void SelectSkill(SkillType skillType)
    {
        selectSkillType = skillType;
        upgradeButton.interactable = selectSkillType != SkillType.None && currentCurrency >= requiredCurrency;
        if (selectSkillType == SkillType.Count)
        {
            SetCurrncy(countUpgrade, selectSkillType);
        }
        else if (selectSkillType == SkillType.Grade)
        {
            SetCurrncy(gradeUpgrade, selectSkillType);
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
            case 4:
                requiredCurrency = 99999;
                break;
        }

        if (skillType == SkillType.Grade)
        {
            gradeCurrency = requiredCurrency;
        }

        else if (skillType == SkillType.Count)
        {
            countCurrency = requiredCurrency;
        }

        
        SetDialogueTxt();
    }
    
    private void ToggleLight(List<GameObject> lights, int upgradeLevel)
    {
        for (int i = 0; i < lights.Count; i++)
        {
            if (i == upgradeLevel - 1)
            {
                lights[i].SetActive(true);
            }
        }
    }
    private void SetDialogueTxt()
    {
        string colorCode = currentCurrency < requiredCurrency ? "<color=#FF0000>" : "<color=#00FF00>";
        currencyText.text = $"{colorCode}{currentCurrency}/{requiredCurrency}</color>";
        gradeText.text = ($"많은 열매가 맺힐 수 있는 확률을 올립니다.\n" +
                          $"재화열매 추가획득 확률 : {currencyProbability}% 무기열매 추가획득 확률 : {weaponProbability}%" +
                          $" \n {countUpgrade}/4");
        countText.text = ($"좋은 열매가 나올수 있는 확률을 올립니다. \n" +
                          $"등급 확률 : {gradeProbability}% \t{gradeUpgrade}/4");
    }
    
    private void GetEquipAndCurrencyData()
    {
        if (GameManager.Instance.player.TryGetComponent(out CurrencySystem currency))
        {
            currencySystem = currency;
        }
    }

    private void SaveUpgradeData()
    {
        upgradeData.gradeUpgrade = gradeUpgrade;
        upgradeData.countUpgrade = countUpgrade;
        upgradeData.gradeProbability = gradeProbability;
        upgradeData.currencyProbability = currencyProbability;
        upgradeData.weaponProbability = weaponProbability;
    }

    private void LoadUpgradeData()
    {
        gradeUpgrade = upgradeData.gradeUpgrade;
        countUpgrade = upgradeData.countUpgrade;
        gradeProbability = upgradeData.gradeProbability;
        currencyProbability = upgradeData.currencyProbability;
        weaponProbability = upgradeData.weaponProbability;
    }
}