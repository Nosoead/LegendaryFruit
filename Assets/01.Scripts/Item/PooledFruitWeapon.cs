using UnityEngine;
using UnityEngine.Pool;

public class PooledFruitWeapon : Item, IInteractable, ISetPooledObject<PooledFruitWeapon>
{
    public WeaponSO weaponData;

    protected IObjectPool<PooledFruitWeapon> objectPool;
    public IObjectPool<PooledFruitWeapon> ObjectPool
    { get => objectPool; set => objectPool = value; }

    public override void EnsureComponents()
    {
        base.EnsureComponents();
        if (weaponData != null)
        {
            itemSprite.sprite = weaponData.weaponSprite;
        }
    }

    #region /Interact
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
        ObjectPool.Release(this);
    }

    private void FruitEquip()
    {
        ObjectPool.Release(this);
    }
    #endregion

    #region /ObjectPool
    public void SetPooledObject(IObjectPool<PooledFruitWeapon> pool)
    {
        ObjectPool = pool;
    }

    public void PoolRelease()
    {
        ObjectPool.Release(this);
    }
    #endregion

    public WeaponSO GetWeaponSO()
    {
        return weaponData;
    }

    public void SetWeaponSO(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
    }
}
