using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInputHandler : MonoBehaviour
{
    private PlayerInput input;
    private bool isPlay;
    private bool isEsc;
    private bool isTab;
    private bool isInteract;
    private bool isNpc;
    private int npcLayer;
    public UnityAction OnInteractEvent;

    private void Awake()
    {
        input = GatherInputManager.Instance.input;
        npcLayer = LayerMask.NameToLayer("NPC");
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
        input.Changer.Disable();
        input.Player.Disable();
        input.UI.Interact.Enable();
    }

    private void OnInteractUI(InputAction.CallbackContext context)
    {
        OnInteractEvent?.Invoke();
        //UI 끄게하고
        // 꺼지면 다른거 다시 ㄷ ㅏ구독
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == npcLayer)
        {
            input.Changer.Interact.Enable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == npcLayer)
        {
            input.UI.Interact.Disable();
            input.Player.Enable();
            input.Changer.Enable();
        }
    }


    private void OnInventory(InputAction.CallbackContext context)
    {
        isPlay = !isPlay;

        if (isPlay)
        {
            input.Changer.Setting.Disable();
            UIManager.Instance.ToggleUI<InventoryUI>(false);
            input.Player.Disable();
        }
        else
        {
            input.Player.Enable();
            UIManager.Instance.ToggleUI<InventoryUI>(false);
            input.Changer.Setting.Enable();
        }
    }

    private void OnSetting(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsSettingOpen) return;
        isPlay = !isPlay;
        if (isPlay)
        {
            input.Changer.Inventory.Disable();
            UIManager.Instance.ToggleUI<ESCUI>(false);
            input.Player.Disable();
        }
        else
        {
            input.Player.Enable();
            UIManager.Instance.ToggleUI<ESCUI>(false);
            input.Changer.Inventory.Enable();
        }
    }
}