using System;
using UnityEngine;

public class GatherInputManager : Singleton<GatherInputManager>
{
    public PlayerInput input;
    public bool isNpc {get;set;}
    public bool isPlay {get;set;}
    public bool isTab {get;set;}
    public bool isEsc {get;set;}

    protected override void Awake()
    {
        base.Awake();
        input = new PlayerInput();
 
    }
    
    public void SetInput()
    {
        input.Player.Enable();
        input.UI.Disable();
    }
    public void ResetStates()
    {
        isPlay = true;
        isNpc = false;
        isTab = false;
        isEsc = false;

        input.Changer.Enable();
        input.Player.Enable();
        input.UI.Disable();
        Time.timeScale = 1f;
    }
}
