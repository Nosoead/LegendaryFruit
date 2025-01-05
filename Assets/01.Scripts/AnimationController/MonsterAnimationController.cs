using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MonsterAnimationController : AnimationController
{
    private readonly int PatternAttack = Animator.StringToHash("PatternAttack");
    private readonly int isRun = Animator.StringToHash("isRun");
    private readonly int Attack = Animator.StringToHash("Attack");
    private readonly int isDie = Animator.StringToHash("isDie");
    private readonly int isHit = Animator.StringToHash("isHit");

    private Dictionary<int, PatternData> pattrens = new Dictionary<int, PatternData>();
    private bool isAttackComplete = false;

    private AnimationClip currentPatternAnimaion;
    private RegularMonsterSO regularMonster;
    private BossMonsterSO bossMonster;
    private SpriteRenderer effectSprite;
    private AnimatorOverrideController overrideController;

    [SerializeField] private MonsterStatManager monsterStatManager;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private ParticleSystem[] numberParticle;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        monsterStatManager.DamageTakenEvent += OnDamageReceived;
        monsterStatManager.DamageTakenEvent += OnHitParticlePlay;
    }
    private void OnDisable()
    {
        monsterStatManager.DamageTakenEvent -= OnDamageReceived;
        monsterStatManager.DamageTakenEvent -= OnHitParticlePlay;
    }

    protected override void EnsureComponents()
    {
        base.EnsureComponents();
        if(effectSprite == null )
        {
            effectSprite = GetComponentInChildren<SpriteRenderer>();
        }
    }

    // Animation Init
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

    public void SetPatternAnimation(Dictionary<int,PatternData> patternData)
    {
        pattrens = patternData;
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

    public void OnTakeHit()
    {
        Animator.SetTrigger(isHit);
    }

    public void OnDie()
    {
        Animator.SetTrigger(isDie);
    }

    #endregion

    #region RegularMonster AttackAnimations
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
    #endregion

    #region BossMonster AttackAnimations
    // BossMonster DefalutAttack Animation

    public AnimationClip SetPatternAnimationData(PatternData data)
    {       
        return currentPatternAnimaion = data.pattrenAttackAnimation;
    }

    public void BossDefalutAttack(bool isAttacking)
    {
        Animator.SetBool(Attack, isAttacking);
    }
    
    public void BossPatternAttack(bool isTrigger)
    {
        var clip = currentPatternAnimaion;
        overrideController["Boss_PatternAttack"] = clip;
        if(isTrigger )
        {
            Animator.SetTrigger(PatternAttack);
        }
        else
        {
            Animator.ResetTrigger(PatternAttack);
        }
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

    public bool HasPatternAttackFinished()
    {
        var state = Animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_PatternAttack");
        float animationTime = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if(state && animationTime >= 1)
        {
            return true;
        }
        return false;
    }

    public void OnHitParticlePlay(float damage)
    {
        if(!particle.isPlaying)
        {
            //ParticleDamage();
            particle.Play();
        }
        else
        {
            particle.Stop();
        }
    }

    public void OnDamageReceived(float damage)
    {
        string str = damage.ToString();
        Debug.Log(str);
    }
}
