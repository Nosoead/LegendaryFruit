using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    public PlayerMovementStateMachine StateMachine { get; private set; }
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerStatManager statManager;
    [SerializeField] private PlayerAttack attack;
    [SerializeField] private PlayerGround ground;
    [SerializeField, Range(0.2f, 1.25f)] private float timeToJumpApex = 0.25f;
    [SerializeField, Range(1f, 10f)] private float upwardMovementMultiplier = 1f;
    [SerializeField, Range(1f, 10f)] private float downwardMovementMultiplier = 4f;
    private float fallSpeedLimit = 20f;
    private float defaultGravityScale = 1;
    public float gravMultiplier;
    public float LookDirection { get; private set; }
    public float MoveDirection { get; private set; }
    public float MoveSpeed { get; private set; }
    public float DashDistance { get; private set; }
    public float JumpHeight { get; private set; }

    public int MaxDashCount { get; private set; } = 2;
    public int MaxJumpCount { get; private set; } = 2;
    public int CurrentDashCount { get; private set; }
    public int CurrentJumpCount { get; private set; }

    public bool IsDashing { get; private set; } = false;
    public bool CanDash { get; private set; } = true;
    public bool CanJump { get; private set; } = true;

    public bool IsMoveKeyPressed { get; private set; } = false;
    public bool IsDashKeyPressed { get; private set; } = false;
    public bool IsJumpKeyPressed { get; private set; } = false;

    public bool IsGround { get; private set; }
    public bool IsAttacking = false; //{ get; private set; }


    #region /Unity life Cycle
    private void Awake()
    {
        EnsureComponents();
        StateMachine = new PlayerMovementStateMachine(this);
        ResetDashCount();
        ResetJumpCount();
    }

    private void OnEnable()
    {
        statManager.OnSubscribeToStatUpdateEvent += OnStatUpdatedEvent;
        controller.OnDirectionEvent += OnDirectionEvent;
        controller.OnMoveEvent += OnMovement;
        controller.OnSubCommandEvent += OnSubCommandEvent;
        controller.OnDashEvent += OnDashEvent;
        controller.OnJumpEvent += OnJumpEvent;
    }

    private void OnDisable()
    {
        statManager.OnSubscribeToStatUpdateEvent -= OnStatUpdatedEvent;
        controller.OnDirectionEvent -= OnDirectionEvent;
        controller.OnMoveEvent -= OnMovement;
        controller.OnSubCommandEvent -= OnSubCommandEvent;
        controller.OnDashEvent -= OnDashEvent;
        controller.OnJumpEvent -= OnJumpEvent;
    }

    private void Start()
    {
        StateMachine.Initialize(StateMachine.idleState);
    }

    private void Update()
    {
        if (IsDashing)
        {
            Debug.Log(IsDashing);
            return;
        }
        SetPhysics();
        CheckGround();
    }

    private void FixedUpdate()
    {
        if (IsDashing)
        {
            Debug.Log(IsDashing);
            return;
        }
        StateMachine.Execute();
        CalculateGravity();
    }
    #endregion

    private void EnsureComponents()
    {
        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
        }
        if (playerCollider == null)
        {
            playerCollider = GetComponent<BoxCollider2D>();
        }
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }
        if (statManager == null)
        {
            statManager = GetComponent<PlayerStatManager>();
        }
        if (attack == null)
        {
            attack = GetComponent<PlayerAttack>();
        }
        if (ground == null)
        {
            ground = GetComponent<PlayerGround>();
        }
    }

    private void SetPhysics()
    {
        Vector2 newGravity = new Vector2(0, (-2 * JumpHeight) / (timeToJumpApex * timeToJumpApex));
        defaultGravityScale = (newGravity.y / Physics2D.gravity.y);
        playerRigidbody.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
    }

    private void CalculateGravity()
    {
        if (IsGround)
        {
            gravMultiplier = defaultGravityScale;
        }
        else if (!IsGround && playerRigidbody.velocity.y > 0.01f)
        {
            gravMultiplier = upwardMovementMultiplier;
        }
        else if (!IsGround && playerRigidbody.velocity.y < -0.01f)
        {
            gravMultiplier = downwardMovementMultiplier;
        }
        float velocityX = playerRigidbody.velocity.x;
        float velocityY = playerRigidbody.velocity.y;
        playerRigidbody.velocity = new Vector2(velocityX, Mathf.Clamp(velocityY, -fallSpeedLimit, 50));
    }

    public Coroutine SetStartCoroutine(IEnumerator enumerator)
    {
        return StartCoroutine(enumerator);
    }

    public void SetStopCoroutine(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }

    #region /subscribeMethod
    private void OnStatUpdatedEvent(string statKey, float value)
    {
        switch (statKey)
        {
            case "MoveSpeed":
                MoveSpeed = value;
                break;
            case "DashDistance":
                DashDistance = value;
                break;
            case "JumpHeight":
                JumpHeight = value;
                break;
        }
    }
    private void OnDirectionEvent(float directionValue)
    {
        this.LookDirection = directionValue;
    }
    private void OnMovement(float moveValue, bool isMoveKeyPressed)
    {
        this.MoveDirection = moveValue;
        this.IsMoveKeyPressed = isMoveKeyPressed;
    }
    private void OnSubCommandEvent()
    {

    }
    private void OnDashEvent(bool isDashKeyPressed)
    {
        this.IsDashKeyPressed = isDashKeyPressed;
        //ApplyDash();
    }
    private void OnJumpEvent(bool isJumpKeyPressed)
    {
        this.IsJumpKeyPressed = isJumpKeyPressed;
    }
    #endregion

    #region /rigidbodyMethod
    public void SetVelocity(Vector2 velocity)
    {
        playerRigidbody.velocity = velocity;
    }

    public Vector2 GetVelocity()
    {
        return playerRigidbody.velocity;
    }

    public void AddForce(Vector2 force, ForceMode2D mode)
    {
        playerRigidbody.AddForce(force, mode);
    }

    public void SetGravityScale(float gravityScale)
    {
        playerRigidbody.gravityScale = gravityScale;
    }

    public float GetGravityScale()
    {
        return defaultGravityScale;
    }
    #endregion

    #region /countingMethod
    public void DecreaseDashCount()
    {
        CurrentDashCount = Mathf.Max(0, CurrentDashCount - 1);
        if (CurrentDashCount == 0)
        {
            CanDash = false;
        }
    }

    public void ResetDashCount()
    {
        CurrentDashCount = MaxDashCount;
    }

    public void DecreaseJumpCount()
    {
        CurrentJumpCount = Mathf.Max(0, CurrentJumpCount - 1);
        if (CurrentJumpCount == 0)
        {
            CanJump = false;
        }
    }

    private void ResetJumpCount()
    {
        CurrentJumpCount = MaxJumpCount;
    }
    #endregion

    #region /boolMethod
    public void SetIsDashing(bool isDashing)
    {
        this.IsDashing = isDashing;
    }
    public void SetIsDashKeyPressed(bool isPressed)
    {
        IsDashKeyPressed = isPressed;
    }

    public void SetCanDash(bool canDash)
    {
        this.CanDash = canDash;
    }

    public void SetIsJumpKeyPressed(bool isPressed)
    {
        IsJumpKeyPressed = isPressed;
    }

    public void SetCanJump(bool canJump)
    {
        this.CanJump = canJump;
    }

    #region /DashMethod
    private void ApplyDash(Vector2 dash)
    {
        if (IsDashing)
        {
            return;
        }
        if (CanDash)
        {
            //StartCoroutine(coDash());
        }
    }

    //private IEnumerator coDash()
    //{
    //    CanDash = false;
    //    IsDashing = true;
    //    float originalGravity = playerRigidbody.gravityScale;
    //    playerRigidbody.gravityScale = 0f;
    //    playerRigidbody.velocity = new Vector2(transform.localScale.x * LookDirection * DashDistance, 0f);
    //    yield return dashingTime;
    //    playerRigidbody.gravityScale = originalGravity;
    //    playerRigidbody.velocity = Vector2.zero;
    //    playerRigidbody.AddForce(dash, ForceMode2D.Impulse);
    //    IsDashing = false;
    //    yield return dashingCooldown;
    //    CanDash = true;
    //}
    #endregion
    private void CheckGround()
    {
        IsGround = ground.GetOnGround();
        if (!(playerRigidbody.velocity.y > 0.01f) && IsGround)
        {
            CanJump = true;
            ResetJumpCount();
        }
    }
    #endregion
}
