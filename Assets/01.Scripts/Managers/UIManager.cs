using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    //옵션에 따라 이전 UI창 false로 할지 안할지
    //필드에 사전 만들기
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>();
    private Stack<UIBase> uiActiveStack = new Stack<UIBase>();
    private string path;

    private void Start()
    {
        SetUIDictionary();
        path = "UI";
    }

    private void SetUIDictionary()
    {
        UIBase[] uiArray = ResourceManager.Instance.LoadAllResources<UIBase>(path);
        foreach (var ui in uiArray)
        {
            string uiName = ui.GetType().Name;
            if (!uiDictionary.ContainsKey(uiName))
            {
                uiDictionary.Add(uiName, ui);
            }
        }
    }

    private T GetUI<T>() where T : UIBase
    {
        var uiName = typeof(T).Name;

        if (uiActiveStack.TryPeek(out UIBase uiStack) && uiStack is T prefab)
        {
            uiActiveStack.Pop();
            return uiStack as T;
        }
        else if (uiDictionary.TryGetValue(uiName, out var uiPrefab) && uiPrefab != null)
        {
            var uiInstance = Instantiate(uiPrefab);
            uiInstance.gameObject.SetActive(false);
            return uiInstance as T;
        }
        return null;
    }

    //열기
    private void OpenUI<T>(T ui, bool isPreviousWindowActive) where T : UIBase
    {
        ui.Open();
        if (uiActiveStack.TryPeek(out UIBase result) && !isPreviousWindowActive)
        {
            result.gameObject.SetActive(false);
        }
        uiActiveStack.Push(ui);
    }

    //닫기
    private void CloseUI<T>(T ui, bool isPreviousWindowActive) where T : UIBase
    {
        ui.Close();
        if (!isPreviousWindowActive && uiActiveStack.TryPeek(out UIBase result))
        {
            result.gameObject.SetActive(true);
        }
    }

    //토글
    public void ToggleUI<T>(bool isPreviousWindowActive) where T : UIBase
    {
        T ui = GetUI<T>();
        if (ui == null)
        {
            return;
        }
        if (ui.gameObject.activeSelf)
        {
            CloseUI(ui, isPreviousWindowActive);
        }
        else
        {
            OpenUI(ui, isPreviousWindowActive);
        }
    }
}
