using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIBase
{
    [SerializeField] private Button exitButton;
    [SerializeField] private SoundBarUI soundbarUI;

    public static bool IsActive { get; private set; } = false;

    public override void Open()
    {
        base.Open();
        IsActive = true;
        exitButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<SettingUI>(false));
        exitButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
        //if (Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.F))
    }

    public override void Close()
    {
        base.Close();
        IsActive = false; 
    }
}
