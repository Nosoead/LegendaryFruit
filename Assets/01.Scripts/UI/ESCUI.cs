using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ESCUI : UIBase
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text currentTime;

    public override void Open()
    {
        base.Open();
        backButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<ESCUI>(true));
        backButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
        backButton.onClick.AddListener(() => GatherInputManager.Instance.ResetStates());
        settingButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<SettingUI>(false));
        settingButton.onClick.AddListener(() => UIManager.Instance.ToggleSettingState(true));
        settingButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
        newGameButton.onClick.AddListener(() => DataManager.Instance.DeleteData<SaveDataContainer>());
    
        newGameButton.onClick.AddListener(() => SceneManagerExtension.Instance.LoadScene(SceneType.OneCycleScene));
        newGameButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
        exitButton.onClick.AddListener(() => Quit());
        TimeSpan timeSpan = GameManager.Instance.GetCurrentTimeData();
        currentTime.text = timeSpan.ToString(@"hh\:mm\:ss");
        
    }

    private void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Unity Editor 플레이 모드 종료
#else
    Application.Quit(); // 빌드된 게임 종료
#endif
    }
}