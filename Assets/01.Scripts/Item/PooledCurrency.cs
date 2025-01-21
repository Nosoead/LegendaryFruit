using UnityEngine;
using UnityEngine.Pool;

public class PooledCurrency : Item, IInteractable, ISetPooledObject<PooledCurrency>
{
    public CurrencySO currencyData;
    private CurrencySystem currencySystem;

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
        if (currencySystem == null)
        {
            if (GameManager.Instance.player.TryGetComponent(out CurrencySystem currencySys))
            {
                currencySystem = currencySys;
            }
        }
    }

    #region /Interact
    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            ConsumItem();
            PoolRelease();
        }
    }

    private void ConsumItem()
    {
        currencySystem.GetCurrency((int)currencyData.currencyValue, isGlobalCurrency: false);
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
