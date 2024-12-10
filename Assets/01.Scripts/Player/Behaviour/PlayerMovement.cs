using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//Move,Jump,Dash
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerStatManager statManager;
    [SerializeField] private PlayerGround playerGround;
    private Vector2 velocity = Vector2.zero;
    private float moveDirection;
    private float moveSpeed;

    [Header("DashInfo")]
    private Vector2 dash = Vector2.right;
    private float lookDirection = 1f;
    private float dashForce;
    private bool isDashing;
    private bool canDash = true;
    private WaitForSeconds dashingTime = new WaitForSeconds(0.2f);
    private WaitForSeconds dashingCooldown = new WaitForSeconds(1f);

    [Header("JumpInfo")]
    private Vector2 jump = Vector2.zero;
    private float jumpForce;
    private float direction;
    private int jumpCounter;
    private bool isGround;

    private void Awake()
    {
        EnsureComponents();
    }

    private void OnEnable()
    {
        controller.OnDirectionEvent += OnDirectionEvent;
        controller.OnMoveEvent += OnMovement;
        controller.OnSubCommandEvent += OnSubCommandEvent;
        controller.OnDashEvent += OnDashEvent;
        controller.OnJumpEvent += OnJumpEvent;
    }

    private void OnDisable()
    {
        controller.OnDirectionEvent -= OnDirectionEvent;
        controller.OnMoveEvent -= OnMovement;
        controller.OnSubCommandEvent -= OnSubCommandEvent;
        controller.OnDashEvent -= OnDashEvent;
        controller.OnJumpEvent -= OnJumpEvent;
        statManager.UnsubscribeToStatUpdateEvent(moveStats);
    }

    private void Start()
    {
        statManager.SubscribeToStatUpdateEvent(moveStats);
    }

    private void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        velocity.y = playerRigidbody.velocity.y;
        //ApplyMovement(velocity);
        if (isDashing)
        {
            return;
        }
        ApplyMovement();
    }

    #region /Ensure Components
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
        if (playerGround == null)
        {
            playerGround = GetComponent<PlayerGround>();
        }
    }
    #endregion

    private void moveStats(string statKey, float value)
    {
        switch (statKey)
        {
            case "MoveSpeed":
                moveSpeed = value;
                break;
            case "JumpForce":
                jumpForce = value;
                break;
            case "DashForce":
                dashForce = value;
                break;
        }
    }

    #region /Event Method
    private void OnDirectionEvent(float directionValue)
    {
        lookDirection = directionValue;
    }

    private void OnMovement(float moveValue)
    {
        direction = moveValue;
        velocity.x = direction * moveSpeed;
        velocity = new Vector2(velocity.x, velocity.y);
        ApplyMovement();
    }

    private void OnSubCommandEvent()
    {

    }

    private void OnDashEvent()
    {
        dash = Vector2.right * lookDirection * dashForce;
        ApplyDash(dash);
    }

    private void OnJumpEvent()
    {
        jump = Vector2.up * jumpForce;
        if (isGround)
        {
            ExitCoroutine();
            ApplyJump(jump);
        }
        else if (!isGround && jumpCounter == 1)
        {
            Debug.Log(jumpCounter);
            ExitCoroutine();
            ApplyJump(jump);
            jumpCounter--;
        }
    }
    #endregion

    #region /Apply Method
    private void ApplyMovement()
    {
        if (isDashing)
        {
            return;
        }
        playerRigidbody.velocity = velocity;
    }

    private void ApplyDash(Vector2 dash)
    {
        if (isDashing)
        {
            return;
        }
        if (canDash)
        {
            StartCoroutine(coDash());
        }
    }

    private IEnumerator coDash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = playerRigidbody.gravityScale;
        playerRigidbody.gravityScale = 0f;
        playerRigidbody.velocity = new Vector2(transform.localScale.x * lookDirection * dashForce, 0f);
        yield return dashingTime;
        playerRigidbody.gravityScale = originalGravity;
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.AddForce(dash, ForceMode2D.Impulse);
        isDashing = false;
        yield return dashingCooldown;
        canDash = true;
    }

    private void ApplyJump(Vector2 jump)
    {
        if (isDashing)
        {
            playerRigidbody.velocity = Vector2.zero;
        }
        playerRigidbody.AddForce(jump, ForceMode2D.Impulse);
    }
    #endregion

    private void ExitCoroutine()
    {
        StopAllCoroutines();
        playerRigidbody.gravityScale = 1f;
        isDashing = false;
        canDash = true;
    }

    private void CheckGround()
    {
        isGround = playerGround.GetOnGround();
        if (isGround)
        {
            jumpCounter = 1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (lookDirection > 0)
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 1f);
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position - Vector3.right * 1f);
        }
    }
}
