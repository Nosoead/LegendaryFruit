using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public UnityAction<float> OnDirectionEvent;
    public UnityAction<float, bool> OnMoveEvent;
    public UnityAction<bool, bool> OnSubCommandEvent;
    public UnityAction<bool> OnDashEvent;
    public UnityAction<bool> OnJumpEvent;
    public UnityAction OnAttackEvent;
    public UnityAction OnSkill1Event;
    public UnityAction OnSkill2Event;
    public UnityAction OnSwapWeaponEvent;
    public UnityAction<bool> OnTapInteractEvent;
    public UnityAction<bool> OnHoldInteractEvent;
    
    public PlayerInput input;
    private bool isLeftPressed;
    private bool isRightPressed;
    private bool isDownPressed;
    private bool isUpPressed;
    private bool isPlay = true;
    private void Awake()
    {
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        //PlayerMovement
        input.Player.Move.started += PlayerDirection;
        input.Player.Move.started += PlayerMove;
        input.Player.Move.canceled += PlayerMove;

        //SubCommand ex) UpArrow + Attak, DownArrow + Jump ...
        input.Player.SubCommand.started += PlayerSubCommand;
        input.Player.SubCommand.canceled += PlayerSubCommand;

        //PlayerMovement + alpha
        input.Player.Dash.started += PlayerDash;
        input.Player.Dash.canceled += PlayerDash;
        input.Player.Jump.started += PlayerJump;
        input.Player.Jump.canceled += PlayerJump;

        //PlayerAttack
        input.Player.Attack.started += PlayerAttack;
        input.Player.Skill1.performed += PlayerSkill1;
        input.Player.Skill1.canceled += PlayerSkill1;
        input.Player.Skill2.performed += PlayerSkill2;
        input.Player.Skill2.canceled += PlayerSkill2;

        //PlayerEquip
        input.Player.SwapWeapon.started += PlayerSwapWeapon;

        //PlayerInteract -> 
        input.Player.Interact.started += PlayerTapInteract;
        input.Player.Interact.canceled += PlayerTapInteract;
        input.Player.Interact.performed += PlayerHoldInteract;
        input.Player.Interact.canceled += PlayerHoldInteract;
        
        //input.Changer.Change.started += Change;
        //input.Changer.Enable();
        //input.Player.Enable();
    }

    private void OnDisable()
    {
        //input.Changer.Change.started -= Change;
        //input.Player.Disable();
    }
    /*private void Change(InputAction.CallbackContext context)
    {
        isPlay = !isPlay;
        //Time.timeScale = isPlay ? 1f : 0f;
        if (isPlay)
        {
            input.UI.Disable();
            input.Player.Enable();
        }
        else
        {
            input.Player.Disable();
            input.UI.Enable();
        }
    }*/
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
        float inputValue = context.ReadValue<float>();
        if (context.started)
        {
            if (inputValue < -0.01f)
            {
                isDownPressed = true;
            }
            else if (inputValue > 0.01f)
            {
                isUpPressed = true;
            }
        }
        else if (context.canceled)
        {
            if (isDownPressed && inputValue > -0.01f)
            {
                isDownPressed = false;
            }
            else if (isUpPressed && inputValue < 0.01f)
            {
                isUpPressed = false;
            }
        }

        bool IsDownPressed = isDownPressed;
        bool IsUpPressed = isUpPressed;
        OnSubCommandEvent?.Invoke(isDownPressed, isUpPressed);
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
    
}