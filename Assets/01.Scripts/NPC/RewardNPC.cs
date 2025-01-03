//안내문구
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

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

    //TODO ItemManager에서 랜덤으로 들고올 수 있도록
    [SerializeField] private WeaponSO weaponData;
    //오브젝트풀
    private IObjectPool<PooledFruitWeapon> fruitWeapon;
    private IObjectPool<PooledReward> reward;
    private PooledReward pooledReward;

    public override void InitNPC()
    {
        Debug.Log("test");
        gameObject.layer = LayerMask.NameToLayer("NPC");
        playerLayer = LayerMask.NameToLayer("Player");
        invincibleLayer = LayerMask.NameToLayer("Invincible");
        if (pressFCanvas == null)
        {
            pressFCanvas = GetComponentInChildren<Canvas>();
        }
        pressFCanvas.gameObject.SetActive(false);

        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));
        }
        PoolManager.Instance.CreatePool<PooledFruitWeapon>(PoolType.PooledFruitWeapon, false, 5, 5);
        PoolManager.Instance.CreatePool<PooledReward>(PoolType.PooledReward, false, 5, 5);
        fruitWeapon = PoolManager.Instance.GetObjectFromPool<PooledFruitWeapon>(PoolType.PooledFruitWeapon);
        reward = PoolManager.Instance.GetObjectFromPool<PooledReward>(PoolType.PooledReward);
        //TODO 인덱스접근 자동으로 할 수 있게
    }

    public override void SetReward()
    {
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
        }
    }

    private void GetWeaponFromReward()
    {
        if (weaponData != null)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            PooledFruitWeapon weapon = fruitWeapon.Get();
            weapon.gameObject.transform.position = spawnPositions[randomNum].position;
            pooledReward.GetWeapon(weapon.gameObject);
            weapon.weaponData = this.weaponData;
            weapon.EnsureComponents();
        }
    }
}