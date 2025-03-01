using System.Collections.Generic;
using UnityEngine;


public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>();
    private Stack<UIBase> uiActiveStack = new Stack<UIBase>();
    private Dictionary<string, UIBase> persistentUI = new Dictionary<string, UIBase>();
    private string path;

    public bool IsSettingOpen { get; private set; }

    public void ForeInit()
    {
        SetUIDictionary();
    }

    public void PostInit()
    {
        OpenPersistentUI<PlayerCanvasUI>(true);
        OpenPersistentUI<FadeInUI>(true);
        OpenPersistentUI<PromptUI>(true);
    }

    private void SetUIDictionary()
    {
        path = "UI";
        UIBase[] uiArray = ResourceManager.Instance.LoadAllResources<UIBase>($"{path}");
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

    private void OpenUI<T>(T ui, bool isPreviousWindowActive) where T : UIBase
    {
        ui.Open();
        if (uiActiveStack.TryPeek(out UIBase result) && !isPreviousWindowActive)
        {
            result.gameObject.SetActive(false);
        }
        uiActiveStack.Push(ui);
    }

    private void CloseUI<T>(T ui, bool isPreviousWindowActive) where T : UIBase
    {
        ui.Close();
        if (!isPreviousWindowActive && uiActiveStack.TryPeek(out UIBase result))
        {
            result.gameObject.SetActive(true);
        }
    }

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


    public void ToggleUI<T>(bool isPreviousWindowActive, bool isOpened) where T : UIBase
    {
        T ui = GetUI<T>();
        if (ui == null)
        {
            return;
        }
        if (ui.gameObject.activeSelf && !isOpened)
        {
            CloseUI(ui, isPreviousWindowActive);
        }
        else if (!ui.gameObject.activeSelf && isOpened)
        {
            OpenUI(ui, isPreviousWindowActive);
        }
    }

    public void ToggleUI<T>(bool isPreviousWindowActive, int index) where T : UIBase
    {
        T ui = GetUI<T>();
        if (ui == null)
        {
            return;
        }
        if (ui is NpcDialougeUI dialougeUI)
        {
            if (ui.gameObject.activeSelf)
            {
                CloseUI(ui, isPreviousWindowActive);
            }
            else
            {
                dialougeUI.GetDialougeIndex(index);
                OpenUI(ui, isPreviousWindowActive);
            }
        }
        else
        {
            UnityEngine.Debug.LogError("잘못된 UI사용방식입니다.");
        }
    }

    public void OpenPersistentUI<T>(bool isPersistentOpen)
    {
        var uiName = typeof(T).Name;
        var uiPrefab = uiDictionary[uiName];
        var uiInstance = Instantiate(uiPrefab);
        if (uiInstance is IPersistentUI)
        {
            uiInstance.Open();
            persistentUI.Add(uiName, uiInstance);
            if (isPersistentOpen)
            {
                uiInstance.gameObject.SetActive(true);
            }
            if (!isPersistentOpen)
            {
                uiInstance.gameObject.SetActive(false);
            }
        }
    }

    public UIBase GetPersistentUI<T>()
    {
        var uiName = typeof(T).Name;
        return persistentUI[uiName];
    }

    public void ResetUI()
    {
        foreach (var ui in persistentUI)
        {
            ui.Value.Close();
        }
        persistentUI.Clear();
        uiActiveStack.Clear();
    }

    public void ToggleSettingState(bool isOpen)
    {
        IsSettingOpen = isOpen;
    }
}
