using UnityEngine;

public abstract class MonsterAttributeLogics 
{
    public abstract void ApplyAttackLogic(GameObject target, float damage,float attributeValue=0,float attributeRateTime=0,int attributeStack=0);
    //각 상태이상에 대한 정의
}

public class NormalLogic : MonsterAttributeLogics
{ // 통과할수 있는가

    public override void ApplyAttackLogic(GameObject target, float damage,float attributeValue=0,float attributeRateTime=0,int attributeStack=0)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);

        }
    }
    //공격범위 내에 있는 몬스터를 컬렉션으로 모아서
    //그 각각의 몬스터한테 데미지를 줌 
    //    컬렉션 for문
}

public class BurnLogic : MonsterAttributeLogics
{
    //추후 SO 리팩토링
    /*private float attributeValue;
    private float attributeRateTime;
    private int attributeStack;*/
    public override void ApplyAttackLogic(GameObject target, float damage,float attributeValue=0,float attributeRateTime=0,int attributeStack=0)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.BurnDamage(damage,attributeValue,attributeRateTime,attributeStack);

        }
    }
}
public class SlowDown : MonsterAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage,float attributeValue=0,float attributeRateTime=0,int attributeStack=0)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.SlowDown(damage,attributeValue,attributeRateTime);

        }
    }
}