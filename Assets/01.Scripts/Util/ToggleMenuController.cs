using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ToggleMenuController : MonoBehaviour
{
    public PlayerInput input;
    public UnityAction OnUserInfoEvent;
    public UnityAction OnSettingWindowEvent;
    private bool isInventoryWindowOpen = false;
    private bool isSettingWindowOpen = false;

    private void Awake()
    {
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        //TODO UI -> GameManager refactoring
        input.Player.UserInfo.started += PlayerUserInfo;
        //Input.Player.UserInfo.canceled += PlayerUserInfo;
        input.Player.SettingWindow.started += PlayerSettingWindow;
        //Input.Player.SettingWindow.canceled += PlayerSettingWindow;
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

    public void PlayerUserInfo(InputAction.CallbackContext context)
    {
        if (isSettingWindowOpen)
        {
            Debug.Log("윈도우");
            isSettingWindowOpen = !isSettingWindowOpen;
            UIManager.Instance.ToggleUI<ESCUI>(false);
        }
        isInventoryWindowOpen = !isInventoryWindowOpen;
        Debug.Log("Tap" +  isInventoryWindowOpen);
        UIManager.Instance.ToggleUI<InventoryUI>(false);
    }
    public void PlayerSettingWindow(InputAction.CallbackContext context)
    {
        if (isInventoryWindowOpen)
        {
            isInventoryWindowOpen = !isInventoryWindowOpen;
            UIManager.Instance.ToggleUI<InventoryUI>(false);
        }
        isSettingWindowOpen = !isSettingWindowOpen;
        UIManager.Instance.ToggleUI<ESCUI>(false);
    }
}
