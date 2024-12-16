using DG.Tweening;
using UnityEngine;

public class WeaponAnimationController : AnimationController
{
    private PlayerController playerController;
    private SpriteRenderer playerSprite;
    private Transform handPosition;
    protected override void Awake()
    {
        base.Awake();
        playerSprite = GetComponentInParent<SpriteRenderer>();
    }


    private void LateUpdate()
    {
        bool isFlip = playerSprite.flipX;
        if (isFlip)
        {
            Sprite.flipX = true;
            this.transform.localPosition = new Vector3(-0.8f, 0.8f, 0);
        }
        if (!isFlip)
        {
            Sprite.flipX = false;
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
        if(handPosition == null)
        {
            handPosition = GetComponentInParent<Transform>();
        }
    }
    private void OnAttackEvent()
    {
        Animator.SetTrigger("Attack");
    }
}
