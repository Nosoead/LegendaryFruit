using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TabButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [FormerlySerializedAs("tabGroup")] [SerializeField] private TabGroupUI tabGroupUI;
    [SerializeField] private Image backgroundImage;
    
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;
    // Start is called before the first frame update
    private void Start()
    {
        backgroundImage = GetComponent<Image>();
        tabGroupUI.Subscribe(this);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroupUI.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroupUI.OnTabExit(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroupUI.OnTabSelected(this);
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
}
