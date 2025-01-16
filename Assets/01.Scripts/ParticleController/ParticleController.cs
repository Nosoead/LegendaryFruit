using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem particle;
    public ParticleSystem[] numberParticle;
    public Material damageMaterial;


    // Damage
    protected virtual void OnDamageReceived(float damage, AttributeType type)
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
        OnHitParticlePlay();
    }
    protected virtual void SetDamageColor(Material material, AttributeType type)
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
                // 넉백 미구현이라 임시로 SlowDown이 노란색임
                color = Color.blue;
                break;
            case AttributeType.Knockback:
                color = Color.yellow;
                break;
            case AttributeType.Blindness:
                color = Color.yellow;
                break;
            case AttributeType.Poison:
                break;
        }
        material.color = color;
    }

    // Heal
    protected virtual void OnDamageReceived(float damage)
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

                    SetDamageColor(damageMaterial);

                    SetFrameOverTime(textureSheetAnimation, digit, totalFrames);
                }
            }
            else
            {
                numberParticle[i].gameObject.SetActive(false);
            }
        }
        OnHitParticlePlay();
    }


    protected virtual void SetDamageColor(Material material)
    {
        var color = material.color;
        material.color = Color.green;
    }

    protected virtual void SetFrameOverTime(ParticleSystem.TextureSheetAnimationModule textureSheetAnimation, int frame, int totalFrames)
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

    protected virtual void OnHitParticlePlay()
    {
        if (particle.isPlaying)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        particle.Clear();
        particle.Play();
    }
}
