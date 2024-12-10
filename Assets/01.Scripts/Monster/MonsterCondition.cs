using System.Collections;
using UnityEngine;

public class MonsterCondition : MonoBehaviour,IDamageable
{

    [SerializeField] private Monster monster; //stat statHandler
    [SerializeField] private Stat stat;
    [SerializeField] private StatHandler statHandler;

    private bool isBurn = false;
    private bool isSlow = false;

    private Coroutine slowDownCoroutine;
    private Coroutine burnCoroutine;
    public void TakeDamage(float damage)
    {
        monster.Data.maxHealth -= damage;

        if (monster.Data.maxHealth <= 0)
        {
            Die();
        }
    }

    public void BurnDamage(float damage, float attributeValue, float attributeRateTime, int attributeStack)
    {
        if (isBurn) return;
        
        isBurn = true;
        burnCoroutine = StartCoroutine(BurnCoroutine(damage, attributeValue, attributeRateTime));
    }

    private IEnumerator BurnCoroutine(float damage, float attributeValue, float attributeRateTime)
    {
        float burnDuration = attributeRateTime;
        while (burnDuration > 0)
        {
            TakeDamage(damage);
            burnDuration -= attributeValue;
            
            yield return new WaitForSeconds(attributeRateTime);
        }
        isBurn = false;
        //코루틴 화상뎀
    }
    
    
    public void SlowDown(float damage, float attributeValue, float attributeRateTime)
    {
        if(isSlow) return;
        
        isSlow = true;
        slowDownCoroutine = StartCoroutine(SlowDownCoroutine(damage,attributeValue, attributeRateTime));
    }
    private IEnumerator SlowDownCoroutine(float damage, float slowAmount, float attributeRateTime)
    {
        float slowDuration = attributeRateTime;
        while (slowDuration > 0)
        {
            TakeDamage(damage);
            monster.Data.moveSpeed -= slowAmount;
            
            slowDuration -= Time.deltaTime;
            yield return null;
        }
        monster.Data.moveSpeed += slowAmount;
        isSlow = false;
        //코루틴 슬로우다운
    }
    public void Die()
    {
        //Pool, SetActive(false)
    }
}
