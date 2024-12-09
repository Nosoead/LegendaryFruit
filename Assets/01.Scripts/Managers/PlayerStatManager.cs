using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    [SerializeField] private PlayerSO playerData;
    private PlayerStat stat;
    private StatHandler statHandler;

    private void Awake()
    {
        stat = new PlayerStat();
        statHandler = new StatHandler();
    }

    private void Start()
    {
        //TODO : SaveManager ���� �� LoadData�� ���� �������� �� ��,
        //       Load ���� null ���ο� ���� �ʱ�ȭ �� ��
        stat.InitStat(playerData);
    }

    public void ApplyInstantDamage(float damage)
    {
        float result = statHandler.Substract(stat.GetStatValue("CurrentHealth"), damage);
        stat.UpdateCurrentHealth(result);
    }

    public void ApplyDamageOverTime(float attributeValue, float attributeRateTiem, int attributeStack)
    {
        
    }

    public void ApplyTemporaryStatReduction(float attributeValue, float attributeRateTiem)
    {
        
    }

    public void Heal(float heal)
    {
        float result = statHandler.Add(stat.GetStatValue("CurrentHealth"), heal, stat.GetStatValue("MaxHealth"));
        stat.UpdateCurrentHealth(result);
    }

    public void IncreaseStat(string statKey, float eatValue)
    {
        float result = statHandler.Add(stat.GetStatValue(statKey), eatValue);
        stat.UpdateStat(statKey, result);
    }
}