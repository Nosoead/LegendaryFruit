using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParticleController : ParticleController
{
    [SerializeField] private MonsterStatManager monsterStatManager;

    private void Awake()
    {
        
        EnsureComponents();
    }

    private void OnEnable()
    {
        monsterStatManager.DamageTakenEvent += OnDamageReceived;
    }
    private void OnDisable()
    {
        monsterStatManager.DamageTakenEvent -= OnDamageReceived;
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
}
