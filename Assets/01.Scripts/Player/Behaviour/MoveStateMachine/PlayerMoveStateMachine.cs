using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStateMachine
{
    public IState CurrentState { get; private set; }

    public PlayerMoveState moveState;
    public PlayerDashState dashState;
    public PlayerJumpState jumpState;

    public PlayerMoveStateMachine(PlayerMovement player)
    {
        this.moveState = new PlayerMoveState(player);
        this.dashState = new PlayerDashState(player);
        this.jumpState = new PlayerJumpState(player);
    }
}
