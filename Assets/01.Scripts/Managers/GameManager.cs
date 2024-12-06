using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private RewardTree rewardTree;

    public void testUIButton1()
    {
        UIManager.Instance.ToggleUI<UItest>(isPreviousWindowActive:false);
    }
    public void testUIButton2()
    {
        UIManager.Instance.ToggleUI<UItest2>(isPreviousWindowActive: false);
    }

    // Test ¿ë
    public bool isClear = false;
    public bool isGetWeapon = false;
    public bool isCreatReward = false;
    public void StageClear()
    {
        if (!isClear)
        {
            rewardTree.SetReward();
            isClear = true;
        }
        else { return; };
    }
}
