using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public UnityAction<float> OnDirectionEvent;
    public UnityAction<float> OnMoveEvent;
    public UnityAction OnSubCommandEvent;
    public UnityAction OnDashEvent;
    public UnityAction OnJumpEvent;
    public UnityAction OnAttackEvent;
    public UnityAction OnSkill1Event;
    public UnityAction OnSkill2Event;
    public UnityAction OnSwapWeaponEvent;
    public UnityAction OnInteractEvent;
    public UnityAction OnUserInfoEvent;
    public UnityAction OnSettingWindowEvent;

    public PlayerInput Input;

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
        //Input.Player.Dash.canceled += PlayerDash;
        Input.Player.Jump.started += PlayerJump;
        //Input.Player.Jump.canceled += PlayerJump;

        //PlayerAttack
        Input.Player.Attack.performed += PlayerAttack;
        //Input.Player.Attack.canceled += PlayerAttack;
        Input.Player.Skill1.performed += PlayerSkill1;
        Input.Player.Skill1.canceled += PlayerSkill1;
        Input.Player.Skill2.performed += PlayerSkill2;
        Input.Player.Skill2.canceled += PlayerSkill2;

        //PlayerEquip
        Input.Player.SwapWeapon.performed += PlayerSwapWeapon;
        Input.Player.SwapWeapon.canceled += PlayerSwapWeapon;

        //PlayerInteract -> 
        Input.Player.Interact.performed += PlayerInteract;
        Input.Player.Interact.canceled += PlayerInteract;

        //TODO UI -> GameManager refactoring
        Input.Player.UserInfo.performed += PlayerUserInfo;
        Input.Player.UserInfo.canceled += PlayerUserInfo;
        Input.Player.SettingWindow.performed += PlayerSettingWindow;
        Input.Player.SettingWindow.canceled += PlayerSettingWindow;
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
        OnMoveEvent?.Invoke(moveValue);
    }
    public void PlayerSubCommand(InputAction.CallbackContext context)
    {
        OnSubCommandEvent?.Invoke();
    }
    public void PlayerDash(InputAction.CallbackContext context)
    {
        OnDashEvent?.Invoke();
    }
    public void PlayerJump(InputAction.CallbackContext context)
    {
        OnJumpEvent?.Invoke();
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
    }
    public void PlayerInteract(InputAction.CallbackContext context)
    {
    }
    public void PlayerUserInfo(InputAction.CallbackContext context)
    {
    }
    public void PlayerSettingWindow(InputAction.CallbackContext context)
    {
    }
}