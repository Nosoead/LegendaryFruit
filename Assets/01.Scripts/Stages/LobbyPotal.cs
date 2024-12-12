using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPotal : Potal
{
    protected override void ChagedStage()
    {
        StageManager.Instance.StageChange("Stage1");
    }
}
