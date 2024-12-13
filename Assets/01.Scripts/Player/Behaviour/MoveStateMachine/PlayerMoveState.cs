using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : IState
{
    private PlayerMovement player;

    public PlayerMoveState(PlayerMovement player)
    {
        this.player = player;
        Debug.Log(player.moveSpeed);
    }
    public void Enter()
    {
        
    }

    public void Excute()
    {
        
    }

    public void Exit()
    {
        
    }
}
