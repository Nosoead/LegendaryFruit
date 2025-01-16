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
        if (playerStatManager == null)
        {
            playerStatManager = GetComponent<PlayerStatManager>();
        }
    }


    private void OnEnable()
    {
        playerStatManager.DamageTakenEvent += OnDamageReceived;
        playerStatManager.OnHealEvent += OnDamageReceived;
    }
    private void OnDisable()
    {
        playerStatManager.DamageTakenEvent -= OnDamageReceived;
        playerStatManager.OnHealEvent -= OnDamageReceived;
    }
}
