using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcUpgradeUI : UIBase
{
    [SerializeField] private UIDialogue uiDialoguePrefab;
    [SerializeField] private TextMeshProUGUI chanceLevelText;
    [SerializeField] private TextMeshProUGUI amountLevelText;
    [SerializeField] private TextMeshProUGUI chanceNameText;
    [SerializeField] private TextMeshProUGUI amountNameText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cancelButton;
    private SaveDataContainer saveDataContainer;
    private UIDialogue uiDialogue;
    private int dialogueIndex;
    private bool isUpgrade;
    private float currentCurrency = 100;// saveDataContainer.currencyData.globalCurrency;
    private int requiredCurrency = 100;
  
  
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
    upgradeButton.onClick.AddListener(OnUpgradeBtn);
  }
  private void GetStatData()
  {
      saveDataContainer = PlayerInfoManager.Instance.GetSaveData();
  }
    public void OnUpgradeBtn()
      {
          //currentCurrency = 1000;
          //requiredCurrency = 100; //임시
          if (currentCurrency >= requiredCurrency)
          {
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
      }
      private void Init()
      {
          
          if (!uiDialogue)
          {
              //UIManager.Instance.ToggleUI<UIDialogue>(true);
              GameObject dialogueObject = Instantiate(uiDialoguePrefab.gameObject);
              uiDialogue = dialogueObject.GetComponent<UIDialogue>();
              uiDialogue.gameObject.SetActive(false);
  
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
