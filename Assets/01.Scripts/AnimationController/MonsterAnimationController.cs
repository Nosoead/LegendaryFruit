using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class MonsterAnimationController : AnimationController
{
    private int isRun = Animator.StringToHash("isRun");
    private int Attack = Animator.StringToHash("Attack");
    private int isDie = Animator.StringToHash("isDie");
    private int isHit = Animator.StringToHash("isHit");
    private int isArea = Animator.StringToHash("isArea");
    private int Monster_Attack = Animator.StringToHash("Monster_Attack");

    private bool isAttackComplete = false;

    private SpriteRenderer effectSprite;
    private MonsterSO monsterData;
    private AnimatorOverrideController overrideController;

    protected override void Awake()
    {
        base.Awake();
        monsterData = EntityManager.Instance.monsterData;
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
        overrideController = monsterData.animatorOverrideController;
        Animator.runtimeAnimatorController = overrideController;
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

    public void OnAttack(bool isAttack)
    {
        AnimationClip selectClip;
        float randomValue = Random.Range(0f, 100f);
        if(randomValue <= monsterData.patternAttackChance && monsterData.pattrenAttackClip.Length > 0 )
        {
            int randomIndex = Random.Range(0, monsterData.pattrenAttackClip.Length);
            selectClip = monsterData.pattrenAttackClip[randomIndex];
            Debug.Log($"{selectClip.name}");
        }
        else
        {
            selectClip = monsterData.defalutAttackClip;
        }
        overrideController["Monster_Attack"] = selectClip;
        Animator.SetBool(Attack, isAttack);
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
