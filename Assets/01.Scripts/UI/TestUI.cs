using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{

    private void Awake()
    {
        UIManager.Instance.ForeInit();
    }
    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.R))
        {
            UIManager.Instance.ToggleUI<GameEndUI>(false);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            UIManager.Instance.ToggleUI<WeaponUpgradeUI>(false);
        }
    }
}