using UnityEngine;
//using System.Diagnostics;

public class OneCycleScene : BaseScene
{
    public override void Init()
    {
        StageManager.Instance.Init();
        UIManager.Instance.ForeInit();
        GameManager.Instance.Init();
        UIManager.Instance.PostInit();
        GameManager.Instance.GameStart();
        PlayerInfoManager.Instance.SetCurrency();
    }
}