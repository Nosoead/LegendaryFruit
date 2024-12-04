using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    private WeaponSO weaponData = null; // 추후 리펙토링
    private bool isGetWeapon = false;
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    //SO가 없는 빈 무기프리펩
    [SerializeField] private GameObject weaponPrefab;

    public void SetPositon(Vector2 positon)
    {
        //TODO 위치설정 하기
    }    

    public void SetRewardData(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
        spriteRenderer.sprite = this.weaponData.rewardSprite;
        DataToObject();
    }

    private void DataToObject()
    {
        throw new NotImplementedException();
    }
}
