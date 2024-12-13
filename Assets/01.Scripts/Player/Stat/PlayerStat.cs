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
    public UnityAction<string, float> OnStatUpdated;
    public UnityAction OnDie;
    private Dictionary<string, float> stats = new Dictionary<string, float>();

    public override void InitStat(GameSO gameData)
    {
        if (gameData is PlayerSO playerData)
        {
            stats["MaxHealth"] = playerData.maxHealth;
            stats["CurrentHealth"] = playerData.maxHealth;
            stats["CurrentAttackPower"] = playerData.attackPower;
            stats["CurrentDefense"] = playerData.defense;
            stats["AttackSpeed"] = playerData.attackSpeed;
            stats["MoveSpeed"] = playerData.moveSpeed;
            stats["JumpForce"] = playerData.jumpForce;
            stats["DashForce"] = playerData.dashForce;

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
        if (stats.ContainsKey(statKey))
        {
            stats[statKey] = currentValue;
            //Debug.Log("먹고난 후 : " + stats[statKey]);
            Debug.Log($"{statKey} : {currentValue}");
            OnStatUpdated?.Invoke(statKey, currentValue);
            if (statKey == "CurrentHealth" && stats["CurrentHealth"] == 0)
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
        if (stats.ContainsKey("CurrentHealth"))
        {
            float newValue = Mathf.Clamp(currentHealth, 0f, stats["MaxHealth"]);
            UpdateStat("CurrentHealth", newValue);
        }
    }
}


// TODO: DataManager에 데이터 이동 기능 추가, stage 전환 시 PlayerStatData를 저장하고 
//       Static Class SaveManager를 활용하여 데이터를 JSON으로 저장하는 방식 구현
[System.Serializable]
public class PlayerStatData
{
    public float MaxHealth;
    public float CurrentHealth;
    public float CurrentDamage;
    public float CurrentDefense;
    public float AttackSpeed;
    public float MoveSpeed;
    public float DashForce;
}