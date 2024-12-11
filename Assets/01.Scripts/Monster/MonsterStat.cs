using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterStat : Stat
{
    public UnityAction<string, float> OnStatUpdated;
    public UnityAction OnMonsterDie;
    private Dictionary<string, float> stats = new Dictionary<string, float>();

    public override void InitStat(GameSO gameData)
    {
        if (gameData is MonsterSO monsterData)
        {
            stats["maxHealth"] = monsterData.maxHealth;
            stats["currentHealth"] = monsterData.maxHealth;
            stats["currentAttackPower"] =monsterData.attackPower;
            stats["currentDefense"] =monsterData.defense;
            stats["attackSpeed"] = monsterData.attackSpeed;
            stats["moveSpeed"] = monsterData.moveSpeed;
            stats["attackDistance"] = monsterData.attackDistance;
            stats["chaseRange"] = monsterData.chaseRange;
            stats["AttributeType"] = (int)monsterData.type;
            stats["attributeValue"] = monsterData.attributeValue;
            stats["attributeRateTime"] = monsterData.attributeRateTime;
            stats["inGameMoney"] = monsterData.inGameMoney;
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
            if (statKey == "currentHealth" && stats["currentHealth"] == 0)
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
        if (stats.ContainsKey("currentHealth"))
        {
            float newValue = Mathf.Clamp(currentHealth, 0f, stats["maxHealth"]);
            UpdateStat("currentHealth", newValue);
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
