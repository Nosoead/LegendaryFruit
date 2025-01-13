using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParticeSO", menuName = "ScriptableObject/ParticleSO", order = 5)]
public class ParticleSO  : ScriptableObject
{
    public ParticleType particleType;
    public Material[] materials;
    public List<ParticleData> particleData;
}

[System.Serializable]
public class ParticleData
{
    public ParticleSystem.MinMaxCurve  minMaxCurve;
    public float particleSize;
    public float particleLifeTime;
    public int textureSeetAnimtaionX;
    public int textureSeetAnimtaionY;
    public bool isTextureSeetAntimationActive;
    public bool isSizeOverTimeActive;
}

