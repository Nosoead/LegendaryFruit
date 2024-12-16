using DG.Tweening;
using UnityEngine;

public class WeaponAnimationController : AnimationController
{
    private PlayerController playerController;
    private SpriteRenderer playerSprite;
    private SpriteRenderer[] effectSprite;
    private Transform handPosition;
    protected override void Awake()
    {
        base.Awake();
    }


    private void LateUpdate()
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
            effectSprite[1].flipX= false;
            this.transform.localPosition = new Vector3(0.8f, 0.8f, 0);
        }
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
        Animator.SetTrigger("Attack");
    }
}
