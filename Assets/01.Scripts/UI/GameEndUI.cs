using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : UIBase
{
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI exitTxt;
    [SerializeField] private Button exitButton;

    public override void Open()
    {
        base.Open();
        if (GameManager.Instance.isClear)
        {
            titleTxt.text = "Game Clear!";
            exitButton.onClick.AddListener(() => SceneManagerExtension.Instance.LoadScene(SceneType.TitleScene));
        }
        else
        {
            titleTxt.text = "Game Over..";
            exitButton.onClick.AddListener(() => SceneManagerExtension.Instance.LoadScene(SceneType.OneCycleScene));
        }
        exitTxt.text = "돌아가기";
        exitButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<GameEndUI>(false));
    }
}