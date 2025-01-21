using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TabButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [FormerlySerializedAs("tabGroup")] [SerializeField] private TabGroupUI tabGroupUI;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject objectToActiveate;
    [SerializeField] private Text hoverText;
    
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
        tabGroupUI.Subscribe(this);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroupUI.OnTabEnter(this);
        ShowText(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroupUI.OnTabExit(this);
        ShowText(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroupUI.OnTabSelected(this);
        SoundManagers.Instance.PlaySFX(SfxType.UIButton);
    }

    public void Select()
    {
        if (onTabSelected != null)
        {
            onTabSelected.Invoke();
        }
    }
    public void Deselect()
    {
        if (onTabDeselected != null)
        {
            onTabDeselected.Invoke();
        }
    }

    public void SetSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            backgroundImage.sprite = sprite;
        }
    }

    private void ShowText(bool show)
    {
        if (hoverText != null)
        {
            hoverText.gameObject.SetActive(show) ;
        }
    }
}
