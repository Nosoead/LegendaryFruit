using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttributeLogics
{
    protected Monster monster;
    public virtual bool CanPenetrate => false;
    public abstract void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, int attributeStack);

    protected Collider2D[] player = new Collider2D[10];
}

public class PlayerBurn : PlayerAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, int attributeStack)
    {

    }
}

public class PlayerSlowDown : PlayerAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, int attributeStack)
    {

    }
}

public class PlayerNormal : PlayerAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, int attributeStack)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        damageable.TakeDamage(damage);
    }
}