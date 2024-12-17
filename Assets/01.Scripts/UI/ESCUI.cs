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

    private void Update()
    {
        TimeSpan timeSpan =  TimeSpan.FromSeconds(Time.time); // 일시정지되면 같이 멈추는 시간
        //TimeSpan timeSpan = TimeSpan.FromSeconds(Time.unscaledTime); //일시정지되도 흘러가는 시간
        nowTime.text = timeSpan.ToString(@"hh\:mm\:ss");
    }

    public override void Open()
    {
        base.Open();
        backButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<ESCUI>(true));
        settingButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ToggleUI<SettingUI>(false);
            SoundManagers.Instance.PlaySFX(SfxType.UIButton);
        });
        newGameButton.onClick.AddListener(() => SceneManager.LoadScene(4)); // 뉴게임에 해당하는 씬 넣기
        exitButton.onClick.AddListener(() => UIManager.Instance.ToggleUI<SettingUI>(false));
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

