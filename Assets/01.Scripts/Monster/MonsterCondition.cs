using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MonsterCondition : MonoBehaviour, IDamageable
{
    [SerializeField] private MonsterStatManager statManager;
    public event UnityAction<AttributeType> OnTakeHitType;
    private MonsterAnimationController controller;
    private Rigidbody2D monsterRigidbody;

    // 각 속성을 확인하여 필요한 데이터를 반환
    private Coroutine coBurnDamage;
    private Coroutine coSlowDown;
    private Coroutine coKnockback;

    // 각 속성 남은 시간 미리 캐싱하는 것 논의 필요.
    private WaitForSeconds burnWaitTime;
    private WaitForSeconds slowDownTime;
    private WaitForSeconds knockbackTime;

    // 각 속성 코루틴 체크용
    private bool isBurn = false;
    private bool isSlowDown = false;
    private bool isKnockback = false;

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
        if (monsterRigidbody == null)
        {
            monsterRigidbody = GetComponent<Rigidbody2D>();
        }

    }

    #region /BurnDamageLogic
    public void BurnDamage(float damage, float attributeValue, float attributeRateTime, float attributeStack)
    {       
        controller.OnTakeHit();
        OnTakeHitType?.Invoke(AttributeType.Normal);
        ParticleManager.Instance.SetParticleTypeAndPlay(transform.position, ParticleType.NormalDamage);
        statManager.ApplyInstantDamage(damage);
        if (coBurnDamage != null && isBurn)
        {
            StopCoroutine(coBurnDamage);
        }
        isBurn = true;
        coBurnDamage = StartCoroutine(BurnDamageCoroutine(attributeValue, attributeRateTime, attributeStack));
    }

    private IEnumerator BurnDamageCoroutine(float attributeValue, float attributeRateTime, float attributeStack)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        burnWaitTime = new WaitForSeconds(attributeRateTime);
        for (int i = 0; i < attributeStack; i++)
        {
            OnTakeHitType?.Invoke(AttributeType.Burn);
            ParticleManager.Instance.SetParticleTypeAndPlay(transform.position, ParticleType.BurnDamage);
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
        ParticleManager.Instance.SetParticleTypeAndPlay(transform.position, ParticleType.SlowDownDamage);
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

    #region /KnockbackLogic
    public void Knockback(float damage, float attributeValue, float attributeRateTime, float lookDirection)
    {
        controller.OnTakeHit();
        OnTakeHitType?.Invoke(AttributeType.Knockback);
        statManager.ApplyInstantDamage(damage);
        ParticleManager.Instance.SetParticleFlip(lookDirection);
        ParticleManager.Instance.SetParticleTypeAndPlay(transform.position, ParticleType.KnockBackDamage);
        if (coKnockback != null && isKnockback)
        {
            StopCoroutine(coKnockback);
        }
        isKnockback = true;
        monsterRigidbody.velocity = Vector2.zero;
        coKnockback = StartCoroutine(KnockbackCoroutine(attributeValue, attributeRateTime, lookDirection));
    }

    private IEnumerator KnockbackCoroutine(float attributeValue, float attributeRateTime, float lookDirection)
    {
        yield return null;
        Vector3 knockbackForce = Vector3.right * attributeValue * lookDirection;
        monsterRigidbody.AddForce(knockbackForce, ForceMode2D.Impulse);
        knockbackTime = new WaitForSeconds(attributeRateTime);
        yield return knockbackTime;
        monsterRigidbody.velocity = Vector2.zero;
        isKnockback = false;
    }
    #endregion

    public void TakeDamage(float damage)
    { 
        controller.OnTakeHit();
        OnTakeHitType?.Invoke(AttributeType.Normal);
        statManager.ApplyInstantDamage(damage);
        ParticleManager.Instance.SetParticleTypeAndPlay(transform.position, ParticleType.NormalDamage);
        SfxType randomSfx = (Random.Range(0,2) == 0) ? SfxType.MonsterDamaged1 : SfxType.MonsterDamaged2;
        SoundManagers.Instance.PlaySFX(randomSfx);
    }

}
