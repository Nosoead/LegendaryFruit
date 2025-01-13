using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Pool;
using UnityEngine.Rendering;

public class PooledParticle : MonoBehaviour, ISetPooledObject<PooledParticle>
{
    protected IObjectPool<PooledParticle> objectPool;
    public ParticleSystem particle;
    public ParticleSystem[] childrenParticles;
    public ParticleSystemRenderer[] particleSystemRenderer;
    private ParticleSO currentParticleData;
    private ParticleHelper particleHelper;
    private List<ParticleData> particleData;
    private Coroutine particleCoroutine;

    public IObjectPool<PooledParticle> ObjectPool
    { get => objectPool; set => objectPool = value; }

    public void SetPooledObject(IObjectPool<PooledParticle> pool)
    {
        ObjectPool = pool;
    }

    public void SetHelpaer(ParticleHelper helper)
    {
        particleHelper = helper;    
    }

    public void SetParticeType(ParticleSO data)
    {
        currentParticleData = data;
        int materialCount = data.materials.Length;
        int rendererCount = particleSystemRenderer.Length;  

        for(int i = 0; i < rendererCount; i++)
        {
            if (i < materialCount)
            {
                particleSystemRenderer[i].material = data.materials[i];
                particleSystemRenderer[i].gameObject.SetActive(true);
            }
            else
            {
                particleSystemRenderer[i].gameObject.SetActive(false);
            }
        }
        ParticleTypeToSetting();
    }

    private void ParticleTypeToSetting()
    {
        if (currentParticleData.particleData == null) return;
        switch(currentParticleData.particleType)
        {
            case ParticleType.Heal:
                ParticleDataSetting(childrenParticles);
                break;
            case ParticleType.NormalDamage:
                ParticleDataSetting(childrenParticles);
                break;
            case ParticleType.BurnDamage:
                ParticleDataSetting(childrenParticles);
                break;
            case ParticleType.SlowDownDamage:
                ParticleDataSetting(childrenParticles);
                break;
        }
    }

    private void ParticleDataSetting(ParticleSystem[] system)
    {
        for(int i = 0; i < currentParticleData.particleData.Count; i++)
        {
            if (system[i].gameObject.activeSelf == false) return;
            particleHelper.SetMainModule(system[i], currentParticleData.particleData[i].particleSize, 
                currentParticleData.particleData[i].minMaxCurve, currentParticleData.particleData[i].particleLifeTime);

            particleHelper.SetTextureSheetAnimation(system[i], currentParticleData.particleData[i].textureSeetAnimtaionX,
                currentParticleData.particleData[i].textureSeetAnimtaionY, currentParticleData.particleData[i].isTextureSeetAntimationActive);

            particleHelper.SetSizeLifeOverTime(system[i], currentParticleData.particleData[i].isSizeOverTimeActive);
        }
    }


    public void ParticlePlay()
    {
        if (particle.isPlaying)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        particle.Play();

        if (particleCoroutine != null)
        {
            StopCoroutine(particleCoroutine);
        }

        particleCoroutine = StartCoroutine(WaitForParticleToStop());
    }

    public void ReleaseParticle()
    {
        objectPool.Release(this);
    }

    IEnumerator WaitForParticleToStop()
    {
        yield return new WaitForSeconds(0.1f);

        while (particle.IsAlive(true))
        {
            yield return null;
        }

        ReleaseParticle();
    }
}
