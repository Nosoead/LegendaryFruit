using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStatManager statManager;
    //�� �Ӽ��� ���� �ʿ� �ڷ�ƾ
    private Coroutine coBurnDamage;
    private Coroutine coSlowDown;

    //�� �ڷ�ƾ�� ���� �ð��ֱ� ĳ��??
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
        }
        coSlowDown = StartCoroutine(SlowDownCoroutine(attributeValue, attributeRateTime));
    }

    private IEnumerator SlowDownCoroutine(float attributeValue, float attributeRateTiem)
    {
        slowDownTime = new WaitForSeconds(attributeRateTiem);
        //TODO ���� ���
        yield return slowDownTime;
        //TODO ���� ����
    }
    #endregion
    public void TakeDamage(float damage)
    {
        statManager.ApplyInstantDamage(damage);
    }
}
