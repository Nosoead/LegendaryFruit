using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public UnityAction OnGameClear;

    public GameObject player;
    private bool isClear = false;
    private string savedTimeKey = "SavedTime";
    private TimeSpan currentTime;
    private TimeSpan sceneLoadTime;

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
        sceneLoadTime = TimeSpan.FromSeconds(Time.time); ;
    }

    public void GameStart()
    {
        if (DataManager.Instance.GetCanLoad<SaveDataContainer>())
        {
            Load();
            LoadSavedTime();
            StageManager.Instance.ChangeStage(PlayerInfoManager.Instance.GetStageID());
        }
        else
        {
            ResetSavedTiem();
            StageManager.Instance.ChangeStage(StageType.StageLobby);
        }

        if (DataManager.Instance.GetCanLoad<PersistentData>())
        {
            PlayerInfoManager.Instance.GlobalLoad();
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
        ResetSavedTiem();
    }

    public void Save()
    {
        PlayerInfoManager.Instance.Save();
    }

    private void Load()
    {
        PlayerInfoManager.Instance.Load();
    }


    private void OnApplicationQuit()
    {
        SaveCurrentTime();
        Destroy(gameObject);
    }

    #region /Time
    private void SaveCurrentTime()
    {
        TimeSpan currentTime = this.currentTime + TimeSpan.FromSeconds(Time.time);
        PlayerPrefs.SetString(savedTimeKey, currentTime.ToString());
        PlayerPrefs.Save();
    }

    private void LoadSavedTime()
    {
        string savedTimeString = PlayerPrefs.GetString(savedTimeKey, "00:00:00");
        TimeSpan savedTime = TimeSpan.Parse(savedTimeString);
        currentTime = savedTime;
    }

    private void ResetSavedTiem()
    {
        PlayerPrefs.SetString(savedTimeKey, "00:00:00");
        currentTime = TimeSpan.Zero;
        PlayerPrefs.Save();
    }

    public TimeSpan GetCurrentTimeData()
    {
        TimeSpan timeSpan;
        timeSpan = currentTime - sceneLoadTime + TimeSpan.FromSeconds(Time.time);
        return timeSpan;
    }
    #endregion
}