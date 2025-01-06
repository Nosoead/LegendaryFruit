using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillData
{
    public int currentLevel;
    public int maxLevel;
    public bool isUnlocked;
}

public class SkillUI : MonoBehaviour
{
    public List<GameObject> lightImage;

    public void UpdateUI(bool isUpgrade)
    {
        foreach (GameObject image in lightImage)
        {
            image.SetActive(isUpgrade);
        }
    }
}
public class NpcUpgradeUI : UIBase
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private GameObject skill1LightImage;
    public List<SkillData> skillList;
    public List<SkillUI> skillUIList;
    private int dialogueIndex;
    private bool isUpgrade;
    private float currentCurrency = 100;// saveDataContainer.currencyData.globalCurrency;
    private int requiredCurrency = 100;
  public override void Open()
  {
      base.Open();
  }
  private void Start()
  {
      Init();
      InitializeUI();
  }

  private void Init()
  {
 
          for (int i = 0; i < skillList.Count; i++)
          {
              skillUIList[i].UpdateUI(skillList[i].currentLevel > 0);
              skillList[i].isUnlocked = (i == 0);
          }
          //DialogueManager.Instance.SetUIDialogue(uiDialogue);

  }
  private void InitializeUI()
  {
      cancelButton.onClick.RemoveAllListeners();
      cancelButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<NpcUpgradeUI>(true));
      cancelButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
      upgradeButton.onClick.RemoveAllListeners();
      upgradeButton.onClick.AddListener(() => OnUpgradeBtn(1));
      upgradeButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
  }
  private void UpgradeSkill(int skillIndex)
  {
      if (skillList[skillIndex].isUnlocked &&
          skillList[skillIndex].currentLevel < skillList[skillIndex].maxLevel)
      {
          skillList[skillIndex].currentLevel++;
          skillUIList[skillIndex].UpdateUI(true);

          if (skillIndex + 1 < skillList.Count)
          {
              skillList[skillIndex + 1].isUnlocked = true;
          }
      }
  }

  public void OnUpgradeBtn(int skillIndex)
  {
      // 강화를 하면 재화가 깍이고 스탯이 올라감
      // 강화하면 해당 강화는 더이상 불가 다음 강화 가능
      
      if (currentCurrency >= requiredCurrency)
      {
          UpgradeSkill(skillIndex);
          currentCurrency -= requiredCurrency;
      }
      Debug.Log("?");
  }
  
}
