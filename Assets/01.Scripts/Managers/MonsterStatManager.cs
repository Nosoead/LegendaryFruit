using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MonsterStatManager : MonoBehaviour
{
    [SerializeField] private MonsterSO monsterData;
    private MonsterStat stat;
    private StatHandler statHandler;

    private void Awake()
    {
        stat = new MonsterStat();
        statHandler = new StatHandler();
    }

    private void OnEnable()
    {
        stat.OnMonsterDie += OnMonsterDie;
    }

    private void OnDisable()
    {
        stat.OnMonsterDie -= OnMonsterDie;
    }
    private void Start()
    {
        // TODO: SaveManager를 통해 LoadData로 데이터 로드 시,
        //       Load 결과가 null인 경우 초기화 처리 추가
     
    }

    public void SetInitStat()
    {
        stat.InitStat(monsterData);
    }

    public void SubscribeToStatUpdates(UnityAction<string, float> listener) // stat 구독
    {
        stat.OnStatUpdated += listener;
        
    }

    public void UnsubscribeToUpdateEvent(UnityAction<string, float> listener) // stat 구독 해제
    {
        stat.OnStatUpdated -= listener;
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
        gameObject.SetActive(false);
    }
}