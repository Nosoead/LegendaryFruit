using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcUpgradeUI : UIBase
{
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
    private int countProbability; // 재화
    private int weaponProbability; //무기
    private float currentCurrency = 1000; // saveDataContainer.currencyData.globalCurrency;
    private int countCurrency; // 업그레이드비용
    private int gradeCurrency;

    public override void Open()
    {
        base.Open();
        //TODO 확률(재화열매 등급 높을 확률)
        //TODO 10%, 20%, 30%, 40%
        //TODO 열매갯수 최대 2개 (재화 1개, 무기 1개)
        //TODO 재화 50% 재화 100%, 무기 50, 무기 100%
    }

    private void Start()
    {
        Initialize();
        GetEquipAndCurrencyData();
        SetDialogueTxt();
        GradeButtonState();
        CountButtonState();
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

        for (int i = 0; i < countButton.Length; i++)
        {
            int index = i;
            countButton[i].onClick.RemoveAllListeners();
            countButton[i].onClick.AddListener(() => SelectSkill(index, SkillType.Count));
        }

        for (int i = 0; i < countButton.Length; i++)
        {
            int index = i;
            gradeButton[i].onClick.RemoveAllListeners();
            gradeButton[i].onClick.AddListener(() => SelectSkill(index, SkillType.Grade));
        }
        //countButton.onClick.AddListener(() => SelectSkill(SkillType.Count));
        //gradeButton.onClick.AddListener(() => SelectSkill(SkillType.Grade));
    }

    private void SelectSkill(int buttonIndex, SkillType skillType)
    {
        selectSkillType = SkillType.None;
        upgradeButton.interactable = false;

        if (skillType == SkillType.Count)
        {
            if (currentCurrency >= countCurrency)
            {
                selectSkillType = SkillType.Count;
                SetCurrncy(countUpgrade, SkillType.Count);
            }
            else
            {
                selectSkillType = SkillType.None;
            }
        }
        else if (skillType == SkillType.Grade)
        {
            if (currentCurrency >= gradeCurrency)
            {
                selectSkillType = SkillType.Grade;
                SetCurrncy(countUpgrade, SkillType.Grade);
            }
            else
            {
                selectSkillType = SkillType.None;
            }
        }

        if (selectSkillType != SkillType.None && currentCurrency >= requiredCurrency)
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
        }

        SetDialogueTxt();
    }


    private void OnUpgradeBtn()
    {
        if (selectSkillType == SkillType.Grade && currentCurrency >= gradeCurrency)
        {
            GradeUpgrade();
        }

        else if (selectSkillType == SkillType.Count && currentCurrency >= countCurrency)
        {
            CountUpgrade();
        }
        else
        {
            return;
        }

        SetDialogueTxt();
        /*GradeButtonState();
        CountButtonState();*/
        Debug.Log(selectSkillType);
        SetCurrncy(999, SkillType.None);
        selectSkillType = SkillType.None;
        upgradeButton.interactable = false;
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
                gradeProbability += 10;
                GradeButtonState();
                GradeActiveLight(gradeUpgrade);
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
                countProbability += 20;
                CountButtonState();
                CountActiveLight(countUpgrade);
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
            case 4:
                requiredCurrency = 9999;
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

        if (currentCurrency >= requiredCurrency)
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
        }
    }

    private void CountActiveLight(int upgradeLevel)
    {
        for (int i = 0; i < countLightImage.Count; i++)
        {
            if (i == upgradeLevel - 1)
            {
                countLightImage[i].SetActive(true);
            }
        }
    }

    private void GradeActiveLight(int upgradeLevel)
    {
        for (int i = 0; i < gradeLightImage.Count; i++)
        {
            if (i == upgradeLevel - 1)
            {
                gradeLightImage[i].SetActive(true);
            }
        }
    }

    private void GetEquipAndCurrencyData()
    {
        if (GameManager.Instance.player.TryGetComponent(out CurrencySystem currency))
        {
            currencySystem = currency;
        }
    }

    private void SetDialogueTxt()
    {
        if (selectSkillType == SkillType.Count) SetCurrncy(countUpgrade, SkillType.Count);
        if (selectSkillType == SkillType.Grade) SetCurrncy(gradeUpgrade, SkillType.Grade);

        string colorCode = currentCurrency < requiredCurrency ? "<color=#FF0000>" : "<color=#00FF00>";
        currencyText.text = $"{colorCode}{currentCurrency}/{requiredCurrency}</color>";
        countText.text = ($"많은 열매가 맺힐 수 있는 확률을 올립니다.\n" +
                          $"개수 확률 : {gradeProbability}% \t{gradeUpgrade}/4");
        gradeText.text = ($"좋은 열매가 나올수 있는 확률을 올립니다. \n" +
                          $"등급 확률 : {countProbability}% \t{countUpgrade}/4");
    }

    public void GetUpgradeStats(out int gradeUpgrade, out int countUpgrade, out int gradeProbability,
        out int countProbability)
    {
        gradeUpgrade = this.gradeUpgrade;
        countUpgrade = this.countUpgrade;
        gradeProbability = this.gradeProbability;
        countProbability = this.countProbability;
    }

    private void CountButtonState()
    {
        for (int i = 0; i < countButton.Length; i++)
        {
            var color = countButton[i].colors;
            color.normalColor = Color.white;
            countButton[i].colors = color;
            if (i > countUpgrade || currentCurrency < requiredCurrency)
            {
                color.normalColor = new Color32(129, 129, 129, 158);
                countButton[i].colors = color;
                countButton[i].onClick.RemoveAllListeners();
            }
            else
            {
                int index = i;
                countButton[i].onClick.RemoveAllListeners();
                countButton[i].onClick.AddListener(() => SelectSkill(index, SkillType.Count));
            }
        }
    }

    private void GradeButtonState()
    {
        for (int i = 0; i < gradeButton.Length; i++)
        {
            var color = gradeButton[i].colors;
            color.normalColor = Color.white;
            gradeButton[i].colors = color;
            if (i > gradeUpgrade || currentCurrency < requiredCurrency)
            {
               color.normalColor = new Color32(129, 129, 129, 158);
               gradeButton[i].colors = color;
                gradeButton[i].onClick.RemoveAllListeners();
            }
            else
            {
                int index = i;
                gradeButton[i].onClick.RemoveAllListeners();
                gradeButton[i].onClick.AddListener(() => SelectSkill(index, SkillType.Grade));
            }
        }
    }
}