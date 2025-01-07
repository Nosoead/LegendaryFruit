using System.Collections;
using UnityEngine;

public class PlayerDashState : IState
{

    private PlayerMovementHandler player;
    private WaitForSeconds dashTime = new WaitForSeconds(0.3f);
    private WaitForSeconds dashTerm = new WaitForSeconds(0.3f);
    private WaitForSeconds dashCoolDownTime = new WaitForSeconds(0.5f);
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
                player.SetLayerToPlayer();
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
        if (player.CurrentDashCount != 0)
        {
            if (coDashRoutine != null)
            {
                player.SetStopCoroutine(coDashRoutine);
            }
            if (player.CanDash)
            {
                coDashRoutine = player.SetStartCoroutine(dashRoutine());
                player.PlayDustParticle();
            }
        }
    }

    private IEnumerator dashRoutine()
    {
        if (player.CurrentDashCount == 2)
        {
            EnterDashLogic();
            yield return dashTime;
            ExitDashLogic();

            yield return dashTerm;

            if (player.CurrentDashCount == 1)
            {
                yield return dashCoolDownTime;
                player.ResetDashCount();
            }
        }
        else if (player.CurrentDashCount == 1)
        {
            EnterDashLogic();
            yield return dashTime;
            ExitDashLogic();
            yield return dashCoolDownTime;
            player.ResetDashCount();
        }
    }

    private void EnterDashLogic()
    {
        player.SetLayerToDefault();
        player.DecreaseDashCount();
        player.SetCanDash(false);
        player.SetIsDashing(true);
        player.SetGravityScale(0f);
        Vector2 dashVelocity = Vector2.right * player.LookDirection * player.DashDistance;
        player.SetVelocity(dashVelocity);
        SoundManagers.Instance.PlaySFX(SfxType.PlayerDash);
    }
    private void ExitDashLogic()
    {
        player.SetLayerToPlayer();
        player.SetGravityScale(player.GetGravityScale());
        player.SetVelocity(Vector2.zero);
        player.SetCanDash(true);
        player.SetIsDashing(false);
    }
}
