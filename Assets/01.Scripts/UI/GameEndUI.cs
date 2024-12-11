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
        }
        else
        {
            titleTxt.text = "Game Over..";
        }
        exitTxt.text = "돌아가기";
        exitButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<GameEndUI>(false));
        exitButton.onClick.AddListener(() => GameManager.Instance.GameRestart());
    }
}