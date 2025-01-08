using System;
using UnityEngine;

public class PlayerParticleController : ParticleController
{
    [SerializeField] private PlayerStatManager playerStatManager;

    private void Awake()
    {
        EnsureComponents();
    }

    private void EnsureComponents()
    {
        if(playerStatManager == null)
        {
            playerStatManager = GetComponent<PlayerStatManager>();
        }
        if(particle == null)
        {
            //particle = GetComponent<>
        }
    }


    private void OnEnable()
    {
        playerStatManager.DamageTakenEvent += OnDamageReceived;
        playerStatManager.DamageTakenEvent += OnHitParticlePlay;
    }
    private void OnDisable()
    {
        playerStatManager.DamageTakenEvent -= OnDamageReceived;
        playerStatManager.DamageTakenEvent -= OnHitParticlePlay;
    }
}
