using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParticleController : MonoBehaviour
{
    [SerializeField] private MonsterStatManager monsterStatManager;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private ParticleSystem[] numberParticle;
    [SerializeField] private Material damageMaterial;

    private void Awake()
    {
        EnsureComponents();
    }

    private void OnEnable()
    {
        monsterStatManager.DamageTakenEvent += OnDamageReceived;
        monsterStatManager.DamageTakenEvent += OnHitParticlePlay;
    }
    private void OnDisable()
    {
        monsterStatManager.DamageTakenEvent -= OnDamageReceived;
        monsterStatManager.DamageTakenEvent -= OnHitParticlePlay;
    }

    private void EnsureComponents()
    {
        if(monsterStatManager == null)
        {
            monsterStatManager = GetComponent<MonsterStatManager>();    
        }
        if(particle == null)
        {
            particle = GetComponentInChildren<ParticleSystem>();
        }
    }

    public void OnDamageReceived(float damage, AttributeType type)
    {
        int damageInt = Mathf.FloorToInt(damage);
        string str = damageInt.ToString("D3");
        bool isLeadingZero = true;

        for (int i = 0; i < numberParticle.Length; i++)
        {
            if (i < str.Length)
            {
                int digit = int.Parse(str[i].ToString());
                if (digit == 0 && isLeadingZero && i < str.Length - 1)
                {
                    numberParticle[i].gameObject.SetActive(false);
                }
                else
                {
                    isLeadingZero = false;
                    numberParticle[i].gameObject.SetActive(true);
                    var textureSheetAnimation = numberParticle[i].textureSheetAnimation;

                    int totalFrames = textureSheetAnimation.numTilesX * textureSheetAnimation.numTilesY;

                    SetDamageColor(damageMaterial, type);

                    SetFrameOverTime(textureSheetAnimation, digit, totalFrames);
                }
            }
            else
            {
                numberParticle[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetDamageColor(Material material, AttributeType type)
    {
        var color = material.color;
        switch (type)
        {
            case AttributeType.Normal:
                color = Color.white;
                break;
            case AttributeType.Burn:
                color = Color.red;
                break;
            case AttributeType.SlowDown:
                color = Color.blue;
                break;
            case AttributeType.Blindness:
                color = Color.yellow;
                break;
            case AttributeType.Poison:
                break;
        }
        material.color = color;
    }

    private void SetFrameOverTime(ParticleSystem.TextureSheetAnimationModule textureSheetAnimation, int frame, int totalFrames)
    {
        if (frame < 0 || frame >= totalFrames)
        {
            return;
        }
        float ratio = (float)frame / (totalFrames - 1);
        var frameCurve = textureSheetAnimation.frameOverTime;
        frameCurve.constant = ratio;
        textureSheetAnimation.frameOverTime = frameCurve;
    }

    public void OnHitParticlePlay(float damage, AttributeType type)
    {
        if (particle.isPlaying)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        particle.Clear();
        particle.Play();
    }
}
