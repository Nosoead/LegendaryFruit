using UnityEngine;

public abstract class PlayerAttributeLogics
{
    public abstract void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, float attributeStack);
}

public class PlayerBurn : PlayerAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, float attributeStack)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.BurnDamage(damage, attributeValue, attributeRateTime, attributeStack);
        }
    }
}

public class PlayerSlowDown : PlayerAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, float attributeStack)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.SlowDown(damage, attributeValue, attributeRateTime);
        }
    }
}

public class PlayerNormal : PlayerAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, float attributeStack)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}