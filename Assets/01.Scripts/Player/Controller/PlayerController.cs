using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public UnityAction<float> OnDirectionEvent;
    public UnityAction<float, bool> OnMoveEvent;
    public UnityAction OnSubCommandEvent;
    public UnityAction<bool> OnDashEvent;
    public UnityAction<bool> OnJumpEvent;
    public UnityAction OnAttackEvent;
    public UnityAction OnSkill1Event;
    public UnityAction OnSkill2Event;
    public UnityAction OnSwapWeaponEvent;
    public UnityAction<bool> OnTapInteractEvent;
    public UnityAction<bool> OnHoldInteractEvent;
    public UnityAction OnUserInfoEvent;
    public UnityAction OnSettingWindowEvent;

    public PlayerInput Input;
    private bool isLeftPressed;
    private bool isRightPressed;
    private void Awake()
    {
        Input = new PlayerInput();
    }

    private void OnEnable()
    {
        //PlayerMovement
        Input.Player.Move.started += PlayerDirection;
        Input.Player.Move.started += PlayerMove;
        Input.Player.Move.performed += PlayerMove;
        Input.Player.Move.canceled += PlayerMove;

        //SubCommand ex) UpArrow + Attak, DownArrow + Jump ...
        Input.Player.SubCommand.started += PlayerSubCommand;

        //PlayerMovement + alpha
        Input.Player.Dash.started += PlayerDash;
        Input.Player.Dash.canceled += PlayerDash;
        Input.Player.Jump.started += PlayerJump;
        Input.Player.Jump.canceled += PlayerJump;

        //PlayerAttack
        Input.Player.Attack.started += PlayerAttack;
        //Input.Player.Attack.canceled += PlayerAttack;
        Input.Player.Skill1.performed += PlayerSkill1;
        Input.Player.Skill1.canceled += PlayerSkill1;
        Input.Player.Skill2.performed += PlayerSkill2;
        Input.Player.Skill2.canceled += PlayerSkill2;

        //PlayerEquip
        Input.Player.SwapWeapon.started += PlayerSwapWeapon;
        //Input.Player.SwapWeapon.canceled += PlayerSwapWeapon;

        //PlayerInteract -> 
        Input.Player.Interact.started += PlayerTapInteract;
        Input.Player.Interact.canceled += PlayerTapInteract;
        Input.Player.Interact.performed += PlayerHoldInteract;
        Input.Player.Interact.canceled += PlayerHoldInteract;

        //TODO UI -> GameManager refactoring
        Input.Player.UserInfo.started += PlayerUserInfo;
        //Input.Player.UserInfo.canceled += PlayerUserInfo;
        Input.Player.SettingWindow.started += PlayerSettingWindow;
        //Input.Player.SettingWindow.canceled += PlayerSettingWindow;
        Input.Player.Enable();
    }

    private void OnDisable()
    {
        Input.Player.Disable();
    }

    public void PlayerDirection(InputAction.CallbackContext context)
    {
        float directionValue = Mathf.Sign(context.ReadValue<float>());
        OnDirectionEvent?.Invoke(directionValue);
    }
    public void PlayerMove(InputAction.CallbackContext context)
    {
        float moveValue = context.ReadValue<float>();
        if (context.started)
        {
            if (moveValue < -0.01f)
            {
                isLeftPressed = true;
            }
            else if (moveValue > 0.01f)
            {
                isRightPressed = true;
            }
        }
        else if (context.canceled)
        {
            if (isLeftPressed && moveValue > -0.01f)
            {
                isLeftPressed = false;
            }
            else if (isRightPressed && moveValue < 0.01f)
            {
                isRightPressed = false;
            }
        }

        bool isMoving = isLeftPressed || isRightPressed;
        OnMoveEvent?.Invoke(moveValue, isMoving);
    }
    public void PlayerSubCommand(InputAction.CallbackContext context)
    {
        OnSubCommandEvent?.Invoke();
    }
    public void PlayerDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnDashEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            OnDashEvent?.Invoke(false);
        }
    }
    public void PlayerJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnJumpEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            OnJumpEvent?.Invoke(false);
        }
        
    }
    public void PlayerAttack(InputAction.CallbackContext context)
    {
        OnAttackEvent?.Invoke();
    }
    public void PlayerSkill1(InputAction.CallbackContext context)
    {
    }
    public void PlayerSkill2(InputAction.CallbackContext context)
    {
    }
    public void PlayerSwapWeapon(InputAction.CallbackContext context)
    {
        OnSwapWeaponEvent?.Invoke();
    }
    public void PlayerTapInteract(InputAction.CallbackContext context)
    {
        float result = context.ReadValue<float>();
        bool isTapInteract;
        if (result > 0.5)
        {
            isTapInteract = true;
        }
        else
        {
            isTapInteract = false;
        }
        OnTapInteractEvent?.Invoke(isTapInteract);
    }
    public void PlayerHoldInteract(InputAction.CallbackContext context)
    {
        float result = context.ReadValue<float>();
        bool isHoldInteract;
        if (result > 0.5)
        {
            isHoldInteract = true;
        }
        else
        {
            isHoldInteract = false;
        }
        OnHoldInteractEvent?.Invoke(isHoldInteract);
    }
    public void PlayerUserInfo(InputAction.CallbackContext context)
    {
        OnUserInfoEvent?.Invoke();
    }
    public void PlayerSettingWindow(InputAction.CallbackContext context)
    {
        OnSettingWindowEvent?.Invoke();
    }
}