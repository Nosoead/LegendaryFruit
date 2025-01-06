using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineConfiner2D confiner;
    private Dictionary<StageType, Stage> stages = new Dictionary<StageType, Stage>();
    private Stage currentStage = null;
    private StageType currentStageType;
    private int monsterCount;

    //일단 몬스터만 풀링
    private IObjectPool<PooledMonster> monster;
    private IObjectPool<PooledBossMonster> bossMonster;

    public event Action<StageType> onFadeImage;

    protected override void Awake()
    {
        base.Awake();
        //RegisterStage();
    }

    private void Start()
    {
     
        //TODO : 오브젝트풀 시작하자마자 다 뽑아와서 등록까지 할 것.
        //instance찍고 딕셔너리로 참조만 하면 바로 풀 사용할 수 있도록, CreatePool사용없이
        PoolManager.Instance.CreatePool<PooledMonster>(PoolType.PooledMonster, false, 7, 12);
        PoolManager.Instance.CreatePool<PooledBossMonster>(PoolType.PooledBossMonster, false, 7, 12);

        CacheMonster();
    }

    public void Init()
    {
        RegisterStage();
        SetCameraBoundary();
        CacheMonster();
      
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
        
        if (player == null)
        {
            player = GameManager.Instance.player;
        }
        if (currentStage != null)
        {
            Stage lastStage = currentStage;
            lastStage.gameObject.SetActive(false);
        }
        currentStageType = type;
        currentStage = stages[currentStageType];
        currentStage.gameObject.SetActive(true);
        if (!currentStage.stageData.isCombatStage)
        {
            GameManager.Instance.isClear = true;
        }
        else if (currentStage.stageData.isBossStage)
        {
            GameManager.Instance.isClear = false;
            currentStage.SetStage(player, bossMonster, confiner);
            return;
        }
        else
        {
            GameManager.Instance.isClear = false;
        }

        monsterCount = currentStage.stageData.TotalMonsterCount();
        
        if (!UIManager.Instance.IsUIActive<FadeImage>())
        {
            UIManager.Instance.ToggleUI<FadeImage>(true);
        }

        onFadeImage?.Invoke(type);
        currentStage.SetStage(player, monster, confiner);
    }
    
    public void MonsterDie()
    {
        monsterCount--;
        monsterCount = Mathf.Max(monsterCount, 0);
        CheckClear();
    }

    private void CheckClear()
    {
        if (monsterCount == 0)
        {
            GameManager.Instance.isClear = true;
            if (GameManager.Instance.isClear)
            {
                currentStage.SetReward();
            }
            if (currentStage.stageData.stageID == (int)StageType.StageBoss)
            {
                GameManager.Instance.GameEnd();
            }
        }
    }

    public StageType GetCurrentStage()
    {
        return currentStageType;
    }
}