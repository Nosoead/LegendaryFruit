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

    // Test 주석입니다.
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

    [SerializeField] private StageBase currentStage = null;

    public void SetStage( )
    {
        currentStage = StageManager.Instance.GetStage<LobbyStage>();
        if( currentStage != null )
        {
            Instantiate(currentStage);
        }
        else
        {
            currentStage = StageManager.Instance.GetStage<LobbyStage>();
        }
    }


}
