using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponSO weaponData;
    [SerializeField] private SpriteRenderer sprite;
    public Ease ease;

    public void Start()
    {
        this.sprite.sprite = weaponData.weaponSprite;
    }
    public void SetWeaponPosition(Vector2 position)
    {
        this.transform.position = position;
    }
}
