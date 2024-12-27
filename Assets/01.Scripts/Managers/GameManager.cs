using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject player;
    public bool isClear = false;
    public bool isGetWeapon = false;
    public bool isCreatReward = false;

    protected override void Awake()
    {
        base.Awake(); 
        player = GameObject.Find("Mainplayer");
        //test용
    }

    public void Init()
    {
        if (player == null)
        {
            player = GameObject.Find("Mainplayer");
        }
        GameStart();
    }

    public void GameStart()
    {
        StageManager.Instance.ChangeStage(StageType.Stage0);
        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
    }

    public void GameEnd()
    {
        Invoke(nameof(DelayedGameEnd), 1f);
    }

    private void DelayedGameEnd()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ToggleUI<GameEndUI>(isPreviousWindowActive: false);
    }

}