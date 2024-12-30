using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private GameObject player;

    private Dictionary<StageType, Stage> stages = new Dictionary<StageType, Stage>();
    private Stage currentStage = null;
    private StageType currentStageType;
    private int monsterCount;

    //일단 몬스터만 풀링
    private IObjectPool<PooledMonster> monster;

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

    public void Init()
    {
        RegisterStage();
        CacheMonster();
    }

    private void CacheMonster()
    {
        monster = PoolManager.Instance.GetObjectFromPool<PooledMonster>(PoolType.PooledMonster);
    }

    public void ResetStageManager()
    {
        stages.Clear();
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
        else
        {
            GameManager.Instance.isClear = false;
        }
        monsterCount = currentStage.stageData.monsterCount;
        //FadeIn
        currentStage.SetStage(player, monster);
        //FadeOut
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