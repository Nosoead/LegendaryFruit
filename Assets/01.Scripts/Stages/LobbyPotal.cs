using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPotal : Potal
{
    protected override void ChagedStage()
    {
        if(GameManager.Instance.isClear)
            StageManager.Instance.NextStage("1-1");
    }
}
