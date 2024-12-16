using System.Numerics;
using UnityEngine;

public class MonsterAnimationController : AnimationController
{
    private int isRun = Animator.StringToHash("isRun");
    private int Attack = Animator.StringToHash("Attack");

    protected override void Awake()
    {
        base.Awake();
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
        Animator.SetTrigger("isHit");
    }


}
