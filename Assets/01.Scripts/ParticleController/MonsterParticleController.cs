using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParticleController : ParticleController
{
    [SerializeField] private MonsterStatManager monsterStatManager;
    [SerializeField] private ParticleSystem dieParticle;

    private void Awake()
    {
        
        EnsureComponents();
    }

    private void OnEnable()
    {
        monsterStatManager.DamageTakenEvent += OnDamageReceived;
        //monsterStatManager.OnDieEvent += DieParticlePlay;
    }
    private void OnDisable()
    {
        monsterStatManager.DamageTakenEvent -= OnDamageReceived;
        //monsterStatManager.OnDieEvent -= DieParticlePlay;
    }

    private void EnsureComponents()
    {
        if(monsterStatManager == null)
        {
            monsterStatManager = GetComponent<MonsterStatManager>();    
        }
        if(particle == null)
        {
            particle = GetComponentInChildren<ParticleSystem>();
        }
    }

    private void DieParticlePlay()
    {     
        dieParticle.Play();
    }
}
