using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputHandler : MonoBehaviour
{
    private PlayerInput input;
    private bool isPlay;
    private bool isEsc;
    private bool isTab;
    private bool isInteract;
    private bool isNpc;

    private void Awake()
    {
        input = GatherInput.Instance.input;
    }

    private void OnEnable()
    {
        input.Changer.Interact.started += OnInteract;
        input.Changer.Inventory.started += OnInventory;
        input.Changer.Setting.started += OnSetting;

        input.UI.Enable();
    }

    private void OnDisable()
    {
        input.Changer.Interact.started -= OnInteract;
        input.Changer.Inventory.started -= OnInventory;
        input.Changer.Setting.started -= OnSetting;

        input.UI.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isEsc) return;
        if (isTab) return;
        //input.Player.Enable();
    }

    private void OnInventory(InputAction.CallbackContext context)
    {
        if (isEsc) return;
        isPlay = !isPlay;

        if (isPlay && !isTab)
        {
            input.UI.Enable();
            UIManager.Instance.ToggleUI<InventoryUI>(false);
            input.Player.Disable();
            isTab = true;
        }
        else
        {
            input.Player.Enable();
            UIManager.Instance.ToggleUI<InventoryUI>(false);
            input.UI.Disable();
            isTab = false;
        }
    }

    private void OnSetting(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsSettingOpen || isTab) return;
        isPlay = !isPlay;
        if (isPlay && !isEsc)
        {
            input.UI.Enable();
            UIManager.Instance.ToggleUI<ESCUI>(false);
            input.Player.Disable();
            isEsc = true;
        }
        else
        {
            input.Player.Enable();
            UIManager.Instance.ToggleUI<ESCUI>(false);
            input.UI.Disable();
            isEsc = false;
        }
    }
}