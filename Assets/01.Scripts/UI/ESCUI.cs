using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ESCUI : UIBase
{
    [SerializeField] private Button cancleButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button exitButton;

    public override void Open()
    {
        base.Open();
        cancleButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<ESCUI>(true));
        settingButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<SettingUI>(false));
        
        exitButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<SettingUI>(false));
        exitButton.onClick.AddListener(() => GameManager.Instance.GameRestart());
    }
 
}

