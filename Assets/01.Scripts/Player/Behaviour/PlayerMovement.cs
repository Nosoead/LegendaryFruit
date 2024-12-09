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
    private Vector2 velocity = Vector2.zero;
    private Vector2 dash = Vector2.right;
    private Vector2 jump = Vector2.zero;
    private float moveSpeed;
    private float jumpForce;
    private float dashForce;
    private float direction;
    private bool isDash;

    private void Awake()
    {
        EnsureComponents();
    }

    private void OnEnable()
    {
        controller.OnMoveEvent += OnMovement;
        controller.OnSubCommandEvent += OnSubCommandEvent;
        controller.OnDashEvent += OnDash;
        controller.OnJumpEvent += OnJump;
    }

    private void OnDisable()
    {
        controller.OnMoveEvent -= OnMovement;
        controller.OnSubCommandEvent -= OnSubCommandEvent;
        controller.OnDashEvent -= OnDash;
        controller.OnJumpEvent -= OnJump;
        statManager.UnsubscribeToUpdateEvent(moveStats);
    }

    private void Start()
    {
        statManager.SubscribeToStatUpdates(moveStats);
    }

    private void FixedUpdate()
    {
        velocity.y = playerRigidbody.velocity.y;
        //ApplyMovement(velocity);
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
    private void OnMovement(float moveValue)
    {
        direction = moveValue;
        velocity.x = direction * moveSpeed;
        velocity = new Vector2(velocity.x, velocity.y);
        ApplyMovement(velocity);
    }

    private void OnSubCommandEvent()
    {

    }

    private void OnDash()
    {
        dash = Vector2.right * dashForce;
        ApplyDash(dash);
    }

    private void OnJump()
    {
        jump = Vector2.up * jumpForce;
        ApplyJump(jump);
    }
    #endregion

    #region /Apply Method
    private void ApplyMovement(Vector2 velocity)
    {
        playerRigidbody.velocity = velocity;
    }

    private void ApplyDash(Vector2 dash)
    {
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.AddForce(dash, ForceMode2D.Impulse);
    }

    private void ApplyJump(Vector2 jump)
    {
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.AddForce(jump, ForceMode2D.Impulse);
    }
    #endregion
}
