using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Reward : MonoBehaviour, ISetPooledObject<Reward>
{
    public WeaponSO weaponData = null; // 추후 리펙토링
    private bool isGetWeapon = false; // 무기를 얻었는지
    public event EventHandler <Reward> OnReward;

    [SerializeField] private SpriteRenderer spriteRenderer;

    // SO가 없는 빈 무기 프리펩
    [SerializeField] private Weapon weaponPrefab;

    // 오브젝트 풀 설정
    private IObjectPool<Reward> objectPool;

    private void Start()
    {
        //OnReward += DisableReward;
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
    public void SetPosition(Vector2 position)
    {
        // 위치 설정
        this.transform.position = position;
    }

    // RewardData를 설정
    public void SetRewardData(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
        spriteRenderer.sprite = this.weaponData.rewardSprite;
        DataToObject();
        //GetWeapon();
    }
    private void DisableReward(GameObject obj, Reward reward)
    {
        obj.SetActive(false);
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
        
    }  
}
