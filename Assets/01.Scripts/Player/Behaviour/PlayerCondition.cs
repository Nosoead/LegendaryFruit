using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStatManager statManager;
    // 각 속성을 확인하여 필요한 데이터를 반환
    private Coroutine coBurnDamage;
    private Coroutine coSlowDown;

    // 각 속성 남은 시간 미리 캐싱하는 것 논의 필요.
    private WaitForSeconds burnWaitTime;
    private WaitForSeconds slowDownTime;

    private void Awake()
    {
        if (statManager == null)
        {
            statManager = GetComponent<PlayerStatManager>();
        }
    }

    #region /BurnDamageLogic
    public void BurnDamage(float damage, float attributeValue, float attributeRateTime, int attributeStack)
    {
        statManager.ApplyInstantDamage(damage);
        
        if (coBurnDamage != null)
        {
            StopCoroutine(coBurnDamage);
        }
        coBurnDamage = StartCoroutine(BurnDamageCoroutine(attributeValue, attributeRateTime, attributeStack));
    }

    private IEnumerator BurnDamageCoroutine(float attributeValue, float attributeRateTime, int attributeStack)
    {
        burnWaitTime = new WaitForSeconds(attributeRateTime);
        for (int i = 0; i < attributeStack; i++)
        {
            statManager.ApplyInstantDamage(attributeValue);
            yield return burnWaitTime;
        }
    }
    #endregion

    #region /SlowDownLogic
    public void SlowDown(float damage, float attributeValue, float attributeRateTime)
    {
        statManager.ApplyInstantDamage(damage);
        if (coSlowDown != null)
        {
            StopCoroutine(coSlowDown);
            statManager.ApplyRestoreStat(attributeValue, "MoveSpeed");
            //TODO 연속으로 맞으면 스탯이 증가하는 버그
        }
        coSlowDown = StartCoroutine(SlowDownCoroutine(attributeValue, attributeRateTime));
    }

    private IEnumerator SlowDownCoroutine(float attributeValue, float attributeRateTime)
    {
        slowDownTime = new WaitForSeconds(attributeRateTime);
        statManager.ApplyTemporaryStatReduction(attributeValue, "MoveSpeed");
        yield return slowDownTime;
        Debug.Log("코루틴 끝");
        statManager.ApplyRestoreStat(attributeValue, "MoveSpeed");
    }
    #endregion

    public void TakeDamage(float damage)
    {
        statManager.ApplyInstantDamage(damage);
    }
}
