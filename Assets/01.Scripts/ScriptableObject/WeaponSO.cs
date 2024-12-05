using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObject/WeaponSO", order = 2)]
public class WeaponSO : GameSO
{
    public string name;
    public string description;
    public float attackPower;
    public AttributeType type;
    public GradeType gradeType;
    public float eatValue;
    public float attributeAttackValue;
    public float attributeAttackRateTime;
    public Sprite weaponSprite;
    public Sprite rewardSprite;
}