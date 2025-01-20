
using System.Diagnostics;

public class PlayerIdleState : IState
{
    private PlayerMovementHandler player;

    public PlayerIdleState(PlayerMovementHandler player)
    {
        this.player = player;
    }

    public void Enter()
    {
        
    }

    public void Execute()
    {
        if (player.IsGround)
        {
            if (player.IsMoveKeyPressed && !player.IsAttacking)
            {
                player.StateMachine.TransitionTo(player.StateMachine.moveState);
                return;
            }
            if (player.IsDashKeyPressed && player.CanDash)
            {
                player.StateMachine.TransitionTo(player.StateMachine.dashState);
                return;
            }
            if (player.IsJumpKeyPressed && player.CanJump)
            {
                player.StateMachine.TransitionTo(player.StateMachine.airborneState);
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