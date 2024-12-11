using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int isLookRight = Animator.StringToHash("isLookRight");
    private Animator weaponAnimator;
    private PlayerController controller;
    private float lookDirection = 1f;

    private void Awake()
    {
        EnsureComponents();
    }

    private void OnEnable()
    {
        controller.OnDirectionEvent += OnDirectionEvent;
        controller.OnAttackEvent += OnAttackEvent;
    }

    private void OnDisable()
    {
        controller.OnDirectionEvent -= OnDirectionEvent;
        controller.OnAttackEvent -= OnAttackEvent;
    }

    private void EnsureComponents()
    {
        if (weaponAnimator == null)
        {
            weaponAnimator = GetComponentInChildren<Animator>();
        }
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }
    }

    private void OnDirectionEvent(float directionValue)
    {
        //lookDirection = directionValue;
        //if (lookDirection > 0)
        //{
        //    weaponAnimator.SetBool(isLookRight, true);
        //}
        //else
        //{
        //    weaponAnimator.SetBool(isLookRight, false);
        //}
    }

    private void OnAttackEvent()
    {
        weaponAnimator.SetTrigger(Attack);
    }
}
