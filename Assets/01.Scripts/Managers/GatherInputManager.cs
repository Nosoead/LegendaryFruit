using System;
using UnityEngine;

public class GatherInputManager : Singleton<GatherInputManager>
{
    public PlayerInput input;

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
}
