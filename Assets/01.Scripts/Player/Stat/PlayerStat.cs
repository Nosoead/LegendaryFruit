using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStat : Stat
{
    public UnityAction<string, float> OnStatUpdatedEvent;
    public UnityAction<float, float> OnHealthUpdateEvent;
    public UnityAction OnDie;
    private Dictionary<string, float> statDictionary = new Dictionary<string, float>();
    private PlayerStatData playerStatData = new PlayerStatData();
    private bool hasLoadData = false;
    public override void InitStat(GameSO gameData)
    {
        if (gameData is PlayerSO playerData)
        {
            if (!hasLoadData)
            {
                statDictionary = playerData.playerStats;
                playerStatData = playerData.playerStatData;
                playerData.sync.SyncToDictionary(statDictionary, playerStatData);
            }

            foreach (var stat in statDictionary)
            {
                OnStatUpdatedEvent?.Invoke(stat.Key, stat.Value);
            }
        }
    }

    public float GetStatValue(string statKey)
    {
        if (statDictionary.TryGetValue(statKey, out var currentValue))
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
        if (statDictionary.ContainsKey(statKey))
        {
            statDictionary[statKey] = currentValue;
            Debug.Log($"key{statKey}, Value {currentValue}");
            OnStatUpdatedEvent?.Invoke(statKey, currentValue);
            if (statKey == "MaxHealth")
            {
                OnHealthUpdateEvent?.Invoke(statDictionary["CurrentHealth"], currentValue);
            }
        }
        else
        {
            return;
        }
    }

    public void UpdateCurrentHealth(float currentHealth)
    {
        if (statDictionary["CurrentHealth"] != 0)
        {
            if (statDictionary.ContainsKey("CurrentHealth"))
            {
                float newValue = Mathf.Clamp(currentHealth, 0f, statDictionary["MaxHealth"]);
                statDictionary["CurrentHealth"] = newValue;
                OnHealthUpdateEvent?.Invoke(newValue, statDictionary["MaxHealth"]);
                if (statDictionary["CurrentHealth"] == 0)
                {
                    OnDie?.Invoke();
                }
            }
        }
    }

    public PlayerStatData SavePlayerData(PlayerSO playerData)
    {
        hasLoadData = false;
        playerData.sync.SyncToPlayerStatData(statDictionary, playerStatData);
        return playerStatData;
    }

    public void LoadPlayerData(PlayerSO playerData, PlayerStatData playerStatData)
    {
        hasLoadData = true;
        this.playerStatData = playerStatData;
        playerData.sync.SyncToDictionary(statDictionary, this.playerStatData);
        foreach (var stat in statDictionary)
        {
            OnStatUpdatedEvent?.Invoke(stat.Key, stat.Value);
        }
        OnHealthUpdateEvent?.Invoke(statDictionary["CurrentHealth"], statDictionary["MaxHealth"]);
    }

    public void DeletePlayerData()
    {
        hasLoadData = false;
    }
}