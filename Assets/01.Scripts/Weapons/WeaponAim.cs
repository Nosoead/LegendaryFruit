using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weaponSprite;
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private PlayerController controller;
    [SerializeField] private Animator animator;
    private float lookDirection = 1;
    private bool isLookRight = true;

    private void Awake()
    {
        EnsureComponents();
    }

    private void OnEnable()
    {
        controller.OnDirectionEvent += OnDirectionEvent;
    }

    private void OnDisable()
    {
        controller.OnDirectionEvent -= OnDirectionEvent;
    }

    private void Update()
    {
        if (lookDirection > 0 && !isLookRight)
        {
            FlipSprite();
        }
        else if(lookDirection < 0 && isLookRight)
        {
            FlipSprite();
        }
    }

    private void EnsureComponents()
    {
        if (weaponSprite == null)
        {
            weaponSprite = GetComponentInChildren<SpriteRenderer>();
        }
        if (weaponTransform == null)
        {
            weaponTransform = GetComponentInChildren<Transform>();
        }
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

    }

    private void OnDirectionEvent(float directionValue)
    {
        lookDirection = directionValue;
    }

    private void FlipSprite()
    {
        isLookRight = !isLookRight;

        weaponSprite.flipX = !weaponSprite.flipX;

        Vector3 weaponPosition = weaponTransform.localPosition;
        weaponPosition.x*=-1;
        weaponTransform.localPosition = weaponPosition;
        Debug.Log(weaponTransform.localPosition);

        Vector3 weaponRotation = weaponTransform.localEulerAngles;
        weaponRotation.z = -weaponRotation.z;
        weaponTransform.localEulerAngles = weaponRotation;
    }
}
