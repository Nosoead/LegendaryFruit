using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//Move,Jump,Dash
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private PlayerController controller;
    private Vector2 velocity = Vector2.zero;
    private Vector2 dash = Vector2.right;
    private Vector2 jump = Vector2.zero;
    private float moveSpeed;
    private float jumpForce;
    private float dashForce;
    private float direction;
    private bool isDash;

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
    }

    private void FixedUpdate()
    {
        velocity.y = playerRigidbody.velocity.y;
        //ApplyMovement(velocity);
    }

    public void ApplyDynamicStats(PlayerStat stat)
    {
        moveSpeed = stat.MoveSpeed;
    }

    public void ApplyStaticStats(PlayerStat stat)
    {
        jumpForce = stat.JumpForce;
        dashForce = stat.DashForce;
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
        playerRigidbody.velocity= velocity;
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
