using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RewardPotionNPC : RewardNPC, IInteractable
{
    [SerializeField] private PotionSO potionData;

    //오브젝트풀
    private IObjectPool<PooledPotion> potion;
    private PooledPotion pooledPotion;

    public override void InitNPC()
    {
        base.InitNPC();
        potion = PoolManager.Instance.GetObjectFromPool<PooledPotion>(PoolType.PooledPotion);
        SetReward();
    }

    public override void SetReward()
    {
        ItemSO itemData = ItemManager.Instance.GetItemData(selectNum: 1, ItemType.Potion)[0];
        if (itemData is PotionSO setItemData)
        {
            potionData = setItemData;
        }
        randomNum = Random.Range(0, spawnPositions.Count);
        pooledReward = reward.Get();
        pooledReward.PlayParticle();
        pooledReward.gameObject.transform.position = spawnPositions[randomNum].position;
        pooledReward.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer);
        spriteRenderer.sprite = potionData.rewardSprite;
    }

    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            GetPotionFormReward();
        }
    }

    private void GetPotionFormReward()
    {
        if (potionData != null)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            pooledPotion = potion.Get();
            pooledPotion.gameObject.transform.position = spawnPositions[randomNum].position;
            pooledReward.GetItem(pooledPotion.gameObject);
            pooledPotion.SetPotionSO(potionData);
            pooledPotion.EnsureComponents();
        }
    }

    public override void ReleaseReward()
    {
        base.ReleaseReward();
        if (pooledPotion != null && pooledPotion.gameObject.activeSelf)
        {
            pooledPotion.PoolRelease();
        }
    }
}