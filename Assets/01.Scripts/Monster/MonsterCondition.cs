using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MonsterCondition : MonoBehaviour, IDamageable
{
    [SerializeField] private MonsterStatManager statManager;
    public event UnityAction<AttributeType> OnTakeHitType;
    private MonsterAnimationController controller;
    // 각 속성을 확인하여 필요한 데이터를 반환
    private Coroutine coBurnDamage;
    private Coroutine coSlowDown;

    // 각 속성 남은 시간 미리 캐싱하는 것 논의 필요.
    private WaitForSeconds burnWaitTime;
    private WaitForSeconds slowDownTime;

    // 각 속성 코루틴 체크용
    private bool isBurn = false;
    private bool isSlowDown = false;

    private void Awake()
    {
        if (statManager == null)
        {
            statManager = GetComponent<MonsterStatManager>();
        }
        if(controller == null)
        {
            controller = GetComponent<MonsterAnimationController>();
        }
    }

    #region /BurnDamageLogic
    public void BurnDamage(float damage, float attributeValue, float attributeRateTime, float attributeStack)
    {
        controller.OnTakeHit();
        OnTakeHitType?.Invoke(AttributeType.Burn);
        statManager.ApplyInstantDamage(damage);
        Debug.Log("1타");
        if (coBurnDamage != null && isBurn)
        {
            StopCoroutine(coBurnDamage);
        }
        isBurn = true;
        coBurnDamage = StartCoroutine(BurnDamageCoroutine(attributeValue, attributeRateTime, attributeStack));
    }

    private IEnumerator BurnDamageCoroutine(float attributeValue, float attributeRateTime, float attributeStack)
    {
        burnWaitTime = new WaitForSeconds(attributeRateTime);
        Debug.Log(attributeStack);
        for (int i = 0; i < attributeStack; i++)
        {
            Debug.Log($"{i}번째 타격");
            statManager.ApplyInstantDamage(attributeValue);
            yield return burnWaitTime;
        }
        isBurn = false;
    }
    #endregion

    #region /SlowDownLogic
    public void SlowDown(float damage, float attributeValue, float attributeRateTime)
    {
        controller.OnTakeHit();
        OnTakeHitType?.Invoke(AttributeType.SlowDown);
        statManager.ApplyInstantDamage(damage);
        if (coSlowDown != null && isSlowDown)
        {
            StopCoroutine(coSlowDown);
            statManager.ApplyRestoreStat(attributeValue, "MoveSpeed");
        }
        isSlowDown = true;
        coSlowDown = StartCoroutine(SlowDownCoroutine(attributeValue, attributeRateTime));
    }

    private IEnumerator SlowDownCoroutine(float attributeValue, float attributeRateTime)
    {
        slowDownTime = new WaitForSeconds(attributeRateTime);
        statManager.ApplyTemporaryStatReduction(attributeValue, "MoveSpeed");
        yield return slowDownTime;
        isSlowDown = false;
        statManager.ApplyRestoreStat(attributeValue, "MoveSpeed");
    }
    #endregion

    public void TakeDamage(float damage)
    { 
        controller.OnTakeHit();
        OnTakeHitType?.Invoke(AttributeType.Normal);
        statManager.ApplyInstantDamage(damage);
        SfxType randomSfx = (Random.Range(0,2) == 0) ? SfxType.MonsterDamaged1 : SfxType.MonsterDamaged2;
        SoundManagers.Instance.PlaySFX(randomSfx);
    }

}
