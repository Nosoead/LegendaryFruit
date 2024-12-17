using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : IState
{

    private PlayerMovementHandler player;
    private WaitForSeconds dashTime = new WaitForSeconds(0.3f);
    private WaitForSeconds dashTerm = new WaitForSeconds(0.3f);
    private WaitForSeconds dashCoolDownTime = new WaitForSeconds(1.0f);
    private Coroutine coDashRoutine;

    public PlayerDashState(PlayerMovementHandler player)
    {
        this.player = player;
    }

    public void Enter()
    {
        ApplyDash();
    }

    public void Execute()
    {
        if (!player.IsDashing)
        {
            if (player.IsGround)
            {
                if (!player.IsMoveKeyPressed)
                {
                    player.StateMachine.TransitionTo(player.StateMachine.idleState);

                }
                else
                {
                    player.StateMachine.TransitionTo(player.StateMachine.moveState);
                }
                return;
            }

            if (!player.IsGround)
            {
                player.StateMachine.TransitionTo(player.StateMachine.airborneState);
                return;
            }
        }
        if (player.IsJumpKeyPressed && player.CanJump)
        {
            if (player.CurrentDashCount != 0)
            {
                player.SetStopCoroutine(coDashRoutine);
            }
            player.StateMachine.TransitionTo(player.StateMachine.airborneState);//TODO : 점프
            return;
        }
    }

    public void Exit()
    {
    }

    private void ApplyDash()
    {
        player.SetIsDashKeyPressed(false);
        Debug.Log(player.CurrentDashCount);
        if (player.CurrentDashCount != 0)
        {
            if (coDashRoutine != null)
            {
                player.SetStopCoroutine(coDashRoutine);
            }
            if (player.CanDash)
            {
                coDashRoutine = player.SetStartCoroutine(dashRoutine());
            }
        }
    }

    private IEnumerator dashRoutine()
    {
        if (player.CurrentDashCount == 2)
        {
            player.DecreaseDashCount();
            player.SetCanDash(false);
            player.SetIsDashing(true);
            player.SetGravityScale(0f);
            Vector2 dashVelocity = Vector2.right * player.LookDirection * player.DashDistance;
            player.SetVelocity(dashVelocity);
            Debug.Log($"Dash Velocity: {dashVelocity}");
            Debug.Log($"Applied Velocity: {player.GetVelocity()}");
            //player.AddForce(new Vector2(player.LookDirection * player.DashDistance, 0f), ForceMode2D.Impulse);
            yield return dashTime;
            player.SetGravityScale(player.GetGravityScale());
            player.SetVelocity(Vector2.zero);
            player.SetCanDash(true);
            player.SetIsDashing(false);

            yield return dashTerm;

            if (player.CurrentDashCount == 1)
            {
                yield return dashCoolDownTime;
                player.ResetDashCount();
            }
        }
        else if (player.CurrentDashCount == 1)
        {
            player.DecreaseDashCount();
            player.SetCanDash(false);
            player.SetIsDashing(true);
            player.SetGravityScale(0f);
            player.SetVelocity(new Vector2(player.LookDirection * player.DashDistance, 0f));
            yield return dashTime;
            player.SetGravityScale(player.GetGravityScale());
            player.SetVelocity(Vector2.zero);
            player.SetCanDash(true);
            player.SetIsDashing(false);
            yield return dashCoolDownTime;
            player.ResetDashCount();
        }
    }
}
