using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public void testUIButton1()
    {
        UIManager.Instance.ToggleUI<UItest>(isPreviousWindowActive:true);
    }
    public void testUIButton2()
    {
        UIManager.Instance.ToggleUI<UItest2>(isPreviousWindowActive:true);
    }
}
