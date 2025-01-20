using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParticleController : ParticleController
{
    [SerializeField] private MonsterStatManager monsterStatManager;
    [SerializeField] private ParticleSystem dieParticlePeel;
    [SerializeField] private ParticleSystem dieParticleSeed;

    private void Awake()
    {

        EnsureComponents();
    }

    private void OnEnable()
    {
        monsterStatManager.DamageTakenEvent += OnDamageReceived;
        monsterStatManager.OnDieEvent += DieParticlePeelPlay;
        monsterStatManager.OnDieEvent += DieParticleSeedPlay;
    }
    private void OnDisable()
    {
        monsterStatManager.DamageTakenEvent -= OnDamageReceived;
        monsterStatManager.OnDieEvent -= DieParticlePeelPlay;
        monsterStatManager.OnDieEvent -= DieParticleSeedPlay;
    }

    private void EnsureComponents()
    {
        if (monsterStatManager == null)
        {
            monsterStatManager = GetComponent<MonsterStatManager>();
        }
        if (particle == null)
        {
            particle = GetComponentInChildren<ParticleSystem>();
        }
    }

    private void DieParticlePeelPlay()
    {
        dieParticlePeel.Play();
    }

    private void DieParticleSeedPlay()
    {
        dieParticleSeed.gameObject.SetActive(true);
        dieParticleSeed.Play();
    }
}
