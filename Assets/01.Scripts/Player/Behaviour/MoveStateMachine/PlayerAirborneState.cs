using UnityEngine;

public class PlayerAirborneState : IState
{
    private PlayerMovementHandler player;
    private float jumpSpeed;
    private Vector2 currentVelocity;
    public PlayerAirborneState(PlayerMovementHandler player)
    {
        this.player = player;
    }

    public void Enter()
    {
        if (player.IsJumpKeyPressed == false)
        {
            player.DecreaseJumpCount();
        }
    }

    public void Execute()
    {
        ApplyAirMovement();
        currentVelocity = player.GetVelocity();
        ApplyJump();
        player.SetVelocity(currentVelocity);
        if (player.IsDashKeyPressed && player.CanDash)
        {
            player.StateMachine.TransitionTo(player.StateMachine.dashState);
            return;
        }
        if ((currentVelocity.y < 0.01f) && player.IsGround)
        {
            player.StateMachine.TransitionTo(player.StateMachine.idleState);
        }
    }

    public void Exit()
    {

    }
    private void ApplyJump()
    {
        if (player.IsJumpKeyPressed && player.CanJump)
        {
            player.DecreaseJumpCount();
            player.SetIsJumpKeyPressed(false);
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * player.GetGravityScale() * player.JumpHeight);
            if (currentVelocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - currentVelocity.y, 0f);
            }
            else if (currentVelocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(currentVelocity.y);
            }
            player.SetVelocity(Vector2.zero);
            currentVelocity.y += jumpSpeed;
        }
    }

    private void ApplyAirMovement()
    {
        Vector2 velocity = new Vector2(player.MoveSpeed * player.MoveDirection, player.GetVelocity().y);
        player.SetVelocity(velocity);
    }
}
