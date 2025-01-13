using UnityEngine;
using UnityEngine.Events;

public class WeaponAnimationController : AnimationController
{
    private PlayerEquipment equipment;
    private PlayerController playerController;
    private WeaponSO currentWeapon;
    private SpriteRenderer playerSprite;
    private SpriteRenderer[] effectSprite;
    private Transform handPosition;
    private static readonly int Attack = Animator.StringToHash("Attack");

    [SerializeField] private ParticleSystem particle;
    [SerializeField] private ParticleSystemRenderer particleSystemRenderer;
    private Vector3 currentEffectFlip;
    private bool particleFlip;

    protected override void Awake()
    {
        base.Awake();
        currentEffectFlip = particleSystemRenderer.flip;
    }

    private void LateUpdate()
    {
        CheckAttack();
    }

    private void OnEnable()
    {
        playerController.OnAttackEvent += OnAttackEvent;
        equipment.OnEquipWeaponChanged += OnChangedAnimator;
        equipment.OnEquipWeaponChanged += OnChangedParticle;
    }
    private void OnDisable()
    {
        playerController.OnAttackEvent -= OnAttackEvent;
        equipment.OnEquipWeaponChanged -= OnChangedAnimator;
        equipment.OnEquipWeaponChanged -= OnChangedParticle;
    }

    protected override void EnsureComponents()
    {
        base.EnsureComponents();
        if (playerController == null)
        {
            playerController = GetComponentInParent<PlayerController>();
        }
        if (Sprite != null || Sprite == null)
        {
            Sprite = GetComponentInChildren<SpriteRenderer>();
        }
        if(playerSprite == null)
        {
            playerSprite = GetComponentInParent<SpriteRenderer>();
        }
        if (particleSystemRenderer == null)
        {
            particleSystemRenderer = particle.GetComponent<ParticleSystemRenderer>();
        }
        if (equipment == null)
        {
            equipment = GetComponent<PlayerEquipment>();
        }
    }
    private void OnAttackEvent()
    {
        OnAttackAnimation();
    }

    private void OnAttackAnimation()
    {
        Animator.SetTrigger(Attack);
    }

    private void OnChangedAnimator(WeaponSO weaponSO)
    {
        currentWeapon = weaponSO;   
        if(weaponSO.animatorController != null)
        Animator.runtimeAnimatorController = weaponSO.animatorController;
    }

    private void OnChangedParticle(WeaponSO weaponSO)
    {
        if (weaponSO != null)
        {
            ChangedMaterial(weaponSO);
            ChangedEffectValue(weaponSO);
        }
    }
    private void ChangedMaterial(WeaponSO weaponData)
    {
        var isRagnedWeapon = particle.textureSheetAnimation;
        if (weaponData.effectMaterial == null)
        {
            isRagnedWeapon.enabled = false;
            return;
        }
        isRagnedWeapon.enabled = true;
        particleSystemRenderer.material = weaponData.effectMaterial;
    }
    private void ChangedEffectValue(WeaponSO weaponData)
    {
        var main = particle.main;
        main.startSize = weaponData.effectData.effectSize;
        var colorOverLifeTime = particle.colorOverLifetime;
        colorOverLifeTime.color = weaponData.effectData.gradient;
        if(weaponData.effectMaterial == null)
        {
            var particleVelocity = particle.velocityOverLifetime;
            particleVelocity.enabled = false;
            return;
        }
        var velocity = particle.velocityOverLifetime;
        velocity.x = weaponData.effectData.linearVelocityX;
        velocity.y = weaponData.effectData.linearVelocityY;
    }

    private void CheckAttack()
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Weapon_Attack"))
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                FlipCheck();
            }
        }
        else
        {
            FlipCheck();
        }
    }

    private void FlipCheck()
    {
        var isFlip = playerSprite.flipX;
        if (isFlip == true)
        {
            SetParticleAndSpritePosition(isFlip);
        }
        if (isFlip == false)
        {
            SetParticleAndSpritePosition(isFlip);
        }
    }

    private void FlipVelocity()
    {
        var filpeedX = particleFlip ? -currentWeapon.effectData.linearVelocityX.constant : currentWeapon.effectData.linearVelocityX;
        var velocity = particle.velocityOverLifetime;
        velocity.x = filpeedX;
    }
    
    private void SetParticleAndSpritePosition(bool isFlip)
    {
        if(isFlip == true)
        {
            particleFlip = true;
            Sprite.flipX = true;
            Sprite.gameObject.transform.localPosition = new Vector2(-0.3f, 0.5f);

            particle.gameObject.transform.localPosition = new Vector2(-1f, 0.05f);
            particleSystemRenderer.flip = new Vector2(1f, 0f);
            FlipVelocity();
        }
        if (isFlip == false)
        {
            particleFlip = false;
            Sprite.flipX = false;
            Sprite.gameObject.transform.localPosition = new Vector2(0.3f, 0.5f);

            particle.gameObject.transform.localPosition = new Vector2(1f, 0.05f);
            particleSystemRenderer.flip = new Vector2(0f, 0f);

            FlipVelocity();
        }
    }


    public void CheckAndPlayParticle()
    {
        if (!particle.isPlaying)
        {
            particle.Play();
        }
        else
        {
            particle.Stop();
        }
    }
}
