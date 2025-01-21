using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterStat : Stat
{
    public UnityAction<string, float> OnStatUpdated;
    public UnityAction OnMonsterDie;
    private Dictionary<string, float> stats = new Dictionary<string, float>();
    public UnityAction<float, float> OnHealthChanged;

    public override void InitStat(GameSO gameData)
    {
        if (gameData is MonsterSO monsterData)
        {
            stats["MaxHealth"] = monsterData.maxHealth;
            stats["CurrentHealth"] = monsterData.maxHealth;
            stats["CurrentAttackPower"] = monsterData.attackPower;
            stats["CurrentDefense"] = monsterData.defense;
            stats["AttackSpeed"] = monsterData.attackSpeed;
            stats["MoveSpeed"] = monsterData.moveSpeed;
            stats["AttackDistance"] = monsterData.attackDistance;
            stats["ChaseRange"] = monsterData.chaseRange;
            stats["AttributeType"] = (int)monsterData.type;
            stats["AttributeValue"] = monsterData.attributeValue;
            stats["AttributeStack"] = monsterData.attributeStack;
            stats["AttributeRateTime"] = monsterData.attributeRateTime;
            stats["InGameMoney"] = monsterData.inGameMoney;
            foreach (var stat in stats)
            {
                OnStatUpdated?.Invoke(stat.Key, stat.Value);
            }
        }
    }

    public float GetStatValue(string statKey)
    {
        if (stats.TryGetValue(statKey, out var currentValue))
        {
            return currentValue;
        }
        else
        {
            return -1f;
        }
    }

    public void UpdateStat(string statKey, float currentValue)
    {
        if (stats.TryGetValue(statKey, out var lastValue))
        {
            stats[statKey] = currentValue;
            OnStatUpdated?.Invoke(statKey, currentValue);
            if (statKey == "CurrentHealth" && stats["CurrentHealth"] == 0)
            {
                OnMonsterDie?.Invoke();
            }
        }
        else
        {
            return;
        }
    }

    public void UpdateCurrentHealth(float currentHealth)
    {
        if (stats.ContainsKey("CurrentHealth"))
        {
            float newValue = Mathf.Clamp(currentHealth, 0f, stats["MaxHealth"]);
            UpdateStat("CurrentHealth", newValue);
            OnHealthChanged?.Invoke(newValue, stats["MaxHealth"]);
        }
    }
}


[System.Serializable]
public class MonsterStatData
{
    public float MaxHealth;
    public float CurrentHealth;
    public float CurrentDamage;
    public float CurrentDefense;
    public float AttackSpeed;
    public float MoveSpeed;
    public float DashForce;
}