using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class PlayerEquipment : MonoBehaviour
{
    public UnityAction<WeaponSO> OnEquipWeaponChanged;
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private PlayerInteraction interaction;
    [SerializeField] private SpriteRenderer weaponSprite;
    [SerializeField] private PlayerController controller;
    private IObjectPool<PooledFruitWeapon> fruitWeapon;
    private WeaponSO startingWeaponData;

    private List<WeaponSO> equipWeapons = new List<WeaponSO>();
    private int maxWeaponCapacity = 2;
    private int currentEquipWeaponIndex = 0;

    private void Awake()
    {
        EnsureComponents();
    }

    private void OnEnable()
    {
        controller.OnSwapWeaponEvent += OnSwapWeaponEvent;
        interaction.FruitWeapOnEquipEvent += OnWeaponEquipEvent;
    }

    private void OnDisable()
    {
        controller.OnSwapWeaponEvent -= OnSwapWeaponEvent;
        interaction.FruitWeapOnEquipEvent -= OnWeaponEquipEvent;
    }

    private void Start()
    {
        Init();
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
        if (controller == null)
        {
            controller = GetComponentInParent<PlayerController>();
        }
    }

    private void Init()
    {
        CacheItem();
        if (!DataManager.Instance.GetCanLoad<SaveDataContainer>())
        {
            OnWeaponEquipEvent(weaponData);
        }
    }

    private void CacheItem()
    {
        fruitWeapon = PoolManager.Instance.GetObjectFromPool<PooledFruitWeapon>(PoolType.PooledFruitWeapon);
    }

    private void OnSwapWeaponEvent()
    {
        if (equipWeapons.Count < 2)
        {
            currentEquipWeaponIndex = 0;
            return;
        }
        currentEquipWeaponIndex = (currentEquipWeaponIndex + 1) % equipWeapons.Count;
        UpdateWeaponSprite();
    }

    private void OnWeaponEquipEvent(WeaponSO weaponData)
    {

        if (equipWeapons.Count < maxWeaponCapacity)
        {
            equipWeapons.Add(weaponData);
            currentEquipWeaponIndex = equipWeapons.Count-1;
        }
        else
        {
            ReplaceWeapon(weaponData);
        }
        UpdateWeaponSprite();
    }

    private void ReplaceWeapon(WeaponSO weaponData)
    {
        PooledFruitWeapon discardedObject = fruitWeapon.Get();
        discardedObject.gameObject.transform.position = transform.parent.position;
        discardedObject.gameObject.layer = LayerMask.NameToLayer("Default");
        DropDotween(discardedObject.gameObject, transform.parent.position);
        discardedObject.SetWeaponSO(equipWeapons[currentEquipWeaponIndex]);
        discardedObject.EnsureComponents();
        equipWeapons[currentEquipWeaponIndex] = weaponData;
    }

    private void UpdateWeaponSprite()
    {
        weaponSprite.sprite = equipWeapons[currentEquipWeaponIndex].weaponSprite;
        OnEquipWeaponChanged?.Invoke(equipWeapons[currentEquipWeaponIndex]);
    }

    public (List<WeaponSO>, int) SaveEquipmentData()
    {
        return (equipWeapons, currentEquipWeaponIndex);
    }

    public void LoadEquipmentData(List<WeaponSO> weaponDataList, int currentEquipWeaponIndex)
    {
        equipWeapons = weaponDataList;
        this.currentEquipWeaponIndex = currentEquipWeaponIndex;
        UpdateWeaponSprite();
    }

    public void DeleteEquipmentData()
    {
        equipWeapons.Clear();
        currentEquipWeaponIndex = 0;
    }

    public WeaponSO GetCurrentEquipData()
    {
        return equipWeapons[currentEquipWeaponIndex];
    }

    public void SetUpgradeData(WeaponSO weaponData)
    {
        equipWeapons[currentEquipWeaponIndex] = weaponData;
        UpdateWeaponSprite();
    }

    #region /DropDotween
    private void DropDotween(GameObject go, Vector3 position)
    {
        RaycastHit2D blockHit = Physics2D.Raycast(go.transform.position, Vector2.down, 50, LayerMask.GetMask("Block"));
        RaycastHit2D groundHit = Physics2D.Raycast(go.transform.position, Vector2.down, 50, LayerMask.GetMask("Ground"));
        RaycastHit2D closestHit;

        //가까운 히트
        if (blockHit.collider != null && groundHit.collider != null)
        {
            closestHit = blockHit.distance < groundHit.distance ? blockHit : groundHit;
        }
        else if (blockHit.collider != null)
        {
            closestHit = blockHit;
        }
        else if (groundHit.collider != null)
        {
            closestHit = groundHit;
        }
        else
        {
            return;
        }
        float hitPosY = closestHit.point.y;
        float goalPos = hitPosY + 1f;
        if (closestHit.collider != null)
        {
            // 무기가 떨궈짐
            Sequence sequence = DOTween.Sequence();
            sequence.Append(go.transform.DOMoveY(position.y+3.5f, 0.5f, false))
                    .Append(go.transform.DOMoveY(goalPos, 0.5f, false));
            go.gameObject.layer = LayerMask.NameToLayer("Item");
        }
    }
    #endregion
}
