using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IInteractable
{
    [SerializeField] public WeaponSO weaponData;
    [SerializeField] private SpriteRenderer sprite;

    public void Start()
    {
        this.sprite.sprite = weaponData.weaponSprite;
    }

    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed)
        {
            Destroy(gameObject);
            return;
        }
        else if (isPressed)
        {
            Destroy(gameObject);
            return;
        }
    }
}
