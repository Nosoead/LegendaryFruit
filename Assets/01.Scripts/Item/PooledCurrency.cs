using UnityEngine;
using UnityEngine.Pool;

public class PooledCurrency : Item, IInteractable, ISetPooledObject<PooledCurrency>
{
    public CurrencySO currencyData;

    protected IObjectPool<PooledCurrency> objectPool;
    public IObjectPool<PooledCurrency> ObjectPool
    { get => objectPool; set => objectPool = value; }

    public override void EnsureComponents()
    {
        base.EnsureComponents();
        if (currencyData != null)
        {
            itemSprite.sprite = currencyData.currencySprite;
        }
    }

    #region /Interact
    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            PoolRelease();
        }
    }
    #endregion

    #region /ObjectPool
    public void SetPooledObject(IObjectPool<PooledCurrency> pool)
    {
        ObjectPool = pool;
    }

    public void PoolRelease()
    {
        ObjectPool.Release(this);
    }
    #endregion

    public CurrencySO GetCurrencySO()
    {
        return currencyData;
    }

    public void SetCurrencySO(CurrencySO currencyData)
    {
        this.currencyData = currencyData;
    }
}
