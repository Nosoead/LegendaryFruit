using UnityEngine;

public abstract class PlayerAttributeLogics
{
    public abstract void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, float attributeStack, float lookDirection);
}

public class PlayerBurn : PlayerAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, float attributeStack, float lookDirection)
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
    public override void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, float attributeStack, float lookDirection)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.SlowDown(damage, attributeValue, attributeRateTime);
        }
    }
}

public class PlayerKnockback : PlayerAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, float attributeStack, float lookDirection)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Knockback(damage, attributeValue, attributeRateTime, lookDirection);
        }
    }
}

public class PlayerNormal : PlayerAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage, float attributeValue, float attributeRateTime, float attributeStack, float lookDirection)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}