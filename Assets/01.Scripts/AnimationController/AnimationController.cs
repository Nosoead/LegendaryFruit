using UnityEngine;

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
