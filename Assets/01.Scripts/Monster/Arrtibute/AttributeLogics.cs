using UnityEngine;

public abstract class AttributeLogics 
{
    protected  Monster monster;
    //각 상태이상에 대한 추상클래스
    public virtual bool CanPenetrate => false; // 통과할수 있는가
    public abstract void ApplyAttackLogic(GameObject target, float damage);
    //각 상태이상에 대한 정의
    protected Collider2D[] player = new Collider2D[10];
}

public class NormalLogic : AttributeLogics
{
    public override bool CanPenetrate => true; // 통과할수 있는가

    public override void ApplyAttackLogic(GameObject target, float damage)
    {
        int hitCount  = Physics2D.OverlapCircleNonAlloc(target.transform.position, 1.5f, player);
        for (int i = 0; i < hitCount; i++)
        {
            IDamageable damageable = player[i].gameObject.GetComponent<IDamageable>();
            if (damageable != null && CanPenetrate)
            {
                damageable.TakeDamage(damage);
                Debug.Log(damage);
            }
        }
        //공격범위 내에 있는 몬스터를 컬렉션으로 모아서
        //그 각각의 몬스터한테 데미지를 줌 
        //    컬렉션 for문
    }
}

public class BurnLogic : AttributeLogics
{
    public override bool CanPenetrate => true; // 통과할수 있는가

    public override void ApplyAttackLogic(GameObject target, float damage)
    {
        int hitCount  = Physics2D.OverlapCircleNonAlloc(target.transform.position, 1.5f, player);
        for (int i = 0; i < hitCount; i++)
        {
            IDamageable damageable = player[i].gameObject.GetComponent<IDamageable>();
            if (damageable != null && CanPenetrate)
            {
                damageable.BurnDamage(damage);
                Debug.Log(damage);
            }
        }
    }
}
public class SlowDown : AttributeLogics
{
    public override bool CanPenetrate => true; // 통과할수 있는가

    public override void ApplyAttackLogic(GameObject target, float damage)
    {
        int hitCount  = Physics2D.OverlapCircleNonAlloc(target.transform.position, 1.5f, player);
        for (int i = 0; i < hitCount; i++)
        {
            IDamageable damageable = player[i].gameObject.GetComponent<IDamageable>();
            if (damageable != null && CanPenetrate)
            {
                damageable.SlowDown(damage);
                Debug.Log(damage);
            }
        }
    }
}