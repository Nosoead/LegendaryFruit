using System;
using UnityEngine;
using UnityEngine.Pool;

public class PooledSound : MonoBehaviour, ISetPooledObject<PooledSound>
{
    [SerializeField] private AudioSource audioSource;
    protected IObjectPool<PooledSound> objectPool;
    public IObjectPool<PooledSound> ObjectPool
    { get => objectPool; set => objectPool = value; }

    public event Action OnReleased;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetPooledObject(IObjectPool<PooledSound> pool)
    {
        ObjectPool = pool;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.volume = SoundManagers.Instance.GetSfxVolume();
        audioSource.PlayOneShot(clip);
        Invoke(nameof(RealseSound), clip.length);
    }

    private void RealseSound()
    {
        OnReleased?.Invoke();
        ObjectPool.Release(this);
    }

    public void UpdateVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
