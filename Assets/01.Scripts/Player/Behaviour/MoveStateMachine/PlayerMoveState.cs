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
        //TODO : 애니메이션
        Debug.Log("Enter Move State");
    }

    public void Execute()
    {
        ApplyMovement();

        if (player.IsGround)
        {
            if (!player.IsMoveKeyPressed || player.IsAttacking)
            {
                player.StateMachine.TransitionTo(player.StateMachine.idleState);//TODO : 정지,공격
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
            player.StateMachine.TransitionTo(player.StateMachine.airborneState);//TODO : 점프
            return;
        }
    }

    public void Exit()
    {
        
    }

    private void ApplyMovement()
    {
        Vector2 velocity = new Vector2(player.MoveSpeed * player.MoveDirection, player.GetVelocity().y);
        player.SetVelocity(velocity);
    }
}
