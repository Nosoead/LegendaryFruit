using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Todo ��������� ���� �߰� ��ų ��. ���� ����X ���� ���۰��� �ʵ� ���� ����
// -> UI������ �ջ��� ������ ������ �ٰ� (DataManager���� �ذ�)

//TODO Refactoring : �ʵ� public someField {get; private set;} ĸ��ȭ
//                      public Method���� ������ ����
//                      ���� �������� float[]�� ���� ���⼺ �����
public class PlayerStat : Stat
{
    //TODO Controller��  PlayerStat UPdate���� ����
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


//TODO : DataManager�� ��Ÿ�ӵ����� ���� �� stage ��ȭ �� PlayerStatData�� ���� �� Static Class SaveManager���� ���������� ����ؼ� �������� JSON���� �����ϴ� �������� �׸� ����
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