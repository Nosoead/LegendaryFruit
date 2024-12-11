using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class Reward : MonoBehaviour, ISetPooledObject<Reward>
{
    public WeaponSO weaponData = null; // 그래픽 개선 예정
    //private bool isGetWeapon = false; // 경고를 제거하세요

    [SerializeField] private SpriteRenderer spriteRenderer;


    // SO를 사용해 데이터를 관리하세요
    [SerializeField] private Weapon weaponPrefab;

    // 오브젝트 풀 생성
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
        weaponPrefab.weaponData = weaponData;
    }

    // TODO: DOTween 효과 추가
    public Ease ease;

    public GameObject creatWeapon;

    public void GetWeapon()
    {
        // 초기화
        creatWeapon = Instantiate(weaponPrefab.gameObject);
        creatWeapon.transform.position = this.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(creatWeapon.transform.position,Vector2.down, 50, LayerMask.GetMask("Ground"));
        float hitPosY = hit.point.y;
        float goalPos = hitPosY + 0.3f;

        // TODO: 거리와 시간에 따라 속도 조정
        if (hit.collider != null)
        {
            // 무기가 떨궈짐
            var tween = creatWeapon.transform.DOMoveY(goalPos, 1f, false);
        }

        // 현재 상태
        GameManager.Instance.isCreatReward = false;
    }    
}
