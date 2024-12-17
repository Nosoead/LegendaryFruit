using DG.Tweening;
using UnityEngine;

public class WeaponAnimationController : AnimationController
{
    private PlayerController playerController;
    private SpriteRenderer playerSprite;
    private SpriteRenderer[] effectSprite;
    private Transform handPosition;
    private static readonly int Attack = Animator.StringToHash("Attack");

    public bool isAttacking;

    protected override void Awake()
    {
        base.Awake();
    }


    private void LateUpdate()
    {
        CheckAttack();
    }

    private void OnEnable()
    {
        playerController.OnAttackEvent += OnAttackEvent;
    }
    private void OnDisable()
    {
        playerController.OnAttackEvent -= OnAttackEvent;
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
        if(effectSprite == null)
        {
            effectSprite = GetComponentsInChildren<SpriteRenderer>(true);
        }
    }
    private void OnAttackEvent()
    {
        Animator.SetTrigger(Attack);
    }

    private void CheckAttack()
    {
        FlipCheck();
        //if(Animator.GetCurrentAnimatorStateInfo(0).IsName("Weapon_Attack"))
        //{
        //    if(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        //    {
        //        FlipCheck();
        //    }
        //}
        //else
        //{
        //    FlipCheck();
        //}
    }

    private void FlipCheck()
    {
        bool isFlip = playerSprite.flipX;
        if (isFlip)
        {
            Sprite.flipX = true;
            effectSprite[1].flipX = true;
            this.transform.localPosition = new Vector3(-0.8f, 0.8f, 0);
        }
        if (!isFlip)
        {
            Sprite.flipX = false;
            effectSprite[1].flipX = false;
            this.transform.localPosition = new Vector3(0.8f, 0.8f, 0);
        }
    }
}
