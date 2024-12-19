using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// TODO: 데이터 저장을 추가하고 호출할 수 있도록 구현. 
//       파일 저장 없이 메모리에서만 임시 데이터를 저장하도록 처리
// -> UI 업데이트는 저장된 데이터를 바탕으로 동작 (DataManager에서 관리)

public class PlayerStat : Stat
{
    // TODO: Controller에서 PlayerStat 업데이트 로직 추가
    public UnityAction<string, float> OnStatUpdatedEvent;
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
            //Debug.Log("먹고난 후 : " + stats[statKey]);
            Debug.Log($"{statKey} : {currentValue}");
            OnStatUpdatedEvent?.Invoke(statKey, currentValue);
            if (statKey == "CurrentHealth" && statDictionary["CurrentHealth"] == 0)
            {
                OnDie?.Invoke();
            }
        }
        else
        {
            return;
        }
    }

    public void UpdateCurrentHealth(float currentHealth)
    {
        if (statDictionary.ContainsKey("CurrentHealth"))
        {
            float newValue = Mathf.Clamp(currentHealth, 0f, statDictionary["MaxHealth"]);
            UpdateStat("CurrentHealth", newValue);
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
    }

    public void DeletePlayerData()
    {
        hasLoadData = false;
    }
}