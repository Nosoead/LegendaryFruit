using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponSO weaponData;
    [SerializeField] private SpriteRenderer sprite;

    public void Start()
    {
        this.sprite.sprite = weaponData.weaponSprite;
    }
}
