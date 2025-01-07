using UnityEngine;
//using System.Diagnostics;

public class OneCycleScene : BaseScene
{
    public override void Init()
    {
        StageManager.Instance.Init();
        UIManager.Instance.Init();
        GameManager.Instance.Init();
        PlayerInfoManager.Instance.SetCurrency();
    }
}
