using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartPotal : Potal
{
    protected override void ChagedStage()
    {
        if (GameManager.Instance.isClear == true)
            StageManager.Instance.StageChange("Lobby");
    }
}
