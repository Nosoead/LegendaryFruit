using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    private WeaponSO weaponData = null; // ���� �����丵
    private bool isGetWeapon = false;
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    //SO�� ���� �� ����������
    [SerializeField] private GameObject weaponPrefab;

    public void SetPositon(Vector2 positon)
    {
        //TODO ��ġ���� �ϱ�
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
