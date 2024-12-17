using System.Numerics;
using UnityEngine;

public class MonsterAnimationController : AnimationController
{
    private int isRun = Animator.StringToHash("isRun");
    private int Attack = Animator.StringToHash("Attack");
    private int isDie = Animator.StringToHash("isDie");
    private int isHit = Animator.StringToHash("isHit");

    private SpriteRenderer effectSprite;

    protected override void Awake()
    {
        base.Awake();
    }

    private void LateUpdate()
    {
        //if(Sprite.flipX)
        //{
        //    effectSprite.flipX = true;
        //    transform.GetChild(0).gameObject.transform.localPosition = new UnityEngine.Vector2(0.5f, 0.2f);
        //}
        //if(!Sprite.flipX)
        //{
        //    effectSprite.flipX = false;
        //    transform.GetChild(0).gameObject.transform.localPosition = new UnityEngine.Vector2(-0.5f, 0.2f);
        //}
    }

    protected override void EnsureComponents()
    {
        base.EnsureComponents();
        if(effectSprite == null)
        {
            effectSprite = GetComponentInChildren<SpriteRenderer>(true);
        }
    }

    public void OnIdle()
    {
        Animator.SetBool(isRun, false);
    }

    public void OnMove()
    {
        Animator.SetBool(isRun, true); 
    }

    public void OnAttack()
    {
        Animator.SetTrigger(Attack);
    }

    public void OnHit()
    {
        Animator.SetTrigger(isHit);
    }

    public void OnDie()
    {
        Animator.SetTrigger(isDie);
    }
}
