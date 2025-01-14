using UnityEngine;
using UnityEngine.Pool;

public class PooledPotion : Item, IInteractable, ISetPooledObject<PooledPotion>
{
    public PotionSO potionData;
    private PlayerStatManager stat;

    protected IObjectPool<PooledPotion> objectPool;
    public IObjectPool<PooledPotion> ObjectPool
    { get => objectPool; set => objectPool = value; }

    public override void EnsureComponents()
    {
        base.EnsureComponents();
        if (potionData != null)
        {
            itemSprite.sprite = potionData.potionSprite;
        }
        stat = GameManager.Instance.player.GetComponent<PlayerStatManager>();
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
        stat.Heal(potionData.potionValue);
    }
    #endregion

    #region /ObjectPool
    public void SetPooledObject(IObjectPool<PooledPotion> pool)
    {
        ObjectPool = pool;
    }

    public void PoolRelease()
    {
        ObjectPool.Release(this);
    }
    #endregion

    public PotionSO GetPotionSO()
    {
        return potionData;
    }

    public void SetPotionSO(PotionSO potionData)
    {
        this.potionData = potionData;
    }
}
