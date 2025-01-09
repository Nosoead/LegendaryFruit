using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputHandler : MonoBehaviour
{
    public PlayerInput input;
    private bool isPlay = true;
    private bool isEsc;
    private bool isTab;
    private bool isInteract;

    private void Awake()
    {
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        /*input.UI.Navigate.started += OnNavigate;
        input.UI.Submit.started += OnSubmit;
        input.UI.Cancel.started += OnCancel;
        input.UI.Point.started += OnPoint;
        input.UI.Click.started += OnClick;
        input.UI.ScrollWheel.started += OnScrollWheel;
        input.UI.RightClick.started += OnRightClick;*/

        input.UI.Interact.started += OnInteract;
        input.UI.Inventory.started += OnInventory;
        input.UI.Setting.started += OnSetting;
        
        input.Changer.Change.started += Change;

        input.Changer.Enable();
        input.UI.Enable();
      
    }

    private void OnDisable()
    {
        input.Changer.Change.started -= Change;
        input.UI.Disable();
    }

    private void Change(InputAction.CallbackContext context)
    {
        isPlay = !isPlay;

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
        Debug.Log("Player enabled: " + input.Player.enabled);
        Debug.Log("UI enabled: " + input.UI.enabled);
        Debug.Log("Changer enabled: " + input.Changer.enabled);
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        
    }
    private void OnInventory(InputAction.CallbackContext context)
    {
        Debug.Log("Inventory");
        if (isEsc) return;
        if (!isTab)
        {
            UIManager.Instance.ToggleUI<InventoryUI>(false);
            isTab = true;
        }
        else
        {
            UIManager.Instance.ToggleUI<InventoryUI>(true);
            isTab = false;
        }
    }

    private void OnSetting(InputAction.CallbackContext context)
    {
        if (isTab) return;
        if (!isEsc)
        {
            UIManager.Instance.ToggleUI<ESCUI>(false);

            isEsc = true;
        }
        else
        {
            UIManager.Instance.ToggleUI<ESCUI>(true);
            isEsc = false;
        }
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
    }

    private void OnPoint(InputAction.CallbackContext context)
    {
    }

    private void OnClick(InputAction.CallbackContext context)
    {
    }

    private void OnScrollWheel(InputAction.CallbackContext context)
    {
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
    }
}