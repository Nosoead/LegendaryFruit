using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatManager : MonoBehaviour
{
    [SerializeField] private PlayerSO playerData;
    [SerializeField] private PlayerInteraction playerInteraction;
    private PlayerStat stat;
    private StatHandler statHandler;

    private void Awake()
    {
        if (playerInteraction == null)
        {
            playerInteraction = GetComponent<PlayerInteraction>();
        }
        stat = new PlayerStat();
        statHandler = new StatHandler();
    }

    private void OnEnable()
    {
        playerInteraction.FruitWeaponEatAndStatUpEvent += IncreaseStat;
        stat.OnDie += OnDie;
    }

    private void OnDisable()
    {
        playerInteraction.FruitWeaponEatAndStatUpEvent -= IncreaseStat;
        stat.OnDie -= OnDie;
    }
    private void Start()
    {
        // TODO: SaveManager를 통해 LoadData로 데이터 로드 시,
        //       Load 결과가 null인 경우 초기화 처리 추가
        stat.InitStat(playerData);
    }

    public void SubscribeToStatUpdateEvent(UnityAction<string, float> listener)
    {
        stat.OnStatUpdated += listener;
    }

    public void UnsubscribeToStatUpdateEvent(UnityAction<string, float> listener)
    {
        stat.OnStatUpdated -= listener;
    }

    #region /ApplystatMethod
    public void ApplyInstantDamage(float damage)
    {
        float result = statHandler.Substract(stat.GetStatValue("CurrentHealth"), damage);
        stat.UpdateCurrentHealth(result);
    }

    public void ApplyTemporaryStatReduction(float attributeValue, string statKey)
    {
        float result = statHandler.Substract(stat.GetStatValue(statKey), attributeValue);
        stat.UpdateStat(statKey, result);
    }

    public void ApplyRestoreStat(float attributeValue, string statKey)
    {
        float result = statHandler.Add(stat.GetStatValue(statKey), attributeValue);
        stat.UpdateStat(statKey, result);
    }

    public void Heal(float heal)
    {
        float result = statHandler.Add(stat.GetStatValue("CurrentHealth"), heal, stat.GetStatValue("MaxHealth"));
        stat.UpdateCurrentHealth(result);
    }

    private void IncreaseStat(string statKey, float eatValue)
    {
        //Debug.Log("벨류 업 : " + eatValue);
        float result = statHandler.Add(stat.GetStatValue(statKey), eatValue);
        stat.UpdateStat(statKey, result);
        //Debug.Log("먹고난 후 : " + result);
    }
    #endregion

    private void OnDie()
    {
        GameManager.Instance.GameEnd();
    }
}