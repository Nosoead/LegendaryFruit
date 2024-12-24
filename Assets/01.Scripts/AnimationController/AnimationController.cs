using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public abstract class AnimationController : MonoBehaviour
{
    public Animator Animator;
    public SpriteRenderer Sprite;

    protected virtual void Awake()
    {
        EnsureComponents();

    }

    protected virtual void EnsureComponents()
    {
        if (Animator == null)
        {
            Animator = GetComponent<Animator>();
        }
        if (Sprite == null)
        {
            Sprite = GetComponent<SpriteRenderer>();
        }
    }
}
