using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

public class RewardNPC : NPC, IInteractable
{
    //F키 누르세요
    [SerializeField]
    private Canvas pressFCanvas;
    private int playerLayer;
    private int invincibleLayer;

    //스폰포인트
    [SerializeField] private Transform spawnPositionsRoot;
    private List<Transform> spawnPositions = new List<Transform>();
    private int randomNum;

    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private Light2D NPCSpotlight;

    //오브젝트풀
    private IObjectPool<PooledFruitWeapon> fruitWeapon;
    private IObjectPool<PooledReward> reward;
    private PooledReward pooledReward;
    private PooledFruitWeapon pooledWeapon;

    public override void InitNPC()
    {
        gameObject.layer = LayerMask.NameToLayer("NPC");
        playerLayer = LayerMask.NameToLayer("Player");
        invincibleLayer = LayerMask.NameToLayer("Invincible");
        if (pressFCanvas == null)
        {
            pressFCanvas = GetComponentInChildren<Canvas>();
        }
        pressFCanvas.gameObject.SetActive(false);
        if (NPCSpotlight == null)
        {
            NPCSpotlight = GetComponentInChildren<Light2D>();
        }
        SetSpotlight(isTurnOn : false);
        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));
        }
        PoolManager.Instance.CreatePool<PooledFruitWeapon>(PoolType.PooledFruitWeapon, false, 5, 5);
        PoolManager.Instance.CreatePool<PooledReward>(PoolType.PooledReward, false, 5, 5);
        fruitWeapon = PoolManager.Instance.GetObjectFromPool<PooledFruitWeapon>(PoolType.PooledFruitWeapon);
        reward = PoolManager.Instance.GetObjectFromPool<PooledReward>(PoolType.PooledReward);
    }

    public override void SetReward()
    {
        SetSpotlight(isTurnOn : true);
        weaponData = ItemManager.Instance.GetItemData(selectNum: 1)[0];
        randomNum = Random.Range(0, spawnPositions.Count);
        pooledReward = reward.Get();
        pooledReward.gameObject.transform.position = spawnPositions[randomNum].position;
        pooledReward.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer);
        spriteRenderer.sprite = weaponData.rewardSprite;
    }

    #region /TogglePrompt
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer || collision.gameObject.layer == invincibleLayer)
        {
            TogglePrompt(isOpen: true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer || collision.gameObject.layer == invincibleLayer)
        {
            TogglePrompt(isOpen: false);
        }
    }

    private void TogglePrompt(bool isOpen)
    {
        if (isOpen)
        {
            pressFCanvas.gameObject.SetActive(isOpen);
        }
        else
        {
            pressFCanvas.gameObject.SetActive(isOpen);
        }
    }
    #endregion

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
            pooledReward.GetWeapon(pooledWeapon.gameObject);
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
        if (pooledReward != null && pooledReward.gameObject.activeSelf)
        {
            pooledReward.PoolRelease();
        }

        if (pooledWeapon != null && pooledWeapon.gameObject.activeSelf)
        {
            pooledWeapon.PoolRelease();
        }
    }
}