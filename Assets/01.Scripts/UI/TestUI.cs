using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestUI : MonoBehaviour
{
    private void Update()
    {
        // ESC 키를 눌렀을 때 ESCUI 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.ToggleUI<ESCUI>(true);
            Debug.Log("ESC");
        }

        // Tab 키를 눌렀을 때 InventoryUI 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.Instance.ToggleUI<InventoryUI>(true);
            Debug.Log("TAB");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 1;
        }
        
    }
}
