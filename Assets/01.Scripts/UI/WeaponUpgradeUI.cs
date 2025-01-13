using UnityEngine;
using TMPro;
using UnityEngine.UI;

//TODO : Player 키셋팅 -> 교체(Space Bar) 못하도록
public class WeaponUpgradeUI : UIBase
{
    [SerializeField] private UIDialogue uiDialoguePrefab;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cancelButton;
    private PlayerInput input;

    private PlayerEquipment playerEquip;
    private CurrencySystem currencySystem;
    private WeaponSO weaponData;
    private int upgradeValue = 10000;
    private float currentCurrency;
    private int requiredCurrency;

    private UIDialogue uiDialogue;
    private int dialogueIndex;

    public override void Open()
    {
        base.Open();
    }

    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            input.Player.Disable();
        }
    }

    private void Start()
    {
        input = GatherInputManager.Instance.input;
        InitializeUI();
        GetEquipAndCurrencyData();
        SetCurrncy(weaponData.gradeType);
        SetDialogueTxt();
    }

    private void GetEquipAndCurrencyData()
    {
        if (playerEquip == null)
        {
            playerEquip = GameManager.Instance.player.GetComponentInChildren<PlayerEquipment>();
        }
        if (GameManager.Instance.player.TryGetComponent(out CurrencySystem currency))
        {
            currencySystem = currency;
        }
        if (playerEquip != null)
        {
            weaponData = playerEquip.GetCurrentEquipData();
        }
    }

    private void InitializeUI()
    {
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<WeaponUpgradeUI>(false));
        cancelButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
        cancelButton.onClick.AddListener(() => GatherInputManager.Instance.ResetStates());
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(OnUpgradeBtn);
    }

    private void SetCurrncy(GradeType gradeType)
    {
        if (currencySystem != null)
        {
            currentCurrency = currencySystem.GetCurrencyData(isGlobalCurrency: false);
        }

        switch (gradeType)
        {
            case GradeType.Normal:
                requiredCurrency = 50;
                break;
            case GradeType.Rare:
                requiredCurrency = 100;
                break;
            case GradeType.Unique:
                requiredCurrency = 0;
                break;
            case GradeType.Legendary:
                requiredCurrency = 0;
                break;
        }
    }

    private void SetDialogueTxt()
    {
        Text.text = ($"보유 껍질 개수 : {currentCurrency.ToString()}, 필요 껍질 개수 : {requiredCurrency}");
    }

    private void OnUpgradeBtn()
    {
        if (requiredCurrency == 0)
        {
            dialogueIndex = 10216;
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
            UIManager.Instance.ToggleUI<NpcDialougeUI>(false, dialogueIndex);
            ResetAllData();
            return;
        }

        if (weaponData.type == AttributeType.Normal)
        {
            dialogueIndex = 10217;
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
            UIManager.Instance.ToggleUI<NpcDialougeUI>(false, dialogueIndex);
            ResetAllData();
            return;
        }

        if (currentCurrency >= requiredCurrency)
        {
            currencySystem.UseCurrency(useValue: requiredCurrency, isGlobalCurrency: false);
            ChangeWeaponData();
            dialogueIndex = 10203;
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
            UIManager.Instance.ToggleUI<NpcDialougeUI>(false, dialogueIndex);
            ResetAllData();
        }
        else
        {
            dialogueIndex = 10205;
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
            UIManager.Instance.ToggleUI<NpcDialougeUI>(false, dialogueIndex);
            ResetAllData();
        }
    }

    private void ChangeWeaponData()
    {
        int currentID = weaponData.ID;
        int upgradeID = currentID + upgradeValue;
        weaponData = ItemManager.Instance.GetUpgradeItemData(upgradeID);
        playerEquip.SetUpgradeData(weaponData);
    }

    private void ResetAllData()
    {
        playerEquip = null;
        currencySystem = null;
        weaponData = null;
        currentCurrency = 0;
        requiredCurrency = 0;
    }
}
