using UnityEngine;

public abstract class AttributeLogics 
{
    protected  Monster monster;
    //각 상태이상에 대한 추상클래스
    public virtual bool CanPenetrate => false; // 통과할수 있는가
    public abstract void ApplyAttackLogic(GameObject target, float damage);
    //각 상태이상에 대한 정의
}

public class NormalLogic : AttributeLogics
{
    public override bool CanPenetrate => false; // 통과할수 있는가

    public override void ApplyAttackLogic(GameObject target, float damage)
    {
        IDamageable damageable = target.gameObject.GetComponent<IDamageable>();
        //공격범위 내에 있는 몬스터를 컬렉션으로 모아서
        //그 각각의 몬스터한테 데미지를 줌 
        //    컬렉션 for문
        //데미지를 줌 이값은 누가넣어줌? > SO값
        damageable.TakeDamage(monster.Data.attackPower);
        //(float damage, float attributeValue, float attributeRateTime);
    }
}

public class BurnLogic : AttributeLogics
{
    public override bool CanPenetrate => false; // 통과할수 있는가

    public override void ApplyAttackLogic(GameObject target, float damage)
    {
        IDamageable damageable = target.gameObject.GetComponent<IDamageable>();
        //damageable.BurnDamage(
            //monster.Data.attackPower,monster.Data.attributeValue,monster.Data.attributeRateTime);
    }
}
public class SlowDown : AttributeLogics
{
    public override bool CanPenetrate => false; // 통과할수 있는가

    public override void ApplyAttackLogic(GameObject target, float damage)
    {
        IDamageable damageable = target.gameObject.GetComponent<IDamageable>();
        damageable.SlowDown(
            monster.Data.attackPower,monster.Data.attributeValue,monster.Data.attributeRateTime);
    }
}