using UnityEngine;

public class Item : MonoBehaviour
{
    public SpriteRenderer itemSprite;

    protected void Awake()
    {
        EnsureComponents();
    }

    public virtual void EnsureComponents()
    {
        itemSprite = GetComponent<SpriteRenderer>();
    }
}