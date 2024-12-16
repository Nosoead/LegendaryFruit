using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroupUI : MonoBehaviour
{
    [SerializeField] private List<TabButtonUI> tabButtons;
    [SerializeField] private Sprite tabIdle; 
    [SerializeField] private Sprite tabHover; // 마우스를 올렸을때 이미지
    [SerializeField] private Sprite tabActive; // 활성화 됫을때 이미지
    [SerializeField] private List<GameObject> objectsToSwap;

    private TabButtonUI selectedTab;
    public void Subscribe(TabButtonUI buttonUI)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButtonUI>();
        }
        tabButtons.Add(buttonUI);
    }

    public void OnTabEnter(TabButtonUI buttonUI)
    {
        ResetTabs();
        if (selectedTab == null || buttonUI != selectedTab)
        {
            buttonUI.SetSprite(tabHover);
        }
    }

    public void OnTabExit(TabButtonUI buttonUI)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtonUI buttonUI)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
        selectedTab = buttonUI;
        
        selectedTab.Select();
        
        ResetTabs();
        buttonUI.SetSprite(tabActive);
        int index = buttonUI.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    private void ResetTabs()
    {
        foreach (TabButtonUI button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            
            if (tabIdle != null)
            {
                button.SetSprite(tabIdle);
            }
        }
      
    }
}
