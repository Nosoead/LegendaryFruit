using System;
using UnityEngine;

public class GatherInput : Singleton<GatherInput>
{
  public PlayerInput input;

  protected override void Awake()
  {
    base.Awake();
    input = new PlayerInput();
  }
}
