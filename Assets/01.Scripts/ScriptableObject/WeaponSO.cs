using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSO : ScriptableObject
{
    string name;
    string desciption;
    float attackPoint;
    //AttributType type; <- enum
    float eatValue;
    float attributeValue;
    float attributeRateTime;
    Sprite sprite;
    public Sprite rewardSprite;
}
