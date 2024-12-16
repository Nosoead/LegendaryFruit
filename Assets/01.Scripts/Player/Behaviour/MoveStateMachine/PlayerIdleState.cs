using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerIdleState : IState
{
    private PlayerMovementHandler player;

    public PlayerIdleState(PlayerMovementHandler player)
    {
        this.player = player;
    }

    public void Enter()
    {
        //TODO : 애니메이션
        Debug.Log("Enter Idle State");
    }

    public void Execute()
    {
        if (player.IsGround)
        {
            if (player.IsMoveKeyPressed)
            {
                player.StateMachine.TransitionTo(player.StateMachine.moveState);//TODO : 이동
                return;
            }
            if (player.IsDashKeyPressed && player.CanDash)
            {
                player.StateMachine.TransitionTo(player.StateMachine.dashState);//TODO : 대쉬
                return;
            }
            if (player.IsJumpKeyPressed && player.CanJump)
            {
                player.StateMachine.TransitionTo(player.StateMachine.airborneState);//TODO : 점프
                return;
            }
        }
        else
        {
            player.StateMachine.TransitionTo(player.StateMachine.airborneState);
        }
    }

    public void Exit()
    {

    }
}