using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public GameObject player;

    public void testUIButton1()
    {
        UIManager.Instance.ToggleUI<UItest>(isPreviousWindowActive:false);
    }
    public void testUIButton2()
    {
        UIManager.Instance.ToggleUI<UItest2>(isPreviousWindowActive: false);
    }

    // Test 주석입니다.
    public bool isClear = false;
    public bool isGetWeapon = false;
    public bool isCreatReward = false;

    public void FirstStage()
    {
        StageManager.Instance.StartStage();
    }  
}
