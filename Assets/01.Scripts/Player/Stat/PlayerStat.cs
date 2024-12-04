using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float CurrentDamage { get; set; }
    public float CurrentDefense { get; set; }
    public float AttackSpeed { get; set; }
    public float MoveSpeed { get; set; }

    public override void InitStat(GameSO gameData)
    {
        if (gameData is PlayerSO playerData)
        {
            MaxHealth = playerData.maxHealth;
            CurrentHealth = MaxHealth;
            CurrentDamage = playerData.attackPower;
            CurrentDefense = playerData.defense;
            AttackSpeed = playerData.attackSpeed;
            MoveSpeed = playerData.moveSpeed;
        }
        //Debug.Log($"MaxHealth : {MaxHealth}");
        //Debug.Log($"CurrentHealth : {CurrentHealth}");
        //Debug.Log($"CurrentDamage : {CurrentDamage}");
        //Debug.Log($"CurrentDefense : {CurrentDefense}");
        //Debug.Log($"AttackSpeed : {AttackSpeed}");
        //Debug.Log($"MoveSpeed : {MoveSpeed}");
    }
}
