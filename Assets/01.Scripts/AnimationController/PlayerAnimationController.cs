using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    private PlayerController controller;

    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int isRun = Animator.StringToHash("isRun");
    private static readonly int isJump = Animator.StringToHash("isJump");
    private static readonly int isDash = Animator.StringToHash("isDash");
    private static readonly int isdle = Animator.StringToHash("isdle");

    protected override void Awake()
    {
        base.Awake();
        Animator.SetBool(isdle, true);
        controller = GetComponent<PlayerController>(); 
    }

    private void OnEnable()
    {
        controller.OnDirectionEvent += OnDirectionEvent;
        controller.OnMoveEvent += OnMonveEvent;
        controller.OnJumpEvent += OnJumpEvent;
        controller.OnDashEvent += OnDashEvent;
    }

    private void OnDisable()
    {
        controller.OnDirectionEvent -= OnDirectionEvent;
        controller.OnMoveEvent -= OnMonveEvent;
        controller.OnJumpEvent -= OnJumpEvent;
    }


    private void OnDirectionEvent(float directionValue)
    {
        switch (directionValue)
        {
            case -1f:
                Sprite.flipX = true;
                break;

            case 1f:
                Sprite.flipX = false;
                break;

        }
    }

    private void OnMonveEvent(float value, bool isbool)
    {
        switch (value)
        {
            case 1f:
                Animator.SetBool("isIdle", false);
                Animator.SetBool(isRun, true);
                break;
            case -1f:
                Animator.SetBool("isIdle", false);
                Animator.SetBool(isRun, true);
                break;
            case 0:
                Animator.SetBool("isIdle", true);
                Animator.SetBool(isRun, false);
                break;
        }
    }

    private void OnJumpEvent(bool ispressed)
    {
        Animator.SetTrigger(isJump);
    }

    private void OnDashEvent(bool ispressed)
    {
        Animator.SetTrigger(isDash);
    }
}
