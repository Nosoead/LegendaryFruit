using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObject/PlayerSO", order = 1)]
public class PlayerSO : GameSO
{
    public float maxHealth;
    public float attackPower;
    public float defense;
    public float attackSpeed;
    public float moveSpeed;
    public float jumpForce;
    public float dashForce;
}
