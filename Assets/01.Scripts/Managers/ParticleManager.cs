using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleManager : Singleton<ParticleManager>
{
    private IObjectPool<PooledHitParticle> hitParticle;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        PoolManager.Instance.CreatePool<PooledHitParticle>(PoolType.PooledHitParticle, false, 7, 12);
        CacheParticle();
    }

    private void CacheParticle()
    {
        hitParticle = PoolManager.Instance.GetObjectFromPool<PooledHitParticle>(PoolType.PooledHitParticle);
    }


}
