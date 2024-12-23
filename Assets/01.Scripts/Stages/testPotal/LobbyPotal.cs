using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPotal : testPotal
{
    protected override void ChagedStage()
    {
        testStageManager.Instance.StageChange("Stage1");
    }
}
