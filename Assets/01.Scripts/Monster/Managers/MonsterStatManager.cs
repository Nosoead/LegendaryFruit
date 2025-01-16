using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MonsterStatManager : MonoBehaviour
{
    public UnityAction<string, float> OnSubscribeToStatUpdateEvent;
    public event UnityAction<PatternData, float> OnPatternTriggered;
    public event UnityAction<float, AttributeType> DamageTakenEvent;
    public event UnityAction<RangedAttackData> OnRangedAttackDataEvent;
    public event UnityAction<List<RegularPatternData>> OnRegularPatternDataEvent;
    public UnityAction<float, bool> OnShowHealthBarEvent;
    public event UnityAction OnDieEvent;
    private MonsterAnimationController monsterAnimationController;
    private MonsterCondition condition;
    private PooledMonster pooledMonster;
    private PooledBossMonster pooledBossMonster;
    private MonsterStat stat;
    private StatHandler statHandler;
    private RangedAttackData rangedAttackData;
    private CurrencySystem currencySystem;
    private float inGameCurrency;

    [Header("PattrenStat")] private Dictionary<int, PatternData> pattrens = new Dictionary<int, PatternData>();
    private RegularPatternData regularPatternData;
    private PatternData currentPattrenData;
    private bool isOnCooldown = false;
    private float cooldownTime;
    private float patternDagmae;

    private AttributeType currentTakeAttributeType;

    public bool isDead = false;

    private void Awake()
    {
        if (monsterAnimationController == null)
        {
            monsterAnimationController = GetComponent<MonsterAnimationController>();
        }

        if (pooledMonster == null)
        {
            pooledMonster = GetComponent<PooledMonster>();
        }

        if (pooledBossMonster == null)
        {
            pooledBossMonster = GetComponentInParent<PooledBossMonster>();
        }

        if (condition == null)
        {
            condition = GetComponent<MonsterCondition>();
        }

        if (currencySystem == null)
        {
            if (GameManager.Instance.player.gameObject.TryGetComponent(out CurrencySystem currencySys))
            {
                currencySystem = currencySys;
            }
        }

        stat = new MonsterStat();
        statHandler = new StatHandler();
    }

    private void OnEnable()
    {
        stat.OnStatUpdated += OnStatUpdatedEvent;
        stat.OnMonsterDie += OnMonsterDie;
        stat.OnHealthChanged += OnPattrenToHealth;
        stat.OnHealthChanged += OnHealthData;
        condition.OnTakeHitType += OnAttributeTypeReceived;
    }

    private void OnDisable()
    {
        stat.OnStatUpdated -= OnStatUpdatedEvent;
        stat.OnMonsterDie -= OnMonsterDie;
        stat.OnHealthChanged -= OnPattrenToHealth;
        stat.OnHealthChanged -= OnHealthData;
        condition.OnTakeHitType -= OnAttributeTypeReceived;
    }

    public void SetInitStat(MonsterSO data)
    {
        CachedRanagedAttackData(data);
        if (data is RegularMonsterSO regularMonsterData)
        {
            SetRegularPatternData(regularMonsterData);
            if (regularMonsterData.monsterRagnedAttackData.Count > 0)
            {
                OnRangedAttackDataEvent?.Invoke(rangedAttackData);
                stat.InitStat(regularMonsterData);
                inGameCurrency = data.inGameMoney;
                return;
            }

            stat.InitStat(regularMonsterData);
        }
        else if (data is BossMonsterSO bossMonsterData)
        {
            if (bossMonsterData.monsterRagnedAttackData != null)
            {
                OnRangedAttackDataEvent?.Invoke(rangedAttackData);
                stat.InitStat(bossMonsterData);
                inGameCurrency = data.inGameMoney;
                return;
            }

            stat.InitStat(bossMonsterData);
        }

        inGameCurrency = data.inGameMoney;
    }

    private void OnStatUpdatedEvent(string key, float value)
    {
        OnSubscribeToStatUpdateEvent?.Invoke(key, value);
    }

    public void OnAttributeTypeReceived(AttributeType type)
    {
        currentTakeAttributeType = type;
    }

    private void OnHealthData(float currentHealth, float maxHealth)
    {
        if (currentHealth > 0)
        {
            OnShowHealthBarEvent?.Invoke(currentHealth / maxHealth,true);
        }
        else
        {
            OnShowHealthBarEvent?.Invoke(currentHealth / maxHealth,false);
        }
    }

    #region Pattren

    public void SetPattrenStat(Dictionary<int, PatternData> pattrenData)
    {
        pattrens = pattrenData;
    }

    private void OnPattrenToHealth(float currentHealth, float maxHealth)
    {
        if (currentHealth <= 200 && maxHealth >= 200)
        {
            var data = GetPatternData(currentHealth);
            if (currentPattrenData == data && !isOnCooldown)
            {
                OnPatternTriggered?.Invoke(data, patternDagmae);
                StartCooldown();
            }
        }
    }

    private void StartCooldown()
    {
        isOnCooldown = true;
        Invoke(nameof(ResetCooldown), cooldownTime);
    }

    private void ResetCooldown()
    {
        isOnCooldown = false;
    }

    private PatternData GetPatternData(float health)
    {
        if (health <= 200)
        {
            var pattern = pattrens[1000];
            cooldownTime = pattern.pattrenCoolTime;
            patternDagmae = pattern.patternDamage;
            currentPattrenData = pattern;
            return pattern;
        }

        return null;
    }

    private void CachedRanagedAttackData(MonsterSO data)
    {
        if (data.monsterRagnedAttackData.Count == 0) return;
        for (int i = 0; i < data.monsterRagnedAttackData.Count; i++)
        {
            rangedAttackData = data.monsterRagnedAttackData[i];
        }
    }

    private void SetRegularPatternData(RegularMonsterSO data)
    {
        if (data.patterns.Count == 0) return;
        OnRegularPatternDataEvent?.Invoke(data.patterns);
    }

    #endregion

    #region /ApplystatMethod

    public void ApplyInstantDamage(float damage) // stat 데미지
    {
        float result = statHandler.Substract(stat.GetStatValue("CurrentHealth"), damage);
        DamageTakenEvent?.Invoke(damage, currentTakeAttributeType);
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
        if (!isDead)
        {
            StageManager.Instance.MonsterDie();
            SoundManagers.Instance.PlaySFX(SfxType.MonsterDeath);
            currencySystem.GetCurrency((int)inGameCurrency, isGlobalCurrency: false);
        }

        isDead = true;
        condition.StopAllCoroutines();
        gameObject.layer = LayerMask.NameToLayer("Default");
        monsterAnimationController.OnDie();
        OnDieEvent?.Invoke();
        Invoke("MonsterDieOff", 1.5f);
    }

    private void MonsterDieOff()
    {
        if (pooledBossMonster != null)
        {
            pooledBossMonster.ObjectPool.Release(pooledBossMonster);
        }
        else
        {
            pooledMonster.ObjectPool.Release(pooledMonster);
        }

        gameObject.layer = LayerMask.NameToLayer("Monster");
        isDead = false;
    }

    public MonsterStat GetStat()
    {
        return stat;
    }
}