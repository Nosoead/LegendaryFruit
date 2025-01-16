using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencySO", menuName = "ScriptableObject/CurrencySO", order = 6)]
public class CurrencySO : ItemSO
{
    //   ABBCCC -> A : 아이템등급 / B : 아이템타입 / C : 아이템인덱스(0~999)
    //ex)000002 -> 일반무기등급 burn타입 002번째 아이템
    //!! Currency 타입 B:15
    public int ID;
    public string currencyName;
    public string description;
    public GradeType gradeType;
    public float currencyValue;
    public Sprite currencySprite;
    public Sprite rewardSprite;
}
