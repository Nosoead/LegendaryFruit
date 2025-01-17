using UnityEngine;
using UnityEngine.UI;

public class SoundBarUI : UIBase
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider bgmSliderbar;
    [SerializeField] private Slider sfxSliderbar;


    private void Start()
    {
        if (!PlayerPrefs.HasKey("Volume")) PlayerPrefs.SetFloat("Volume", 1f);
        if (!PlayerPrefs.HasKey("BGM")) PlayerPrefs.SetFloat("BGM", 1f);
        if (!PlayerPrefs.HasKey("SFX")) PlayerPrefs.SetFloat("SFX", 1f);
        
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        bgmSliderbar.value = PlayerPrefs.GetFloat("BGM", 1f);
        sfxSliderbar.value = PlayerPrefs.GetFloat("SFX", 1f);
        
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
        bgmSliderbar.onValueChanged.AddListener(OnBgmSliderChanged);
        sfxSliderbar.onValueChanged.AddListener(OnSfxSliderChanged);
        
        SoundManagers.Instance.SetVolumeMaster(volumeSlider.value);
        SoundManagers.Instance.SetVolume("BGM", bgmSliderbar.value);
        SoundManagers.Instance.SetVolume("SFX", sfxSliderbar.value);
    }

    public void OnVolumeSliderChanged(float value)
    {
        SoundManagers.Instance.SetVolumeMaster(value);
    }

    public void OnBgmSliderChanged(float value)
    {
        SoundManagers.Instance.SetVolume("BGM", value);
    }
    public void OnSfxSliderChanged(float value)
    {
        SoundManagers.Instance.SetVolume("SFX", value);
    }
}
