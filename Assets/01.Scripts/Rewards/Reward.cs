using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Reward : MonoBehaviour, ISetPooledObject<Reward>
{
    public WeaponSO weaponData = null; // ���� �����丵
    private bool isGetWeapon = false; // ���⸦ �������

    [SerializeField] private SpriteRenderer spriteRenderer;

    public event Action <Reward> OnReward;

    // SO�� ���� �� ���� ������
    [SerializeField] private Weapon weaponPrefab;

    // ������Ʈ Ǯ ����
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

    // ���� ��ġ�� ����ش�.
    public void SetPosition(Vector2 position)
    {
        // ��ġ ����
        this.transform.position = position;
    }

    // RewardData�� ����
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

    // TODO : DOTween ��� ����
    public void GetWeapon()
    {
        // �������
        GameObject weapon = Instantiate(weaponPrefab.gameObject);
        OnReward?.Invoke(this);
    }

}
