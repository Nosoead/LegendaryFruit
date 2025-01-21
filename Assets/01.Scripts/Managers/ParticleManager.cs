using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleManager : Singleton<ParticleManager>
{
    private IObjectPool<PooledParticle> pooledParticle;
    public ParticleHelper particleHelper;

    private Dictionary<ParticleType, ParticleSO> particleDictionary = new Dictionary<ParticleType, ParticleSO>();

    private float lookDir;

    protected override void Awake()
    {
        base.Awake();
        SetParticleType();
        particleHelper = new ParticleHelper();
    }

    private void Start()
    {
        PoolManager.Instance.CreatePool<PooledParticle>(PoolType.PooledParticle, false, 7, 12);
        CacheParticle();
    }

    private void CacheParticle()
    {
        pooledParticle = PoolManager.Instance.GetObjectFromPool<PooledParticle>(PoolType.PooledParticle);
    }


    private void SetParticleType()
    {
        var particeData = ResourceManager.Instance.LoadAllResources<ParticleSO>("Particle");
        foreach (var data in particeData)
        {
            particleDictionary.Add(data.particleType, data); 
        }
    }

    public void SetParticleLookDirection(float dir)
    {
        lookDir = dir;
    }

    public void SetParticleTypeAndPlay(Vector3 position, ParticleType type)
    {
        PooledParticle particle =  pooledParticle.Get();
        if(particleDictionary.ContainsKey(type))
        {
            var particleType = particleDictionary[type];
            particle.SetLookDirection(lookDir);
            particle.SetHelpaer(particleHelper);
            particle.SetParticeType(particleType);
            particle.gameObject.transform.position = position;
            particle.ParticlePlay();
        }
        else
        {
            Debug.Log("None Dictionary");
        }
    }
}

