using UnityEngine;

public abstract class MonsterAttributeLogics 
{
    public abstract void ApplyAttackLogic(GameObject target, float damage,float attributeValue,float attributeRateTime, float attributeStack, float lookDirection);
}

public class MonsterBurn : MonsterAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage,float attributeValue,float attributeRateTime, float attributeStack, float lookDirection)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.BurnDamage(damage,attributeValue,attributeRateTime,attributeStack);
        }
    }
}
public class MonsterSlowDown : MonsterAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage,float attributeValue, float attributeRateTime, float attributeStack, float lookDirection)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.SlowDown(damage,attributeValue,attributeRateTime);
            Debug.Log(attributeValue);
        }
    }
}

public class MonsterKnockback : MonsterAttributeLogics
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

public class MonsterNormal : MonsterAttributeLogics
{ 
    public override void ApplyAttackLogic(GameObject target, float damage,float attributeValue, float attributeRateTime, float attributeStack, float lookDirection)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}