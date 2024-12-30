using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PooledSound : MonoBehaviour, ISetPooledObject<PooledSound>
{
    [SerializeField] private AudioSource audioSource;
    protected IObjectPool<PooledSound> objectPool;
    public IObjectPool<PooledSound> ObjectPool
    { get => objectPool; set => objectPool = value; }

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
        audioSource.PlayOneShot(clip);
        ObjectPool.Release(this);
    }
}
