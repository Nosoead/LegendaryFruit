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
    [SerializeField] private TMP_Text nowTime;

    private PlayerInput input {get; set; }

    private void Awake()
    {
        input = GatherInputManager.Instance.input;
    }
    private void Update()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time); // 일시정지되면 같이 멈추는 시간
        //TimeSpan timeSpan = TimeSpan.FromSeconds(Time.unscaledTime); //일시정지되도 흘러가는 시간
        nowTime.text = timeSpan.ToString(@"hh\:mm\:ss");
    }

    public override void Open()
    {
        base.Open();
        backButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<ESCUI>(true));
        backButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
        settingButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<SettingUI>(false));
        settingButton.onClick.AddListener(() => UIManager.Instance.ToggleSettingState(true));
        settingButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
        newGameButton.onClick.AddListener(() => DataManager.Instance.DeleteData<SaveDataContainer>());
        newGameButton.onClick.AddListener(() => SceneManagerExtension.Instance.LoadScene(SceneType.OneCycleScene));
        newGameButton.onClick.AddListener(() => SoundManagers.Instance.PlaySFX(SfxType.UIButton));
        newGameButton.onClick.AddListener(() => input.Changer.Enable());
        newGameButton.onClick.AddListener(() => input.Player.Enable());
        exitButton.onClick.AddListener(() => Quit());
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