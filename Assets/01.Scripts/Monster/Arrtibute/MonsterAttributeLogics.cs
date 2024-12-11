using UnityEngine;

public abstract class MonsterAttributeLogics 
{
    protected  Monster monster;
    //각 상태이상에 대한 추상클래스
    public virtual bool CanPenetrate => false; // 통과할수 있는가
    public abstract void ApplyAttackLogic(GameObject target, float damage);
    //각 상태이상에 대한 정의
    protected Collider2D[] player = new Collider2D[10];
}

public class NormalLogic : MonsterAttributeLogics
{ // 통과할수 있는가

    public override void ApplyAttackLogic(GameObject target, float damage)
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
    public override void ApplyAttackLogic(GameObject target, float damage)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);

        }
    }
}
public class SlowDown : MonsterAttributeLogics
{
    public override void ApplyAttackLogic(GameObject target, float damage)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);

        }
    }
}