using UnityEngine;

public class MonsterCondition : MonoBehaviour,IDamageable
{

    [SerializeField] private Monster monster; //stat statHandler

    public void TakeDamage(float damage)
    {
        
    }

    public void BurnDamage(float damage, float attributeValue, float attributeRateTime)
    {
        
    }

//코루틴 화상뎀
    public void SlowDown(float damage, float attributeValue, float attributeRateTime)
    {

    }

//코루틴 슬로우다운

    public void Die()
    {
        //Pool, SetActive(false)
    }
}
