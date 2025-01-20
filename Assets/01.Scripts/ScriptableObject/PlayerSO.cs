using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObject/PlayerSO", order = 0)]
public class PlayerSO : GameSO
{
    public Dictionary<string, float> playerStats = new Dictionary<string, float>
    {
        { "MaxHealth", 0f },
        { "CurrentHealth", 0f },
        { "AttackPower", 0f },
        { "Defense", 0f },
        { "AttackSpeed", 0f },
        { "MoveSpeed", 0f },
        { "DashDistance", 0f },
        { "JumpHeight", 0f }
    };

    public PlayerStatData playerStatData = new PlayerStatData();

    public SyncStat sync = new SyncStat();
}

[System.Serializable]
public class PlayerStatData
{
    public float maxHealth;
    public float currentHealth;
    public float currentAttackPower;
    public float currentDefense;
    public float attackSpeed;
    public float moveSpeed;
    public float dashDistance;
    public float jumpHeight;
}

public class SyncStat
{
    public void SyncToDictionary(Dictionary<string, float> dictionary, PlayerStatData statData)
    {
        dictionary["MaxHealth"] = statData.maxHealth;
        dictionary["CurrentHealth"] = statData.currentHealth;
        dictionary["AttackPower"] = statData.currentAttackPower;
        dictionary["CurrentDefense"] = statData.currentDefense;
        dictionary["AttackSpeed"] = statData.attackSpeed;
        dictionary["MoveSpeed"] = statData.moveSpeed;
        dictionary["DashDistance"] = statData.dashDistance;
        dictionary["JumpHeight"] = statData.jumpHeight;
    }

    public void SyncToPlayerStatData(Dictionary<string, float> dictonary, PlayerStatData statData)
    {
        statData.maxHealth = dictonary["MaxHealth"];
        statData.currentHealth = dictonary["CurrentHealth"];
        statData.currentAttackPower = dictonary["AttackPower"];
        statData.currentDefense = dictonary["CurrentDefense"];
        statData.attackSpeed = dictonary["AttackSpeed"];
        statData.moveSpeed = dictonary["MoveSpeed"];
        statData.dashDistance = dictonary["DashDistance"];
        statData.jumpHeight = dictonary["JumpHeight"];
    }
}