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
        //test용
        player = GameObject.Find("Mainplayer");
    }

    private void Start()
    {
        GameStart();
        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
    }

    public void GameStart()
    {
        StageManager.Instance.CreatStage();
        StageManager.Instance.StartStage("Lobby");
        //StageManager.Instance.CreatRewardTree();
    }

    public void GameEnd()
    {
        Debug.Log("GameEnd");
        //Time.timeScale = 0f;
        UIManager.Instance.ToggleUI<GameEndUI>(isPreviousWindowActive: false);

        //TODO UI창 끄면 Scene 다시Load하면서 시작하기
    }

    public void GameRestart()
    {
        //TODO 실제 게임 하면 LobbyScene이거나 싹다 초기화해서 lobby맵 띄우기
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("TitleScene");
    }
}