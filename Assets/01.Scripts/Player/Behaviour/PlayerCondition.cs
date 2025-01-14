using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStatManager statManager;
    public event UnityAction<AttributeType> OnTakeHitType;
    private Rigidbody2D playerRigidbody;
    private PlayerInput playerInput;

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

    // 플레이어 데미지 텀
    private Coroutine coTakeDamageCoolDown;
    private WaitForSeconds cooldownTime = new WaitForSeconds(0.2f);
    private bool canTakeDamage = true;


    private void Awake()
    {
        if (statManager == null)
        {
            statManager = GetComponent<PlayerStatManager>();
        }
        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
        }
        if (playerInput == null)
        {
            playerInput = GatherInputManager.Instance.input;
        }
    }

    #region /BurnDamageLogic
    public void BurnDamage(float damage, float attributeValue, float attributeRateTime, float attributeStack)
    {
        if (!canTakeDamage) return;
        StartTakeDamageCooldown();

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
        if (!canTakeDamage) return;
        StartTakeDamageCooldown();

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
        if (!canTakeDamage) return;
        StartTakeDamageCooldown();
        OnTakeHitType?.Invoke(AttributeType.Knockback);
        statManager.ApplyInstantDamage(damage);
        ParticleManager.Instance.SetParticleTypeAndPlay(transform.position, ParticleType.KnockBackDamage);
        if (coKnockback != null && isKnockback)
        {
            StopCoroutine(coKnockback);
        }
        isKnockback = true;
        playerInput.Player.Disable();
        playerRigidbody.velocity = Vector2.zero;
        float currentGravity = playerRigidbody.gravityScale;
        playerRigidbody.gravityScale = 0;
        coKnockback = StartCoroutine(KnockbackCoroutine(attributeValue, attributeRateTime, lookDirection, currentGravity));
    }

    private IEnumerator KnockbackCoroutine(float attributeValue, float attributeRateTime, float lookDirection, float currentGravity)
    {
        yield return null;
        Vector2 knockbackForce = Vector2.right * attributeValue * lookDirection;
        playerRigidbody.AddForce(knockbackForce, ForceMode2D.Impulse);
        knockbackTime = new WaitForSeconds(attributeRateTime);
        yield return knockbackTime;
        playerRigidbody.gravityScale = currentGravity;
        playerRigidbody.velocity = Vector2.zero;
        playerInput.Player.Enable();
        isKnockback = false;
    }
    #endregion

    public void TakeDamage(float damage)
    {
        if (!canTakeDamage) return;
        StartTakeDamageCooldown();

        OnTakeHitType?.Invoke(AttributeType.Normal);
        ParticleManager.Instance.SetParticleTypeAndPlay(transform.position, ParticleType.Heal);
        statManager.ApplyInstantDamage(damage);
        SoundManagers.Instance.PlaySFX(SfxType.PlayerDamaged);
    }

    #region /TakeDamageCooldown
    private void StartTakeDamageCooldown()
    {
        canTakeDamage = false;
        coTakeDamageCoolDown = StartCoroutine(CoolDownCoroutine());
    }

    private IEnumerator CoolDownCoroutine()
    {
        yield return cooldownTime;
        canTakeDamage = true;
    }
    #endregion
}
