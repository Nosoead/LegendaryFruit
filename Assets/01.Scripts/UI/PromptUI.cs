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
    [SerializeField] private Image outlineImage;
    [SerializeField] private PlayerInteraction interaction;

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

    private void OnShowPromptEvent(WeaponSO weaponData, Vector3 position, bool isOpen)
    {
        if (!isOpen)
        {
            promptWindow.gameObject.SetActive(false);
        }
        else
        {
            promptWindow.gameObject.SetActive(true);
            SetPosition(position);
            SetItemData(weaponData);
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
        promptImage.sprite = weaponData.weaponSprite;
        gradeText.text = weaponData.gradeType.ToString();
        promptName.text = weaponData.weaponName;
        promptDescription.text = weaponData.description;
    }
}