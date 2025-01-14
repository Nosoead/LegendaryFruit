using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

public class RewardCurrencyNPC : RewardNPC, IInteractable
{
    [SerializeField] private CurrencySO currencyData;
    [SerializeField] private Light2D NPCSpotlight;

    //오브젝트풀
    private IObjectPool<PooledCurrency> currency;
    private PooledCurrency pooledCurrency;
    public override void InitNPC()
    {
        base.InitNPC();
        if (NPCSpotlight == null)
        {
            NPCSpotlight = GetComponentInChildren<Light2D>();
        }
        SetSpotlight(isTurnOn: false);
        currency = PoolManager.Instance.GetObjectFromPool<PooledCurrency>(PoolType.PooledCurrency);
    }

    public override void SetReward()
    {
        SetSpotlight(isTurnOn: true);
        ItemSO itemData = ItemManager.Instance.GetItemData(selectNum: 1, ItemType.Currency)[0];
        if (itemData is CurrencySO setItemData)
        {
            currencyData = setItemData;
        }
        randomNum = Random.Range(0, spawnPositions.Count);
        pooledReward = reward.Get();
        pooledReward.gameObject.transform.position = spawnPositions[randomNum].position;
        pooledReward.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer);
        spriteRenderer.sprite = currencyData.rewardSprite;
    }

    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if ((isDeepPressed || isPressed) && StageManager.Instance.GetStageClear())
        {
            GetCurrencyFromReward();
            GameManager.Instance.SetGameClear(true);
        }
    }

    private void GetCurrencyFromReward()
    {
        if (currencyData != null)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            pooledCurrency = currency.Get();
            pooledCurrency.gameObject.transform.position = spawnPositions[randomNum].position;
            pooledReward.GetItem(pooledCurrency.gameObject);
            pooledCurrency.SetCurrencySO(currencyData);
            pooledCurrency.EnsureComponents();
        }
    }

    private void SetSpotlight(bool isTurnOn)
    {
        if (isTurnOn)
        {
            NPCSpotlight.gameObject.SetActive(true);
        }
        else
        {
            NPCSpotlight.gameObject.SetActive(false);
        }
    }

    public override void ReleaseReward()
    {
        base.ReleaseReward();
        if (pooledCurrency != null && pooledCurrency.gameObject.activeSelf)
        {
            pooledCurrency.PoolRelease();
        }
    }
}
