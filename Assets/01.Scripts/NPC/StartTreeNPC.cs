using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class StartTreeNPC : RewardNPC, IInteractable
{
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private CurrencySO currencyData;
    private UpgradeDataContainer upgradeData;
    private int[] randomPosition = new int[2];
    private PooledReward[] pooledRewards = new PooledReward[2];

    //오브젝트풀
    private IObjectPool<PooledFruitWeapon> fruitWeapon;
    private IObjectPool<PooledCurrency> currency;
    private PooledFruitWeapon pooledWeapon;
    private PooledCurrency pooledCurrency;

    public override void InitNPC()
    {
        base.InitNPC();
        upgradeData = DataManager.Instance.LoadData<UpgradeDataContainer>();
        fruitWeapon = PoolManager.Instance.GetObjectFromPool<PooledFruitWeapon>(PoolType.PooledFruitWeapon);
        currency = PoolManager.Instance.GetObjectFromPool<PooledCurrency>(PoolType.PooledCurrency);
        SetReward();
    }

    #region /SetReward
    public override void SetReward()
    {
        randomPosition = RandomNumber(2, spawnPositions.Count);
        if (upgradeData.countUpgrade == 0)
        {
            return;
        }
        else if (upgradeData.countUpgrade == 1)
        {
            int randomNum = Random.Range(0, 2);
            if (randomNum == 0)
            {
                return;
            }
            else if (randomNum == 1)
            {
                SetCurrency(spawnPositions[randomPosition[0]].position);
            }
        }
        else if (upgradeData.countUpgrade == 2)
        {
            SetCurrency(spawnPositions[randomPosition[0]].position);
        }
        else if (upgradeData.countUpgrade == 3)
        {
            int randomNum = Random.Range(0, 2);
            if (randomNum == 0)
            {
                SetCurrency(spawnPositions[randomPosition[0]].position);
            }
            else if (randomNum == 1)
            {
                SetCurrency(spawnPositions[randomPosition[0]].position);
                SetWeapon(spawnPositions[randomPosition[1]].position);
            }
        }
        else if (upgradeData.countUpgrade == 4)
        {
            SetCurrency(spawnPositions[randomPosition[0]].position);
            SetWeapon(spawnPositions[randomPosition[1]].position);
        }
    }

    private void SetCurrency(Vector3 position)
    {
        ItemSO itemData = ItemManager.Instance.GetItemData(selectNum: 1, ItemType.Currency)[0];
        if (itemData is CurrencySO setItemData)
        {
            currencyData = setItemData;
        }
        pooledRewards[0] = reward.Get();
        pooledRewards[0].gameObject.transform.position = position;
        pooledRewards[0].gameObject.TryGetComponent(out SpriteRenderer spriteRenderer);
        spriteRenderer.sprite = currencyData.rewardSprite;
    }

    private void SetWeapon(Vector3 position)
    {
        ItemSO itemData = ItemManager.Instance.GetItemData(selectNum: 1, ItemType.Weapon)[0];
        if (itemData is WeaponSO setItemData)
        {
            weaponData = setItemData;
        }
        pooledRewards[1] = reward.Get();
        pooledRewards[1].gameObject.transform.position = position;
        pooledRewards[1].gameObject.TryGetComponent(out SpriteRenderer spriteRenderer);
        spriteRenderer.sprite = weaponData.rewardSprite;
    }

    private int[] RandomNumber(int selectNum, int maxNum)
    {
        int[] resultNum = new int[selectNum];
        int[] numberArray = new int[maxNum];
        for (int i = 0; i < numberArray.Length; i++)
        {
            numberArray[i] = i;
        }

        for (int i = 0; i < selectNum; i++)
        {
            int randomNum = Random.Range(0, maxNum);
            resultNum[i] = numberArray[randomNum];
            numberArray[randomNum] = numberArray[numberArray.Length - i - 1];
        }
        return resultNum;
    }
    #endregion

    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            GetItemFormReward();
        }
    }

    private void GetItemFormReward()
    {
        if (currencyData != null)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            pooledCurrency = currency.Get();
            pooledCurrency.gameObject.transform.position = spawnPositions[randomPosition[0]].position;
            pooledRewards[0].GetItem(pooledCurrency.gameObject);
            pooledCurrency.SetCurrencySO(currencyData);
            pooledCurrency.EnsureComponents();
        }

        if (weaponData != null)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            pooledWeapon = fruitWeapon.Get();
            pooledWeapon.gameObject.transform.position = spawnPositions[randomPosition[1]].position;
            pooledRewards[1].GetItem(pooledWeapon.gameObject);
            pooledWeapon.SetWeaponSO(weaponData);
            pooledWeapon.EnsureComponents();
        }
    }

    public override void ReleaseReward()
    {
        base.ReleaseReward();
        if (pooledWeapon != null && pooledWeapon.gameObject.activeSelf)
        {
            pooledWeapon.PoolRelease();
        }
        if (pooledCurrency != null && pooledCurrency.gameObject.activeSelf)
        {
            pooledCurrency.PoolRelease();
        }
    }
}
