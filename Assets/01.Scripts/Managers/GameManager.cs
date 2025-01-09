using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public UnityAction OnGameClear;

    public GameObject player;
    private bool isClear = false;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.Find("Mainplayer");
    }

    public void Init()
    {
        if (player == null)
        {
            player = GameObject.Find("Mainplayer");
        }
    }

    public void GameStart()
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

    public void SetGameClear(bool isClear)
    {
        this.isClear = isClear;
        OnGameClear?.Invoke();
    }

    public bool GetGameClear()
    {
        return isClear;
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