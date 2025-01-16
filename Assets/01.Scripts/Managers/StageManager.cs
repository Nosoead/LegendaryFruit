using Cinemachine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

public class StageManager : Singleton<StageManager>
{
    public UnityAction<int> OnCountFieldMonster;

    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineConfiner2D confiner;
    private Dictionary<StageType, Stage> stages = new Dictionary<StageType, Stage>();
    private Stage currentStage = null;
    private StageType currentStageType;
    [SerializeField] private Light2D mainLight;
    private int monsterCount;
    public event Action<StageType> OnPlayFadeIn;
    private bool stageClear;

    private IObjectPool<PooledMonster> monster;
    private IObjectPool<PooledBossMonster> bossMonster;

    private void Start()
    {
        PoolManager.Instance.CreatePool<PooledMonster>(PoolType.PooledMonster, false, 7, 12);
        PoolManager.Instance.CreatePool<PooledBossMonster>(PoolType.PooledBossMonster, false, 7, 12);

        CacheMonster();
    }

    public void Init()
    {
        RegisterStage();
        SetCameraBoundary();
        CacheMonster();
        if (mainLight == null)
        {
            GameObject mainLight = GameObject.Find("GlobalLight2D");
            this.mainLight = mainLight.GetComponent<Light2D>();
        }
    }

    private void RegisterStage()
    {
        foreach (StageType stageType in Enum.GetValues(typeof(StageType)))
        {
            Stage resourceStage = ResourceManager.Instance.LoadResource<Stage>($"Stage/{stageType}");
            if (resourceStage != null && !stages.ContainsKey(stageType))
            {
                var obj = Instantiate(resourceStage);
                stages.Add(stageType, obj);
                obj.gameObject.SetActive(false);
            }
        }
    }

    private void SetCameraBoundary()
    {
        if (confiner == null)
        {
            if (GameObject.Find("Virtual Camera").TryGetComponent(out CinemachineVirtualCamera virutalCam))
            {
                confiner = virutalCam.GetComponent<CinemachineConfiner2D>();
            }
        }
    }

    private void CacheMonster()
    {
        monster = PoolManager.Instance.GetObjectFromPool<PooledMonster>(PoolType.PooledMonster);
        bossMonster = PoolManager.Instance.GetObjectFromPool<PooledBossMonster>(PoolType.PooledBossMonster);
    }

    public void ResetStageManager()
    {
        stages.Clear();
        confiner = null;
    }

    public void ChangeStage(StageType type)
    {
        stageClear = false;
        mainLight.intensity = 1;
        if (player == null)
        {
            player = GameManager.Instance.player;
        }
        if (currentStage != null)
        {
            currentStage.PoolRelease();
            Stage lastStage = currentStage;
            lastStage.gameObject.SetActive(false);
        }
        currentStageType = type;
        currentStage = stages[currentStageType];
        currentStage.gameObject.SetActive(true);
        if (!currentStage.GetCombatData())
        {
            GameManager.Instance.SetGameClear(true);
        }
        else if (currentStage.GetBossData())
        {
            GameManager.Instance.SetGameClear(false);
            OnPlayFadeIn?.Invoke(type);
            currentStage.SetStage(player, bossMonster, confiner);
            return;
        }
        else
        {
            GameManager.Instance.SetGameClear(false);
        }
        monsterCount = currentStage.GetMonsterCount();
        OnCountFieldMonster?.Invoke(monsterCount);
        OnPlayFadeIn?.Invoke(type);
        currentStage.SetStage(player, monster, confiner);
    }

    public void MonsterDie()
    {
        monsterCount--;
        monsterCount = Mathf.Max(monsterCount, 0);
        OnCountFieldMonster?.Invoke(monsterCount);
        CheckClear();
    }

    private void CheckClear()
    {
        if (monsterCount == 0)
        {
            if (currentStage.GetStageID() == (int)StageType.StageBoss)
            {
                UIManager.Instance.ToggleUI<BossHPUI>(false,false);
                GameManager.Instance.SetGameClear(true);
            }
            else
            {
                if (currentStage.GetCombatData())
                {
                    stageClear = true;
                    DOTween.To(() => mainLight.intensity, x => mainLight.intensity = x, 0.5f, 0.5f).SetEase(Ease.Linear);
                }
                currentStage.SetReward();
            }
        }
    }

    public StageType GetCurrentStage()
    {
        return currentStageType;
    }

    public bool GetStageClear()
    {
        return stageClear;
    }
}