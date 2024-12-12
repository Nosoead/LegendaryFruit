using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FruitWeapon : Weapon, IInteractable
{
    public WeaponSO weaponData;
    private SpriteRenderer weaponSprite;


    private void Awake()
    {
        EnsureComponents();
    }

    private void EnsureComponents()
    {
        weaponSprite = GetComponent<SpriteRenderer>();
        if (weaponData != null)
        {
            weaponSprite.sprite = weaponData.weaponSprite;
        }
    }

    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed)
        {
            FruitEat();
            return;
        }
        else if (isPressed)
        {
            FruitEquip();
            return;
        }
    }

    private void FruitEat()
    {
        gameObject.SetActive(false);
    }

    private void FruitEquip()
    {
        gameObject.SetActive(false);
    }
}
