using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObject/WeaponSO", order = 2)]
public class WeaponSO : GameSO
{
    //   A0B0CD -> A : 아이템등급 / B : 아이템타입 / C : 아이템인덱스(0~999)
    //ex)13012 -> 무기보상 3-1스테이지 2버전 / 현재는 그냥 1스테이지에 전부 1버전으로
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
}