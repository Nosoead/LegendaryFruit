using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo ��������� ���� �߰� ��ų ��. ���� ����X ���� ���۰��� �ʵ� ���� ����
// -> UI������ �ջ��� ������ ������ �ٰ� (DataManager���� �ذ�)

//TODO Refactoring : �ʵ� public someField {get; private set;} ĸ��ȭ
//                      public Method���� ������ ����
//                      ���� �������� float[]�� ���� ���⼺ �����
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
