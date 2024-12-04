using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Reward : MonoBehaviour, ISetPooledObject<Reward>
{
    private WeaponSO weaponData = null; // 추후 리펙토링
    private bool isGetWeapon = false; // 무기를 얻었는지

    [SerializeField] private SpriteRenderer spriteRenderer;

    // SO가 없는 빈 무기 프리펩
    [SerializeField] private Weapon weaponPrefab;

    // 오브젝트 풀 설정
    private IObjectPool<Reward> objectPool;

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
        Invoke(nameof(DelayMethod), 3f);
    }

    private void DelayMethod()
    {
        objectPool.Release(this);
    }

    // 위치를 잡아준다.
    public void SetPosition(Vector2 position)
    {
        // 위치 설정
    }

    // RewardData를 설정
    public void SetRewardData(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
        spriteRenderer.sprite = this.weaponData.rewardSprite;   
        DataToObject();
    }

    private void DataToObject()
    {
        weaponPrefab.weaponData = weaponData;

    }

    // TODO : DOTween 사용 예정
    private void GetWeapon()
    {
        
    }  
}
