using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public abstract class AnimationController : MonoBehaviour
{
    protected Animator Animator;
    protected SpriteRenderer Sprite;

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
