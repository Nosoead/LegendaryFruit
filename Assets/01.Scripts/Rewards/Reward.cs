using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Reward : MonoBehaviour, ISetPooledObject<Reward>
{
    private WeaponSO weaponData = null; // ���� �����丵
    private bool isGetWeapon = false; // ���⸦ �������

    [SerializeField] private SpriteRenderer spriteRenderer;

    // SO�� ���� �� ���� ������
    [SerializeField] private Weapon weaponPrefab;

    // ������Ʈ Ǯ ����
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

    // ��ġ�� ����ش�.
    public void SetPosition(Vector2 position)
    {
        // ��ġ ����
    }

    // RewardData�� ����
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

    // TODO : DOTween ��� ����
    private void GetWeapon()
    {
        
    }  
}
