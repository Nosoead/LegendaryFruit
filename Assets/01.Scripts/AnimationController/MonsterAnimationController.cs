using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class MonsterAnimationController : AnimationController
{
    private int isRun = Animator.StringToHash("isRun");
    private int Attack = Animator.StringToHash("Attack");
    private int isDie = Animator.StringToHash("isDie");
    private int isHit = Animator.StringToHash("isHit");
    private int Monster_Attack = Animator.StringToHash("Monster_Attack");

    private bool isAttackComplete = false;

    private RegularMonsterSO regularMonster;
    private BossMonsterSO bossMonster;
    private SpriteRenderer effectSprite;
    private AnimatorOverrideController overrideController;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void EnsureComponents()
    {
        base.EnsureComponents();
    }

    public void SetInitMonsterAnimation(MonsterSO monsterData)
    {
        if (monsterData is RegularMonsterSO regularMonsterData)
        {
            regularMonster = regularMonsterData;
            overrideController = regularMonsterData.animatorOverrideController;
            Animator.runtimeAnimatorController = overrideController;
        }
        else if(monsterData is BossMonsterSO bossMonsterData)
        {
            bossMonster = bossMonsterData;
            overrideController = bossMonsterData.animatorOverrideController;
            Animator.runtimeAnimatorController = overrideController;
        }
    }

    #region 애니메이션 파라미터

    public void OnIdle()
    {
        Animator.SetBool(isRun, false);
    }

    public void OnMove(bool isMove)
    {
        Animator.SetBool(isRun, isMove); 
    }

    // RegluarMonster AttackAnimation
    public void OnAttack(bool isAttack)
    {
        AnimationClip selectClip;
        float randomValue = Random.Range(0f, 100f);
        if (randomValue <= regularMonster.patternAttackChance && regularMonster.pattrenAttackClip.Length > 0)
        {
            int randomIndex = Random.Range(0, regularMonster.pattrenAttackClip.Length);
            selectClip = regularMonster.pattrenAttackClip[randomIndex];
            Debug.Log($"{selectClip.name}");
        }
        else
        {
            selectClip = regularMonster.defalutAttackClip;
        }
        overrideController["Monster_Attack"] = selectClip;
        Animator.SetBool(Attack, isAttack);
    }

    // BossMonster AttackAnimation
    public void BossDefalutAttack(bool isAttacking)
    {
        Animator.SetBool(Attack, isAttacking);
    }

    public void OnHit()
    {
        Animator.SetTrigger(isHit);
    }

    public void OnDie()
    {
        Animator.SetTrigger(isDie);
    }

    public void AttackToHold()
    {
        Animator.SetFloat("Blend", Time.deltaTime * 1f);
    }

    #endregion

    // 공격모션 끝났는지 판단 후 대기모션
    public bool OnAttackComplete()
    {
        isAttackComplete = true;
        if(isAttackComplete)
        {
            Animator.SetTrigger("Hold");

            return true;
        }
        return false;
    }
}
