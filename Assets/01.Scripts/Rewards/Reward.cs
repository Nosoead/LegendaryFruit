using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class Reward : MonoBehaviour, ISetPooledObject<Reward>
{
    public WeaponSO weaponData = null; // ���� �����丵
    //private bool isGetWeapon = false; // ���⸦ �������

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
    /// <summary>
    /// ������ ��ġ������ ����
    /// </summary>
    /// <param name="position">������ ��ġ������ ����</param>
    public void SetPosition(Vector2 position)
    {
        // ��ġ ����
        this.transform.position = position;
    }

    // RewardData�� ����
    /// <summary>
    /// ���ſ� � WeaponData�� �־��ٰ�����
    /// </summary>
    /// <param name="weaponData">���ſ� ������ WeaponData</param>
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

    public Ease ease;
    public void GetWeapon()
    {
        // �������
        GameObject weapon = Instantiate(weaponPrefab.gameObject);
        weapon.transform.position = this.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(weapon.transform.position,Vector2.down, 50, LayerMask.GetMask("Ground"));
        float hitPosY = hit.point.y;

        //TODO : �Ÿ��� ���� �������� �ӵ� ����
        if (hit.collider != null)
        {
            // ���߿� �ν����� â���� �����ϸ� ���� �� ��
            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence.Append(weapon.transform.DOMoveY(hitPosY + 0.3f, 0.5f, false)
                .SetSpeedBased());
            sequence.Append(weapon.transform.DOPunchPosition(Vector2.up, 2f, (int)0.7f, 0,false)
                .SetLoops(-1, LoopType.Restart));
        }
        // ���� ����

        GameManager.Instance.isCreatReward = false;
    }

    public void AddTween()
    {

    }

}
