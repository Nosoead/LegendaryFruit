using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptUI : UIBase
{
    //[SerializeField] private WeaponSO weaponSo;
    //private WeaponSO currentWeaponSo;
    [SerializeField] private GameObject promptWindow;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image promptImage;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI promptName;
    [SerializeField] private TextMeshProUGUI promptDescription;
    [SerializeField] private TextMeshProUGUI promptAttackPower;
    [SerializeField] private TextMeshProUGUI equipOrEatText;
    [SerializeField] private GameObject holdButton;
    
    [SerializeField] private Image outlineImage;
    private PlayerInteraction interaction;

    private void Awake()
    {
        promptWindow.SetActive(false);
        if (GameManager.Instance.player.TryGetComponent(out PlayerInteraction interaction))
        {
            this.interaction = interaction;
        }
        if (rectTransform == null)
        {
            rectTransform = promptWindow.GetComponent<RectTransform>();
        }
    }

    private void OnEnable()
    {
        interaction.ShowPromptEvent += OnShowPromptEvent;
        interaction.ShowFillamountInPromptEvent += OnShowFillamount;
    }

    private void OnDisable()
    {
        interaction.ShowPromptEvent -= OnShowPromptEvent;
        interaction.ShowFillamountInPromptEvent -= OnShowFillamount;
    }

    private void OnShowPromptEvent(ItemSO itemData, Vector3 position, bool isOpen)
    {
        if (!isOpen)
        {
            promptWindow.gameObject.SetActive(false);
        }
        else
        {
            promptWindow.gameObject.SetActive(true);
            SetPosition(position);
            if (itemData is WeaponSO weaponData)
            {
                SetItemData(weaponData);
            }
            else if (itemData is PotionSO potionData)
            {
                SetItemData(potionData);
            }
            else if (itemData is CurrencySO currencyData)
            {
                SetItemData(currencyData);
            }
        }
    }

    private void OnShowFillamount(float fillamountValue)
    {
        outlineImage.fillAmount = fillamountValue;
    }

    private void SetPosition(Vector3 promptPosition)
    {
        rectTransform.localPosition = promptPosition;
    }

    private void SetItemData(WeaponSO weaponData)
    {
        equipOrEatText.text = "장착";
        holdButton.gameObject.SetActive(true);
        promptImage.sprite = weaponData.weaponSprite;
        gradeText.text = weaponData.gradeType.ToString();
        promptName.text = weaponData.weaponName;
        promptDescription.text = weaponData.description;
        promptAttackPower.text = $"공격력 : {weaponData.attackPower.ToString()}" +
                                  $" 섭취 값 : {weaponData.eatValue.ToString()}";
    }
    private void SetItemData(PotionSO potionData)
    {
        equipOrEatText.text = "섭취";
        holdButton.gameObject.SetActive(false);
        promptImage.sprite = potionData.potionSprite;
        gradeText.text = potionData.gradeType.ToString();
        promptName.text = potionData.potionName;
        promptDescription.text = potionData.description;
        promptAttackPower.text = "";
    }
    private void SetItemData(CurrencySO currencyData)
    {
        equipOrEatText.text = "섭취";
        holdButton.gameObject.SetActive(false);
        promptImage.sprite = currencyData.currencySprite;
        gradeText.text = currencyData.gradeType.ToString();
        promptName.text = currencyData.currencyName;
        promptDescription.text = currencyData.description;
        promptAttackPower.text = "";
    }
}