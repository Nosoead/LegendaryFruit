using UnityEngine;
//using System.Diagnostics;

public class OneCycleScene : BaseScene
{
    public override void Init()
    {
        StageManager.Instance.Init();
        GameManager.Instance.Init();
        UIManager.Instance.SetUIDictionary();
        UIManager.Instance.ToggleUI<PlayerCanvasUI>(isPreviousWindowActive: false, isOpened: true);
        PlayerInfoManager.Instance.SetCurrency();
        //플레이어 전체 초기화하는 스크립트 뚫어놓을 것. (아마도 PlayerInfoManager)
        //오브젝트 풀의 리셋 및 각씬마다 필요요소 재등록과정 필요.
        //TODO Sound 재생 초기화 및 풀링 등록
    }
}
