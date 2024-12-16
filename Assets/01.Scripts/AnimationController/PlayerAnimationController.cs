using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int isRun = Animator.StringToHash("isRun");
    private static readonly int isJump = Animator.StringToHash("isJump");
    private static readonly int isDash = Animator.StringToHash("isDash");
    private static readonly int isIdle = Animator.StringToHash("isIdle");
    private static readonly int isGround = Animator.StringToHash("isGround");
    private static readonly int isDie = Animator.StringToHash("isDie");
    private static readonly int isHit = Animator.StringToHash("isHit");
    private PlayerController controller;
    private PlayerGround playerGround;
    private PlayerMovement playerMovement;

    private bool playerIsGround;

    protected override void Awake()
    {
        base.Awake();
        Animator.SetBool(isIdle, true);
    }

    private void Update()
    {
        playerIsGround = playerGround.GetOnGround();
        if(playerIsGround)
        {
            Animator.SetTrigger(isGround);
        }
    }

    protected override void EnsureComponents()
    {
        base.EnsureComponents();
        if(controller == null)
        {
            controller = GetComponent<PlayerController>();
        }
        if (playerGround == null)
        {
            playerGround = GetComponent<PlayerGround>();
        }
        if(playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }
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
        controller.OnDashEvent -= OnDashEvent;
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

    private void OnMonveEvent(float value)
    {
        switch (value)
        {
            case 1f:
                Animator.SetBool("jumpINrun", true);
                Animator.SetBool(isIdle, false);
                Animator.SetBool(isRun, true);
                break;
            case -1f:
                Animator.SetBool("jumpINrun", true);
                Animator.SetBool(isIdle, false);
                Animator.SetBool(isRun, true);
                break;
            case 0:
                Animator.SetBool("jumpINrun", false);
                Animator.SetBool(isIdle, true);
                Animator.SetBool(isRun, false);
                break;
        }
    }
    private void OnJumpEvent()
    {
        Animator.SetTrigger(isJump);
    }

    private void OnDashEvent()
    {
        Animator.SetTrigger(isDash);
    }

    public void OnHit()
    {
        Animator.SetTrigger(isHit);
    }
    
    public void OnDie()
    {
        Animator.SetTrigger(isDie);
    }
}
