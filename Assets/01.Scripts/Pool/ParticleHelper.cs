using UnityEngine;

public class ParticleHelper
{
    public void SetMainModule(ParticleSystem particleSystem, float startSize, ParticleSystem.MinMaxCurve curve, float lifeTime)
    {
        var main = particleSystem.main; 
        main.startRotation = curve;
        main.startSize = startSize;
        main.startLifetime = lifeTime;
    }

    public void SetTextureSheetAnimation(ParticleSystem particleSystem, int tilesX, int tilesY, bool isEnabled)
    {
        var textureSheetAnimation = particleSystem.textureSheetAnimation;
        textureSheetAnimation.enabled = isEnabled;
        textureSheetAnimation.numTilesX = tilesX;
        textureSheetAnimation.numTilesY = tilesY;
    }

    public void SetSizeLifeOverTime(ParticleSystem particleSystem, bool isEnabled)
    {
        var sizeLifeOverTime = particleSystem.sizeOverLifetime;
        sizeLifeOverTime.enabled = isEnabled;
    }

    public void SetEmission(ParticleSystem particleSystem)
    {
        var emission = particleSystem.emission;
        emission.enabled = true;
    }
}
