using System.Numerics;
using UnityEngine;

public class MonsterAnimationController : AnimationController
{
    private int isRun = Animator.StringToHash("isRun");
    private int Attack = Animator.StringToHash("Attack");
    private int isDie = Animator.StringToHash("isDie");
    private int isHit = Animator.StringToHash("isHit");
    private int isArea = Animator.StringToHash("isArea");


    private SpriteRenderer effectSprite;

    protected override void Awake()
    {
        base.Awake();
        SetInitMonsterAnimation();
    }

    protected override void EnsureComponents()
    {
        base.EnsureComponents();
        if(effectSprite == null)
        {
            effectSprite = GetComponentInChildren<SpriteRenderer>(true);
        }
    }

    private void SetInitMonsterAnimation()
    {
        Animator.runtimeAnimatorController =
        EntityManager.Instance.monsterData.animatorOverrideController;
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

    public void OnAreaAttack(bool attack)
    {
        if(attack)
        {
            Animator.SetTrigger(isArea);
        }
        else
        {
            Animator.ResetTrigger(isArea);
        }
    }
    public bool OnAreaAttackCheck()
    {
        bool aniName = Animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_AreaAttack");
        float normalizedTime = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (aniName && normalizedTime >= 1)
        {
            return true;
        }
        return false;

    }

    public void Delay(bool isDelay)
    {
        Animator.SetBool("isDelay", isDelay);
    } 

    public void AttackToIdle()
    {
        Animator.SetTrigger("attackToIdle");
    }
}
