using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionSO", menuName = "ScriptableObject/PotionSO", order = 5)]
public class PotionSO : ItemSO
{
    //   ABBCCC -> A : 아이템등급 / B : 아이템타입 / C : 아이템인덱스(0~999)
    //ex)000002 -> 일반무기등급 burn타입 002번째 아이템
    //!! Potion 타입 B:10
    public int ID;
    public string weaponName;
    public string description;
    public GradeType gradeType;
    public float potionValue;
    public Sprite weaponSprite;
    public Sprite rewardSprite;
}
