using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;


public class SoundManagers : Singleton<SoundManagers>
{
    public AudioSource sfxSource;       //sfx전용 오디오 변수
    public AudioSource bgmSource;       //bgm전용 오디오 변수
    private IObjectPool<PooledSound> pooledSound;
    private List<PooledSound> activePooledSounds = new List<PooledSound>();

    private Dictionary<Enum, AudioClip> clipDic = new Dictionary<Enum, AudioClip>(); //sfx클립 저장해놓는 Dic
    private float volumeMaster;
    private float lastSoundTime;
    private float soundInterval = 0.5f;
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    private void Start()
    {
        PoolManager.Instance.CreatePool<PooledSound>(PoolType.PooledSound, false, 1, 5);
        pooledSound = PoolManager.Instance.GetObjectFromPool<PooledSound>(PoolType.PooledSound) as IObjectPool<PooledSound>;
    }
    /// <summary>
    /// SFX 재생 함수
    /// </summary>
    public void PlaySFX(SfxType type, bool hasInterval  = false)
    {
        if (hasInterval)
        {
            lastSoundTime += Time.deltaTime;
            if (lastSoundTime >= soundInterval)
            {
                var sound = pooledSound.Get();
                RegisterPooledSound(sound);
                sound.PlaySound(clipDic[type]);
                lastSoundTime = 0;
            }
        }
        else
        {
            var sound = pooledSound.Get();
            RegisterPooledSound(sound);
            sound.PlaySound(clipDic[type]);
        }
    
    }

    /// <summary>
    /// BGM 재생 함수
    /// </summary>
    public void PlayBGM(BgmType type)
    {
        if (bgmSource.clip == clipDic[type])
            return;

        bgmSource.clip = clipDic[type];
        bgmSource.Play();
    }

    public void SetVolume(string name, float volume)
    {
        if (name == "BGM")
        {
            bgmSource.volume = volume * volumeMaster;
            PlayerPrefs.SetFloat("BGM", volume);
        }
        else if (name == "SFX")
        {
            sfxSource.volume = volume * volumeMaster;
            PlayerPrefs.SetFloat("SFX", volume);
        }

        foreach (var sound in activePooledSounds)
        {
            sound.UpdateVolume(volume * volumeMaster);
        }
    }

    public void SetVolumeMaster(float volume)
    {
        volumeMaster = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        bgmSource.volume = PlayerPrefs.GetFloat("BGM", 1f) * volumeMaster;
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 1f) * volumeMaster;
    }

    public float GetSfxVolume()
    {
        return PlayerPrefs.GetFloat("SFX", 1f) * volumeMaster;
    }
    private void RegisterPooledSound(PooledSound sound)
    {
        if (!activePooledSounds.Contains(sound))
        {
            activePooledSounds.Add(sound);
        }
        sound.OnReleased += () => activePooledSounds.Remove(sound);
    }
    /// <summary>
    /// 씬 로드 시 호출해주는 함수
    /// </summary>
    private void OnLoadCompleted(Scene scene, LoadSceneMode mode)
    {
        if (bgmSource == null)
        {
            return;
        }
        switch (scene.name)
        {
            case "TitleScene":
                if (bgmSource.clip == null || bgmSource.clip != clipDic[BgmType.Title]) //노래가 새로시작되지않게설정
                {
                    bgmSource.clip = clipDic[BgmType.Title];
                    bgmSource.Play();
                }

                break;
            case "OneCycleScene":
                if (bgmSource.clip == null || bgmSource.clip != clipDic[BgmType.InGame])
                {
                    bgmSource.clip = clipDic[BgmType.InGame];
                    bgmSource.Play();
                }

                break;
            case "Boss":
                if (bgmSource.clip == null || bgmSource.clip != clipDic[BgmType.Boss])
                {
                    bgmSource.clip = clipDic[BgmType.Boss];
                    bgmSource.Play();
                }

                break;
        }
    }

    /// <summary>
    /// SoundManager 생성 함수
    /// </summary>
    private void Init()
    {
        GameObject sfxGo = new GameObject("SFX");
        GameObject bgmGo = new GameObject("BGM");

        sfxGo.transform.SetParent(transform);
        bgmGo.transform.SetParent(transform);

        sfxSource = sfxGo.AddComponent<AudioSource>();
        bgmSource = bgmGo.AddComponent<AudioSource>();

        var sfxClipArr = Resources.LoadAll<AudioClip>("Sounds/SFX");
        ClipLoader<SfxType>(sfxClipArr);

        var bgmClipArr = Resources.LoadAll<AudioClip>("Sounds/BGM");
        ClipLoader<BgmType>(bgmClipArr);

        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.volume = 0.3f;

        sfxSource.playOnAwake = false;
        sfxSource.volume = 1f;

        volumeMaster = PlayerPrefs.GetFloat("Volume", 1f);
        bgmSource.volume = PlayerPrefs.GetFloat("BGM", 1) * volumeMaster;
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 1) * volumeMaster;

        SceneManager.sceneLoaded += OnLoadCompleted;
    }

    /// <summary>
    /// 클립 읽어주는 함수
    /// </summary>
    private void ClipLoader<T>(AudioClip[] arr) where T : Enum
    {
        for (int i = 0; i < arr.Length; i++)
        {
            try
            {
                T type = (T)Enum.Parse(typeof(T), arr[i].name);
                clipDic.Add(type, arr[i]);
            }
            catch
            {
                Debug.LogWarning("Need Enum : " + arr[i].name);
            }
        }
    }
}
