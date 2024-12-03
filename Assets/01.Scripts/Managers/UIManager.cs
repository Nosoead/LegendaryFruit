using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    //필드에 사전 만들기
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>();
    private string path;

    private void Start()
    {
        SetUIDictionary();
        path = "UI";
    }

    private void SetUIDictionary()
    {
        UIBase[] uiArray = ResourceManager.Instance.LoadAllResources<UIBase>("UI");
        foreach (var ui in uiArray)
        {
            string uiName = ui.GetType().Name;
            if (!uiDictionary.ContainsKey(uiName))
            {
                uiDictionary.Add(uiName, ui);
            }
        }
        Debug.Log(uiDictionary.Keys + " ||| " + uiDictionary.Values);
    }

    private T GetUI<T>() where T : UIBase
    {
        var uiName = typeof(T).Name;

        if (uiDictionary.TryGetValue(uiName, out var uiPrefab))
        {
            Debug.Log("1");
            return uiPrefab as T;
        }
        else
        {
            Debug.Log("2");
            var uiObj = uiPrefab as T;
            return Instantiate(uiObj);
        }
    }

    ////TODO 경로 설정 학습 후 재개
    //private T CreateUI<T>() where T : UIBase
    //{
    //    var uiName = typeof(T).Name;
    //    //if (!uiDictionary.ContainsKey(uiName))
    //    //{
    //    //    uiDictionary.Add(uiName, ui);
    //    //}
    //    uiDictionary.TryGetValue(uiName, out var uiPrefab);
    //    T uiRes = uiPrefab as T;
    //    var uiObj = Instantiate(uiRes);

    //    return uiObj;
    //}

    //열기
    private void OpenUI<T>(T ui) where T : UIBase
    {
        ui.Open();
    }

    //닫기
    private void CloseUI<T>(T ui) where T : UIBase
    {
        ui.Close();
    }

    //토글
    public void ToggleUI<T>() where T : UIBase
    {
        T ui = GetUI<T>();
        if (!ui.gameObject.activeSelf)
        {
            Debug.Log("11");
            CloseUI(ui);
        }
        else
        {
            Debug.Log("22");
            OpenUI(ui);
        }
        Debug.Log(ui.gameObject.activeSelf);
    }
}
