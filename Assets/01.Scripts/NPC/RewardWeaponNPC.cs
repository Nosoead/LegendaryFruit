using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

public class RewardWeaponNPC : RewardNPC, IInteractable
{
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private Light2D NPCSpotlight;

    //오브젝트풀
    private IObjectPool<PooledFruitWeapon> fruitWeapon;
    private PooledFruitWeapon pooledWeapon;

    public override void InitNPC()
    {
        base.InitNPC();
        if (NPCSpotlight == null)
        {
            NPCSpotlight = GetComponentInChildren<Light2D>();
        }
        SetSpotlight(isTurnOn: false);
        fruitWeapon = PoolManager.Instance.GetObjectFromPool<PooledFruitWeapon>(PoolType.PooledFruitWeapon);
    }

    public override void SetReward()
    {
        SetSpotlight(isTurnOn: true);
        ItemSO itemData = ItemManager.Instance.GetItemData(selectNum: 1, ItemType.Weapon)[0];
        if (itemData is WeaponSO setItemData)
        {
            weaponData = setItemData;
        }
        randomNum = Random.Range(0, spawnPositions.Count);
        pooledReward = reward.Get();
        pooledReward.gameObject.transform.position = spawnPositions[randomNum].position;
        pooledReward.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer);
        spriteRenderer.sprite = weaponData.rewardSprite;
    }

    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            GetWeaponFromReward();
            GameManager.Instance.SetGameClear(true);
        }
    }

    private void GetWeaponFromReward()
    {
        if (weaponData != null)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            pooledWeapon = fruitWeapon.Get();
            pooledWeapon.gameObject.transform.position = spawnPositions[randomNum].position;
            pooledReward.GetItem(pooledWeapon.gameObject);
            pooledWeapon.SetWeaponSO(weaponData);
            pooledWeapon.EnsureComponents();
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
        if (pooledWeapon != null && pooledWeapon.gameObject.activeSelf)
        {
            pooledWeapon.PoolRelease();
        }
    }
}