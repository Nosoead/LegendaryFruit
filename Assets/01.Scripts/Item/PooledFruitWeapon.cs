using UnityEngine;
using UnityEngine.Pool;

public class PooledFruitWeapon : Item, IInteractable, ISetPooledObject<PooledFruitWeapon>
{
    public WeaponSO weaponData;
    private CurrencySystem currencySystem;

    [SerializeField] private ParticleSystem eatParticle;
    protected IObjectPool<PooledFruitWeapon> objectPool;
    public IObjectPool<PooledFruitWeapon> ObjectPool
    { get => objectPool; set => objectPool = value; }

    public override void EnsureComponents()
    {
        base.EnsureComponents();
        if (weaponData != null)
        {
            itemSprite.color = new Color(1f, 1f, 1f, 1f);
            itemSprite.sprite = weaponData.weaponSprite;
        }
        if (currencySystem == null)
        {
            if (GameManager.Instance.player.gameObject.TryGetComponent(out CurrencySystem currencySys))
            {
                currencySystem = currencySys;
            }
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
        currencySystem.GetCurrency(1, isGlobalCurrency: true);
        eatParticle.Play();
        itemSprite.color = new Color(1f, 1f, 1f, 0f);
        Invoke(nameof(ReleaseObject), 1.0f);
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

    private void ReleaseObject()
    {
        ObjectPool.Release(this);
    }
}
