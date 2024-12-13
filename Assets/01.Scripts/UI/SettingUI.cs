using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIBase
{
    [SerializeField] private Button exitButton;

    public override void Open()
    {
        base.Open();

        exitButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<SettingUI>(false));
    }
}
