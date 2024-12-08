using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Todo 장비장착시 스탯 추가 시킬 것. 영구 증가X 무기 장작관련 필드 만들어서 저장
// -> UI에서는 합산한 데이터 송출해 줄것 (DataManager에서 해결)

//TODO Refactoring : 필드 public someField {get; private set;} 캡슐화
//                      public Method만들어서 데이터 갱신
//                      갱신 데이터의 float[]로 묶는 방향성 고민중
public class PlayerStat : Stat
{
    //TODO Controller로  PlayerStat UPdate마다 전송
    public UnityAction<string, float> OnStatUpdated;
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

    public void UpdateStat(string statKey, float currentValue)
    {
        if (stats.TryGetValue(statKey, out var lastValue))
        {
            stats[statKey] = currentValue;
            OnStatUpdated?.Invoke(statKey, currentValue);
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


//TODO : DataManager로 런타임데이터 관리 후 stage 변화 시 PlayerStatData에 저장 후 Static Class SaveManager에서 지역변수로 사용해서 값형으로 JSON으로 저장하는 영구저장 그릇 역할
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