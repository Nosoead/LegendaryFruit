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
        }
        else
        {
            selectClip = monsterData.defalutAttackClip;
        }
        overrideController["Monster_Attack"] = selectClip;
        Animator.SetBool(Attack, isAttack);
    }



    public bool CheckAttackAnimationState()
    {
        var currentState = Animator.GetCurrentAnimatorStateInfo(0).IsName("Monster_Attack");
        var isAnimating = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime; 
        if(currentState && isAnimating  >= 1f)
        {
            Debug.Log($"{isAnimating}");
            return true;
        }
        return false;
    }

    public void ResetAttackTrigger()
    {
        Animator.ResetTrigger(Attack);
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

    public void OffHold()
    {
        Animator.SetBool("Hold", false);
        Animator.SetBool(Attack, true);
    }

    public bool OnAttackComplete()
    {
        isAttackComplete = true;
        if(isAttackComplete)
        {
            Animator.SetBool("Hold", true);
            Animator.SetBool(Attack, false);
            Debug.Log("공격완료 ! Hold로 넘어갑니다.");
            return true;
        }
        return false;
    }
}
