using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterStatManager : MonoBehaviour
{
    public UnityAction<string, float> OnSubscribeToStatUpdateEvent;
    public event UnityAction<PatternData,float> OnPatternTriggered;
    private MonsterAnimationController monsterAnimationController;
    [SerializeField] private PooledMonster pooledMonster;
    private MonsterStat stat;
    private StatHandler statHandler;
    private Dictionary<int,PatternData> pattrens = new Dictionary<int,PatternData>();
    private PatternData currentPattrenData;

    [Header("PattrenStat")]
    private bool isOnCooldown = false;
    private float cooldownTime;
    private float patternDagmae;    

    public bool isDead = false;

    private void Awake()
    {
        if(monsterAnimationController == null)
        {
            monsterAnimationController = GetComponent<MonsterAnimationController>();
        }
        if (pooledMonster == null)
        {
            pooledMonster = GetComponent<PooledMonster>();
        }
        stat = new MonsterStat();
        statHandler = new StatHandler();
    }

    private void OnEnable()
    {
        stat.OnStatUpdated += OnStatUpdatedEvent;
        stat.OnMonsterDie += OnMonsterDie;
        stat.OnHealthChanged += OnPattrenToHealth;
    }

    private void OnDisable()
    {
        stat.OnStatUpdated -= OnStatUpdatedEvent;
        stat.OnMonsterDie -= OnMonsterDie;
        stat.OnHealthChanged -= OnPattrenToHealth;
    }
    private void Start()
    {
        // TODO: SaveManager를 통해 LoadData로 데이터 로드 시,
        //       Load 결과가 null인 경우 초기화 처리 추가
        //SetInitStat();
    }

    public void SetInitStat(MonsterSO data)
    {
        if(data is RegularMonsterSO regularMonsterData)
        {
            stat.InitStat(regularMonsterData);
        }
        else if (data is BossMonsterSO bossMonsterData)
        {
            stat.InitStat(bossMonsterData);
            //SetPattrenStat();
        }
    }

    private void OnStatUpdatedEvent(string key, float value)
    {
        OnSubscribeToStatUpdateEvent?.Invoke(key, value);
    }

    #region Pattren
    public void SetPattrenStat(Dictionary<int, PatternData> pattrenData)
    {
        pattrens = pattrenData;
    }

    private void OnPattrenToHealth(float currentHealth)
    {
        var data = GetPatternData(currentHealth);
        if(currentPattrenData == data && !isOnCooldown)
        {
            OnPatternTriggered?.Invoke(data, patternDagmae);
            StartCooldown();
        }
    }
    private void StartCooldown()
    {
        isOnCooldown = true;
        Invoke(nameof(ResetCooldown), cooldownTime);
        Debug.Log($"{cooldownTime}");
    }

    private void ResetCooldown()
    {
        isOnCooldown = false;
    }

    private PatternData GetPatternData(float health)
    {
        if(health <= 200)
        {
            var pattern = pattrens[1000];
            cooldownTime = pattern.pattrenCoolTime;
            patternDagmae = pattern.patternDamage;
            currentPattrenData = pattern;
            return pattern;
        }
        return null;
    }

    #endregion

    #region /ApplystatMethod
    public void ApplyInstantDamage(float damage) // stat 데미지
    {
        float result = statHandler.Substract(stat.GetStatValue("CurrentHealth"), damage);
        stat.UpdateCurrentHealth(result);
    }

    public void ApplyTemporaryStatReduction(float attributeValue, string statKey) // stat 감소
    {
        float result = statHandler.Substract(stat.GetStatValue(statKey), attributeValue);
        stat.UpdateStat(statKey, result);
    }

    public void ApplyRestoreStat(float attributeValue, string statKey) // stat 복구
    {
        float result = statHandler.Add(stat.GetStatValue(statKey), attributeValue);
        stat.UpdateStat(statKey, result);
    }

    public void Heal(float heal) //stat 힐
    {
        float result = statHandler.Add(stat.GetStatValue("CurrentHealth"), heal, stat.GetStatValue("MaxHealth"));
        stat.UpdateCurrentHealth(result);
    }

    public void IncreaseStat(string statKey, float eatValue) //stat 증가
    {
        float result = statHandler.Add(stat.GetStatValue(statKey), eatValue);
        stat.UpdateStat(statKey, result);
    }
    #endregion

    private void OnMonsterDie()
    {
        isDead = true;
        StopAllCoroutines();
        StageManager.Instance.MonsterDie();
        SoundManagers.Instance.PlaySFX(SfxType.MonsterDeath);
        gameObject.layer = LayerMask.NameToLayer("Default");
        monsterAnimationController.OnDie();
        Invoke("MonsterDieOff", 1.5f);
    }

    private void MonsterDieOff()
    {
        pooledMonster.ObjectPool.Release(pooledMonster);
        gameObject.layer = LayerMask.NameToLayer("Monster");
        isDead = false;
    }
}