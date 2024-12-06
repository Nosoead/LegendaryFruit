using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class Reward : MonoBehaviour, ISetPooledObject<Reward>
{
    public WeaponSO weaponData = null; // 추후 리펙토링
    //private bool isGetWeapon = false; // 무기를 얻었는지

    [SerializeField] private SpriteRenderer spriteRenderer;

    public event Action <Reward> OnReward;

    // SO가 없는 빈 무기 프리펩
    [SerializeField] private Weapon weaponPrefab;

    // 오브젝트 풀 설정
    private IObjectPool<Reward> objectPool;

    private void Start()
    {
        
    }


    public IObjectPool<Reward> ObjectPool
    {
        get => objectPool;
        set => objectPool = value;
    }

    public void SetPooledObject(IObjectPool<Reward> pool)
    {
        objectPool = pool;
    }

    public void ReleaseObject()
    {
        Invoke(nameof(DelayMethod), 0f);
    }

    private void DelayMethod()
    {
        objectPool.Release(this);
    }

    // 생성 위치를 잡아준다.
    /// <summary>
    /// 열매의 위치정보를 전달
    /// </summary>
    /// <param name="position">열매의 위치정보를 전달</param>
    public void SetPosition(Vector2 position)
    {
        // 위치 설정
        this.transform.position = position;
    }

    // RewardData를 설정
    /// <summary>
    /// 열매에 어떤 WeaponData를 넣어줄것인지
    /// </summary>
    /// <param name="weaponData">열매에 설정될 WeaponData</param>
    public void SetRewardData(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
        spriteRenderer.sprite = this.weaponData.rewardSprite;
        DataToObject();
        //GetWeapon();
    }

    private void DataToObject()
    {
        weaponPrefab.weaponData = weaponData;
    }

    // TODO : DOTween 사용 예정
    public void GetWeapon()
    {
        // 무기생성
        GameObject weapon = Instantiate(weaponPrefab.gameObject);
        weapon.transform.position = this.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(weapon.transform.position,Vector2.down, 50, LayerMask.GetMask("Ground"));
        float hitPosY = hit.point.y;
        weapon.transform.DOMoveY(hitPosY + 0.3f, 1f, false);
        Debug.Log(hitPosY);

        // 무기 회전

        // 무기 부유
        GameManager.Instance.isCreatReward = false;
    }

}
