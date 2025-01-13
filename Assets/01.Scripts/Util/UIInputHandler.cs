using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInputHandler : MonoBehaviour
{
    private GatherInputManager inputManager;
    private PlayerInput input;
    private int npcLayer;
    public UnityAction OnInteractEvent;

    private void Awake()
    {
        inputManager = GatherInputManager.Instance;
        input = GatherInputManager.Instance.input;
        npcLayer = LayerMask.NameToLayer("TalkableNPC");
    }

    private void OnEnable()
    {
        input.Changer.Interact.started += OnInteract;
        input.UI.Interact.started += OnInteractUI;
        input.Changer.Inventory.started += OnInventory;
        input.Changer.Setting.started += OnSetting;
    }

    private void OnDisable()
    {
        input.Changer.Interact.started -= OnInteract;
        input.UI.Interact.started -= OnInteractUI;
        input.Changer.Inventory.started -= OnInventory;
        input.Changer.Setting.started -= OnSetting;
    }


    private void OnInteract(InputAction.CallbackContext context)
    {
        if (inputManager.isNpc || inputManager.isPlay)
        {
            if (!inputManager.isEsc && !inputManager.isTab)
            {
                input.UI.Interact.Enable();
                input.Changer.Disable();
                //input.Player.Disable();
            }
        }
    }

    private void OnInteractUI(InputAction.CallbackContext context)
    {
        if (!inputManager.isEsc && !inputManager.isTab)
        {
            OnInteractEvent?.Invoke();
            input.Player.Enable();
            Time.timeScale = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == npcLayer)
        {
            inputManager.isNpc = true;
            input.Changer.Disable();
            input.Changer.Interact.Enable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == npcLayer)
        {
            inputManager.isNpc = false;
            input.UI.Interact.Disable();
            input.Player.Enable();
            input.Changer.Enable();
        }
    }


    private void OnInventory(InputAction.CallbackContext context)
    {
        inputManager.isPlay = !inputManager.isPlay;

        if (!inputManager.isPlay)
        {
            UIManager.Instance.ToggleUI<InventoryUI>(false);
            inputManager.isTab = true;
            input.Changer.Setting.Disable();
            input.Player.Disable();
            Time.timeScale = 0;
        }
        else
        {
            UIManager.Instance.ToggleUI<InventoryUI>(false);
            inputManager.isTab = false;
            input.Changer.Enable();
            input.Player.Enable();
            Time.timeScale = 1;
        }
    }

    private void OnSetting(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsSettingOpen) return;
        inputManager.isPlay = !inputManager.isPlay;
        if (!inputManager.isPlay)
        {
            UIManager.Instance.ToggleUI<ESCUI>(false);
            inputManager.isEsc = true;
            input.Changer.Inventory.Disable();
            input.Player.Disable();
            Time.timeScale = 0;
        }
        else
        {
            UIManager.Instance.ToggleUI<ESCUI>(false);
            inputManager.isEsc = false;
            input.Changer.Enable();
            input.Player.Enable();
            Time.timeScale = 1;
        }
    }
}