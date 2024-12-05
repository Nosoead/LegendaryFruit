using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo 장비장착시 스탯 추가 시킬 것. 영구 증가X 무기 장작관련 필드 만들어서 저장
// -> UI에서는 합산한 데이터 송출해 줄것 (DataManager에서 해결)

//TODO Refactoring : 필드 public someField {get; private set;} 캡슐화
//                      public Method만들어서 데이터 갱신
//                      갱신 데이터의 float[]로 묶는 방향성 고민중
public class PlayerStat : Stat
{
    //TODO JSON Save Data List
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float CurrentDamage { get; set; }
    public float CurrentDefense { get; set; }
    public float AttackSpeed { get; set; }
    public float MoveSpeed { get; set; }

    //Not JSON Save Data List
    //TODO Add AirControllFactor, FrictionFactor, DoubleJumpFactor ....
    public float JumpForce {  get; set; }
    public float DashForce { get; set; }

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
            JumpForce = playerData.jumpForce;
            DashForce = playerData.dashForce;
        }
        //Debug.Log($"MaxHealth : {MaxHealth}");
        //Debug.Log($"CurrentHealth : {CurrentHealth}");
        //Debug.Log($"CurrentDamage : {CurrentDamage}");
        //Debug.Log($"CurrentDefense : {CurrentDefense}");
        //Debug.Log($"AttackSpeed : {AttackSpeed}");
        //Debug.Log($"MoveSpeed : {MoveSpeed}");
    }
}
