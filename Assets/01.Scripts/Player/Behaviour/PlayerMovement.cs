using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Move,Jump,Dash
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D PlayerRigidbody;
    [SerializeField] private BoxCollider2D PlayerCollider;
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerStat stat;

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
        ApplyMovement();
    }

    #region /Event Method
    private void OnMovement(float moveValue)
    {
        
    }

    private void OnSubCommandEvent()
    {
        
    }

    private void OnDash()
    {
        
    }

    private void OnJump()
    {
        
    }
    #endregion

    #region /Apply Method
    private void ApplyMovement()
    {
        
    }
    #endregion
}
