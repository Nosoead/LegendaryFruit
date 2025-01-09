using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem particle;
    public ParticleSystem[] numberParticle;
    public ParticleSystemRenderer[] impactParticles;
    public Material damageMaterial;
    private Dictionary<AttributeType, Material[]> typeMaterial = new Dictionary<AttributeType, Material[]>();

    protected virtual void SetTypeMaterial()
    {
         // TODO : 나중에 속성별로 임팩트 변경 시도 
    }

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
    }

    protected virtual void SetDamageColor(Material material, AttributeType type)
    {
        Debug.Log($"{type.ToString()}");
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

    protected virtual void OnHitParticlePlay(float damage, AttributeType type)
    {
        if (particle.isPlaying)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        particle.Clear();
        particle.Play();
    }
}
