using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponUpgradeUI : UIBase
{
    [SerializeField] private UIDialogue uiDialoguePrefab;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cancelButton;
    private SaveDataContainer saveDataContainer;
    private UIDialogue uiDialogue;
    private int dialogueIndex;
    private bool isUpgrade;
    private float currentCurrency = 100;//saveDataContainer.currencyData.inGameCurrency;
    private int requiredCurrency = 100;

  
    public override void Open()
    {
        if (saveDataContainer == null) GetStatData();
        base.Open();
        Text.text = ($"보유 껍질 개수 : {saveDataContainer.currencyData.inGameCurrency.ToString()}, 필요 껍질 개수 : {requiredCurrency}");
    }
    private void Start()
    {
        Init();
        InitializeUI();
    }

    private void InitializeUI()
    {
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<WeaponUpgradeUI>(true));
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
            dialogueIndex = 10010;
            uiDialogue.gameObject.SetActive(true);
            SetDialogue();
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
            UIManager.Instance.ToggleUI<WeaponUpgradeUI>(true);
        }
        else
        {
            dialogueIndex = 10012;
            uiDialogue.gameObject.SetActive(true);
            SetDialogue();
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
            UIManager.Instance.ToggleUI<WeaponUpgradeUI>(true);
        }
    }
    private void Init()
    {
        
        if (!uiDialogue)
        {
            GameObject dialogueObject = Instantiate(uiDialoguePrefab.gameObject);
            uiDialogue = dialogueObject.GetComponent<UIDialogue>();

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
