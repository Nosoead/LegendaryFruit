using UnityEngine;

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

    private void GameStart()
    {
        if (DataManager.Instance.GetCanLoad<SaveDataContainer>())
        {
            Load();
            StageManager.Instance.ChangeStage(PlayerInfoManager.Instance.GetStageID());
        }
        else
        {
            StageManager.Instance.ChangeStage(StageType.StageLobby);
        }
        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
    }

    public void GameEnd()
    {
        //DelayedGameEnd();
        Invoke(nameof(DelayedGameEnd), 1f);
    }

    public void GameEnd(bool isEnd)
    {
        if (isEnd)
        {
            DelayedGameEnd();
        }
        else
        {
            return;
        }

    }

    private void DelayedGameEnd()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ToggleUI<GameEndUI>(isPreviousWindowActive: false, true);
    }

    public void Save()
    {
        PlayerInfoManager.Instance.Save();
    }

    private void Load()
    {
        PlayerInfoManager.Instance.Load();
    }
}