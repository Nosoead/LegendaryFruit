using UnityEngine;

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
    private ParticleSystemRenderer particleSystemRenderer;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        
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
        Animator.SetTrigger(Attack);
    }

    private void OnChangedAnimator(WeaponSO weaponSO)
    {
        if(weaponSO.animatorController != null)
        Animator.runtimeAnimatorController = weaponSO.animatorController;
    }

    private void OnChangedParticle(WeaponSO weaponSO)
    {
        if (weaponSO.effectMaterial != null)
        {
            ChangedMaterial(weaponSO);
        }
    }
    private void ChangedMaterial(WeaponSO weaponData)
    {
        particleSystemRenderer.material = weaponData.effectMaterial;
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
        bool isFlip = playerSprite.flipX;
        if (isFlip == true)
        {
            SetParticleAndSpritePosition(isFlip);
        }
        if (isFlip == false)
        {
            SetParticleAndSpritePosition(isFlip);
        }
    }
    
    private void SetParticleAndSpritePosition(bool isFlip)
    {
        if(isFlip == true)
        {
            Sprite.flipX = true;
            Sprite.gameObject.transform.localPosition = new Vector2(-0.3f, 0.5f);

            particle.gameObject.transform.localPosition = new Vector2(-1.5f, -0.6f);
            particleSystemRenderer.flip = new Vector2(1f, 0f);
        }
        if(isFlip == false)
        {
            Sprite.flipX = false;
            Sprite.gameObject.transform.localPosition = new Vector2(0.3f, 0.5f);


            particle.gameObject.transform.localPosition = new Vector2(0f, -0.6f);
            particleSystemRenderer.flip = new Vector2(0f, 0f);
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
