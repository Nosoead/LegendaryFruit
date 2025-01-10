/*
using UnityEngine;

public class TestUI : MonoBehaviour
{
    private bool isESC = false;
    private bool isTab = false;


    private void Update()
    {
        /*if (SettingUI.IsActive)
        {
            return;
        }#1#
        // ESC 키를 눌렀을 때 ESCUI 토글
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isESC == false)
            {
                
                isESC = true;
                isTab = false;
                ToggletimeScale(isPlay: false);
            }
            else
            {
                isESC = false;
                ToggletimeScale(isPlay: true);
            }
            UIManager.Instance.ToggleUI<InventoryUI>(true, false);
            UIManager.Instance.ToggleUI<ESCUI>(true);
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
        }

        // Tab 키를 눌렀을 때 InventoryUI 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isTab == false)
            {
                isTab = true;
                isESC = false;
                ToggletimeScale(isPlay: false);
            }
            else
            {
                isTab = false;
                ToggletimeScale(isPlay: true);
            }
            UIManager.Instance.ToggleUI<ESCUI>(true, false);
            UIManager.Instance.ToggleUI<InventoryUI>(true);
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
        }

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Time.timeScale = 0;
        //}
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    Time.timeScale = 1;
        //}

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    UIManager.Instance.ToggleUI<NpcDialougeUI>(false);
        //    SoundManagers.Instance.PlaySFX(SfxType.UIButton);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    UIManager.Instance.ToggleUI<NpcUpgradeUI>(false);
        //    SoundManagers.Instance.PlaySFX(SfxType.UIButton);
        //}
        if (Input.GetKeyDown(KeyCode.T))
        {
            UIManager.Instance.ToggleUI<PromptUI>(false);
        }
    }

    private void ToggleESCTAB()
    {

    }

    private void ToggletimeScale(bool isPlay)
    {
        if (isPlay)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
*/
