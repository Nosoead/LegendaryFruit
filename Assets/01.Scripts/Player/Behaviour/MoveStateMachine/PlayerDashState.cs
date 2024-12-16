using System.Collections;
using UnityEngine;

public class PlayerDashState : IState
{

    private PlayerMovementHandler player;
    private WaitForSeconds dashTime = new WaitForSeconds(0.1f);
    private WaitForSeconds dashTerm = new WaitForSeconds(0.3f);
    private WaitForSeconds dashCoolDownTime = new WaitForSeconds(1.0f);
    private Coroutine coDashRoutine;

    public PlayerDashState(PlayerMovementHandler player)
    {
        this.player = player;
    }

    public void Enter()
    {
        //TODO : 애니메이션
        Debug.Log("Enter Dash State");

        player.SetVelocity(Vector2.zero);
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
    }

    public void Exit()
    {
        player.SetIsDashKeyPressed(false);
    }

    private void ApplyDash()
    {
        if (coDashRoutine != null)
        {
            player.SetStopCoroutine(coDashRoutine);
        }
        coDashRoutine = player.SetStartCoroutine(coDash());
    }

    private IEnumerator coDash()
    {
        if (player.CurrentDashCount == 2)
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

            yield return dashTerm;

            if (player.CurrentDashCount == 1)
            {
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
        }
        else
        {
            yield return dashCoolDownTime;
            player.ResetDashCount();
        }
    }
}
