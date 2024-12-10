using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerController controller;

    private void Awake()
    {
        EnsureComponents();
    }

    //private void OnEnable()
    //{
    //    controller.OnInteractEvent += OnAttackEvent;
    //}

    //private void OnDisable()
    //{
    //    controller.OnInteractEvent -= OnAttackEvent;
    //}

    private void EnsureComponents()
    {
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }
    }

    private void OnAttackEvent()
    {

    }
}
