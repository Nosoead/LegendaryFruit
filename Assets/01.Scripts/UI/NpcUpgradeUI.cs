using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillData
{
    public string skillName;
    public int currentLevel;
    public int maxLevel;
    public bool isUnlocked;
}

public class SkillUI : MonoBehaviour
{
    public GameObject lightImage;

    public void UpdateUI(bool isUpgrade)
    {
        lightImage.SetActive(isUpgrade);
    }
}
public class NpcUpgradeUI : UIBase
{
    [SerializeField] private UIDialogue uiDialoguePrefab;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private GameObject skill1LightImage;
    public List<SkillData> skillList;
    public List<SkillUI> skillUIList;
    private SaveDataContainer saveDataContainer;
    private UIDialogue uiDialogue;
    private int dialogueIndex;
    private bool isUpgrade;
    private float currentCurrency = 100;// saveDataContainer.currencyData.globalCurrency;
    private int requiredCurrency = 100;

    /*public List<SkillData> skillList = new List<SkillData>
    {
        new SkillData { skillName = "확률 업", currentLevel = 0, maxLevel = 4, isUnlocked = false },
        new SkillData { skillName = "개수 업", currentLevel = 0, maxLevel = 4, isUnlocked = false },
    };

    public List<SkillUI> skillUIList = new List<SkillUI>
    {


    };*/
  
  
  public override void Open()
  {
      if (saveDataContainer == null) GetStatData();
      base.Open();
  }
  private void Start()
  {
      Init();
      InitializeUI();
     
  }
  private void Update()
  {
      if (saveDataContainer != null) GetDataToText();
  }

  private void GetDataToText()
  {
     
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
  private void GetStatData()
  {
      saveDataContainer = PlayerInfoManager.Instance.GetSaveData();
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
      if (currentCurrency >= requiredCurrency)
      {
          UpgradeSkill(skillIndex);
          currentCurrency -= requiredCurrency;
      }
      Debug.Log(skillList[skillIndex].skillName + " upgraded to " + requiredCurrency);
      Debug.Log("?");
  }
    /*public void OnUpgradeBtn(int skillIndex)
      {
          //currentCurrency = 1000;
          //requiredCurrency = 100; //임시
          if (currentCurrency >= requiredCurrency)
          {
              UpgradeSkill(skillIndex);
              currentCurrency -= requiredCurrency;
              Debug.Log("재화 깍음 ");
              Debug.Log("SO 바꿈");
              isUpgrade = true;
              dialogueIndex = 10213;
              uiDialogue.gameObject.SetActive(true);
              SetDialogue();
              SoundManagers.Instance.PlaySFX(SfxType.UIButton);
              UIManager.Instance.ToggleUI<NpcUpgradeUI>(true);
              //Destroy( uiDialogue);
          }
          else
          {
              dialogueIndex = 10214;
              uiDialogue.gameObject.SetActive(true);
              SetDialogue();
              SoundManagers.Instance.PlaySFX(SfxType.UIButton);
              UIManager.Instance.ToggleUI<NpcUpgradeUI>(true);
              //Destroy( uiDialogue);
          }
      }*/
      private void Init()
      {
          
          if (!uiDialogue)
          {
              //UIManager.Instance.ToggleUI<UIDialogue>(true);
              GameObject dialogueObject = Instantiate(uiDialoguePrefab.gameObject);
              uiDialogue = dialogueObject.GetComponent<UIDialogue>();
              uiDialogue.gameObject.SetActive(false);

              for (int i = 0; i < skillList.Count; i++)
              {
                  skillUIList[i].UpdateUI(skillList[i].currentLevel > 0);
                  skillList[i].isUnlocked = (i == 0);
              }
              //DialogueManager.Instance.SetUIDialogue(uiDialogue);
          }
  
      }
      private void SetDialogue()
      {
          var dialogue = DialogueManager.Instance.GetDialogueData(dialogueIndex);
          if (dialogue != null)
          {
              uiDialogue.SetDialogue(dialogue); 
          }
      }
}
