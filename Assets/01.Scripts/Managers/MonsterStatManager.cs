using UnityEngine;
using UnityEngine.Events;

public class MonsterStatManager : MonoBehaviour
{
    public UnityAction<string, float> OnSubscribeToStatUpdateEvent;
    MonsterAnimationController monsterAnimationController;
    [SerializeField] private MonsterSO monsterData;
    [SerializeField] private PooledMonster pooledMonster;
    private MonsterStat stat;
    private StatHandler statHandler;

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
    }

    private void OnDisable()
    {
        stat.OnStatUpdated -= OnStatUpdatedEvent;
        stat.OnMonsterDie -= OnMonsterDie;
    }
    private void Start()
    {
        // TODO: SaveManager를 통해 LoadData로 데이터 로드 시,
        //       Load 결과가 null인 경우 초기화 처리 추가
        SetInitStat();
    }

    public void SetInitStat()
    {
        stat.InitStat(monsterData);
    }

    private void OnStatUpdatedEvent(string key, float value)
    {
        OnSubscribeToStatUpdateEvent?.Invoke(key, value);
    }

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
        StopAllCoroutines();
        GameManager.Instance.stageManager.MonsterDie();
        gameObject.layer = LayerMask.NameToLayer("Default");
        monsterAnimationController.OnDie();
        Invoke("MonsterDieOff", 1.5f);
    }

    private void MonsterDieOff()
    {
        pooledMonster.ObjectPool.Release(pooledMonster);
        gameObject.layer = LayerMask.NameToLayer("Monster");
    }
}