using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObject/WeaponSO", order = 4)]
public class WeaponSO : ItemSO
{
    //   ABBCCC -> A : 아이템등급 / B : 아이템타입 / C : 아이템인덱스(0~999)
    //ex)000002 -> 일반무기등급 burn타입 002번째 아이템
    //!! 우선 No.001부터
    public int ID;
    public string weaponName;
    public string description;
    public float attackPower;
    public AttributeType type;
    public GradeType gradeType;
    public float eatValue;
    public float attributeAttackValue;
    public float attributeAttackRateTime;
    public int arrtibuteStatck;
    public Sprite weaponSprite;
    public Sprite rewardSprite;
    public AnimatorOverrideController animatorController;
    public List<RangedAttackData> rangedAttackData;
    public List<EffectData> effectData;
    public Material effectMaterial;
}

[System.Serializable]
public class EffectData
{
    public ParticleSystem.MinMaxCurve linearVelocityX;
    public ParticleSystem.MinMaxCurve linearVelocityY;

    public float effectSize;
    public Gradient gradient;
}

[System.Serializable]
public class RangedAttackData
{
    public ProjectileType projectileType;
    public Sprite projectTileSprite;
    public float attackDistance;
    public float maxDistance;
    public float rangedAttackPower;
    public float rangedAttackSpeed;
    public float rangedAttackRate;
    public List<RangedAttributeData> attributeDatas;
}

[System.Serializable]
public class RangedAttributeData
{
    public AttributeType attributeType;
    public Material trailMaterial;
    public float attributeValue;
    public float attributeLateTime;
    public float attributeStack;
}
