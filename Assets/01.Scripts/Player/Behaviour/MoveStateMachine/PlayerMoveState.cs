using UnityEngine;

public class PlayerMoveState : IState
{
    private PlayerMovementHandler player;

    public PlayerMoveState(PlayerMovementHandler player)
    {
        this.player = player;
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        ApplyMovement();
        
        if (player.IsGround)
        {
            if (!player.IsMoveKeyPressed || player.IsAttacking)
            {
                IsAttacking();
                player.StateMachine.TransitionTo(player.StateMachine.idleState);
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
            return;
        }
        SoundManagers.Instance.PlaySFX(SfxType.PlayerMove,true);

    }

    public void Exit()
    {

    }

    private void ApplyMovement()
    {
        Vector2 velocity = new Vector2(player.MoveSpeed * player.MoveDirection, player.GetVelocity().y);
        player.SetVelocity(velocity);
    }

    private void IsAttacking()
    {
        if (player.IsAttacking)
        {
            player.SetVelocity(Vector2.zero);
        }
    }
}
