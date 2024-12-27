using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCycleScene : BaseScene
{
    public override void Init()
    {
        StageManager.Instance.Init();
        GameManager.Instance.Init();
        //플레이어 전체 초기화하는 스크립트 뚫어놓을 것. (아마도 PlayerInfoManager)
        //오브젝트 풀의 리셋 및 각씬마다 필요요소 재등록과정 필요.
    }
}
