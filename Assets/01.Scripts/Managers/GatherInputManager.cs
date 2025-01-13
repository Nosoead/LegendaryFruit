using System;
using UnityEngine;

public class GatherInputManager : Singleton<GatherInputManager>
{
    public PlayerInput input;
    public bool isNpc {get;set;}
    public bool isPlay {get;set;}

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
        isPlay = false;
        isNpc = false;

        input.Player.Enable();
        input.Changer.Enable();
        input.UI.Disable();
        
    }
}
