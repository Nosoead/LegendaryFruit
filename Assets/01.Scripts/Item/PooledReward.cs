using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
using UnityEngine.Rendering;

public class PooledReward : MonoBehaviour, ISetPooledObject<PooledReward>
{
    public WeaponSO weaponData = null;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PooledFruitWeapon weaponPrefab;

    private IObjectPool<PooledReward> objectPool;

    public IObjectPool<PooledReward> ObjectPool
    {
        get => objectPool;
        set => objectPool = value;
    }

    public void SetPooledObject(IObjectPool<PooledReward> pool)
    {
        objectPool = pool;
    }

    // 물체 위치를 설정합니다.
    /// <summary>
    /// 물체의 위치값을 설정
    /// </summary>
    /// <param name="position">설정할 위치값</param>
    public void SetPosition(Vector2 position)
    {
        // 위치 설정
        this.transform.position = position;
    }

    // RewardData를 추가
    /// <summary>
    /// 보상에 어떤 WeaponData를 줄지 결정합니다.
    /// </summary>
    /// <param name="weaponData">보상에 사용할 WeaponData</param>
    public void SetRewardData(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
        spriteRenderer.sprite = this.weaponData.rewardSprite;
        DataToObject();
    }

    private void DataToObject()
    {
        weaponPrefab.SetWeaponSO(weaponData);
    }

    // TODO: DOTween 효과 추가
    public Ease ease;

    public void GetWeapon(GameObject go)
    {
        ObjectPool.Release(this);
        RaycastHit2D blockHit = Physics2D.Raycast(go.transform.position, Vector2.down, 50, LayerMask.GetMask("Block"));
        RaycastHit2D groundHit = Physics2D.Raycast(go.transform.position,Vector2.down, 50, LayerMask.GetMask("Ground"));
        RaycastHit2D closestHit;

        if (blockHit.collider != null && groundHit.collider != null)
        {
            closestHit = blockHit.distance < groundHit.distance ? blockHit : groundHit;
        }
        else if (blockHit.collider != null)
        {
            closestHit = blockHit;
        }
        else if (groundHit.collider != null)
        {
            closestHit = groundHit;
        }
        else
        {
            return;
        }

        float hitPosY = closestHit.point.y;
        float goalPos = hitPosY + 0.5f;

        // TODO: 거리와 시간에 따라 속도 조정
        if (closestHit.collider != null)
        {
            var tween = go.transform.DOMoveY(goalPos, 1f, false);
        }
    }

    public void PoolRelease()
    {
        ObjectPool.Release(this);
    }
}
