using System.Collections;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    //Component & Script Component
    public PlayerMovementStateMachine StateMachine { get; private set; }
    
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerStatManager statManager;
    [SerializeField] private PlayerAttack attack;
    [SerializeField] private PlayerGround ground;

    //Constant Values
    [SerializeField, Range(0.2f, 1.25f)] private float timeToJumpApex = 0.25f;
    [SerializeField, Range(1f, 10f)] private float upwardMovementMultiplier = 1f;
    [SerializeField, Range(1f, 10f)] private float downwardMovementMultiplier = 4f;
    private float fallSpeedLimit = 20f;
    private float defaultGravityScale = 1;
    public float gravMultiplier;
    private int playerLayer;
    private int invincibleLayer;
    private Collider2D onewayBlockCollider = null;


    //Movement Values
    public float LookDirection { get; private set; } = 1;
    public float MoveDirection { get; private set; }
    public float MoveSpeed { get; private set; }
    public float DashDistance { get; private set; }
    public float JumpHeight { get; private set; }

    //Counter
    public int MaxDashCount { get; private set; } = 2;
    public int MaxJumpCount { get; private set; } = 2;
    public int CurrentDashCount { get; private set; }
    public int CurrentJumpCount { get; private set; }
    private WaitForSeconds colliderIgnoreDuration = new WaitForSeconds(0.25f);

    public bool IsDashing { get; private set; } = false;
    public bool CanDash { get; private set; } = true;
    public bool CanJump { get; private set; } = true;

    //Single Key input Check
    public bool IsMoveKeyPressed { get; private set; } = false;
    public bool IsDashKeyPressed { get; private set; } = false;
    public bool IsJumpKeyPressed { get; private set; } = false;
    public bool IsDownKeyPressed { get; private set; } = false;

    //Combined Key Input Check
    public bool IsDownwardJump { get; private set; } = false;

    //State check
    public bool IsGround { get; private set; }
    public bool IsOnewayBlock { get; private set; } = false;
    public bool IsAttacking { get; private set; } = false;

    //Particle Effect
    [SerializeField] private ParticleEffect particle;

    #region /Unity life Cycle
    private void Awake()
    {
        EnsureComponents();
        StateMachine = new PlayerMovementStateMachine(this);
        playerLayer = LayerMask.NameToLayer("Player");
        invincibleLayer = LayerMask.NameToLayer("Invincible");
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
        attack.OnAttackingEvent += OnAttackingEvent;
    }

    private void OnDisable()
    {
        statManager.OnSubscribeToStatUpdateEvent -= OnStatUpdatedEvent;
        controller.OnDirectionEvent -= OnDirectionEvent;
        controller.OnMoveEvent -= OnMovement;
        controller.OnSubCommandEvent -= OnSubCommandEvent;
        controller.OnDashEvent -= OnDashEvent;
        controller.OnJumpEvent -= OnJumpEvent;
        attack.OnAttackingEvent -= OnAttackingEvent;
    }

    private void Start()
    {
        StateMachine.Initialize(StateMachine.idleState);
    }

    private void Update()
    {
        if (IsDashing)
        {
            return;
        }
        SetPhysics();
    }

    private void FixedUpdate()
    {
        StateMachine.Execute();
        CheckGround();
        CheckOnewayBlock();
        if (!IsDashing)
        {
            CalculateGravity();
        }
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
        if (particle == null)
        {
            particle = GetComponentInChildren<ParticleEffect>();
        }
    }

    #region /subscribeMethod
    private void OnStatUpdatedEvent(string statKey, float value)    {
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
        LookDirection = directionValue;
    }

    private void OnMovement(float moveValue, bool isMoveKeyPressed)
    {
        MoveDirection = moveValue;
        IsMoveKeyPressed = isMoveKeyPressed;
    }

    private void OnSubCommandEvent(bool isDownPressed, bool isUpPressed)
    {
        IsDownKeyPressed = isDownPressed;
    }

    private void OnDashEvent(bool isDashKeyPressed)
    {
        IsDashKeyPressed = isDashKeyPressed;
    }

    private void OnJumpEvent(bool isJumpKeyPressed)
    {
        if (IsOnewayBlock && IsDownKeyPressed)
        {
            IsOnewayBlock = false;
            IsDownwardJump = isJumpKeyPressed;
            DownwardJump();
            return;
        }
        //IsDownKeyPressed = false;
        IsJumpKeyPressed = isJumpKeyPressed;
    }

    private void OnAttackingEvent(bool isAttacking)
    {
        IsAttacking = isAttacking;
    }
    #endregion

    #region /Calculate Physics
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

    private void CheckGround()
    {
        IsGround = ground.GetOnGround();
        if (!(playerRigidbody.velocity.y > 0.01f) && IsGround)
        {
            CanJump = true;
            ResetJumpCount();
        }
    }

    private void CheckOnewayBlock()
    {
        IsOnewayBlock = ground.GetOnewayBlock();
            onewayBlockCollider = ground.GetOnewayCollider();
        if (onewayBlockCollider == null)
        {
        }
    }

    private void DownwardJump()
    {
        if (IsDownwardJump)
        {
            IsDownwardJump = false;
            StartCoroutine(DownwardJumpRoutine());
        }
    }

    private IEnumerator DownwardJumpRoutine()
    {

        Physics2D.IgnoreCollision(playerCollider, onewayBlockCollider, true);
        Debug.Log(onewayBlockCollider.gameObject.ToString());
        yield return colliderIgnoreDuration;
        Physics2D.IgnoreCollision(playerCollider, onewayBlockCollider, false);
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

    #region /FieldSetMethod
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

    public void SetLayerToDefault()
    {
        if (gameObject.layer != invincibleLayer)
        {
            gameObject.layer = invincibleLayer;
        }
    }

    public void SetLayerToPlayer()
    {
        if (gameObject.layer != playerLayer)
        {
            gameObject.layer = playerLayer;
        }
    }
    #endregion

    #region /Coroutine Rental Method
    public Coroutine SetStartCoroutine(IEnumerator enumerator)
    {
        return StartCoroutine(enumerator);
    }

    public void SetStopCoroutine(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
        SetGravityScale(GetGravityScale());
        SetVelocity(Vector2.zero);
        SetCanDash(true);
        SetIsDashing(false);
    }
    #endregion

    #region /Particle
    public void PlayDustParticle()
    {
        particle.SetDirection(LookDirection);
        particle.PlayEffect();
    }
    #endregion

    public void test()
    {
        Debug.Log(onewayBlockCollider.gameObject.ToString());
    }
}
