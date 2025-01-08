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
    private float currentCurrency = 100;//saveDataContainer.currencyData.inGameCurrency; -> 캐릭터 보유 재화로 체크
    private int requiredCurrency = 100;

  
    public override void Open()
    {
        if (saveDataContainer == null) GetStatData();
        base.Open();
        Text.text = ($"보유 껍질 개수 : {saveDataContainer.currencyData.inGameCurrency.ToString()}, 필요 껍질 개수 : {requiredCurrency}");
    }
    private void Start()
    {
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
            dialogueIndex = 10203;
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
            UIManager.Instance.ToggleUI<NpcDialougeUI>(false, dialogueIndex);
            //Destroy( uiDialogue);
        }
        else
        {
            dialogueIndex = 10205;
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
            UIManager.Instance.ToggleUI<NpcDialougeUI>(false, dialogueIndex);
            //Destroy( uiDialogue);
        }
    }
   
}
