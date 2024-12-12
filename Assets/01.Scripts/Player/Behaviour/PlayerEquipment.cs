using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEquipment : MonoBehaviour
{
    //TODO 최대 2개 보관
    public UnityAction<WeaponSO> OnEquipWeaponChanged;

    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private PlayerInteraction interaction;
    [SerializeField] private SpriteRenderer weaponSprite;

    private void Awake()
    {
        EnsureComponents();
    }

    private void OnEnable()
    {
        interaction.FruitWeapOnEquipEvent += OnWeaponEquipEvent;
    }

    private void OnDisable()
    {
        interaction.FruitWeapOnEquipEvent -= OnWeaponEquipEvent;
    }

    private void Start()
    {
        OnWeaponEquipEvent(weaponData);
    }

    private void EnsureComponents()
    {
        if (interaction == null)
        {
            interaction = GetComponentInParent<PlayerInteraction>();
        }
        if (weaponSprite == null)
        {
            weaponSprite = GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void OnWeaponEquipEvent(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
        weaponSprite.sprite = weaponData.weaponSprite;
        OnEquipWeaponChanged?.Invoke(weaponData);
    }

}
