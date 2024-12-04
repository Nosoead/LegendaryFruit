using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReawardTree : MonoBehaviour,IInteractable
{
    protected int rewardCount;
    protected float rewardGrade;
    [SerializeField] private Reward rewardPrefab;
    [SerializeField] private Transform spawnPositionsRoot;
    private List<Transform> spawnPositions = new List<Transform>();
    //private WeaponSO weaponData = null;

    //일단 테스트용으로 weaponData를 임시적으로 넣음
    [SerializeField] private WeaponSO weaponData = null;

    // 리소스매니저에서 SO형태인 무기정보 들고옴

    private void Awake()
    {
        SetReward();
        //PoolManager.Instance.CreatePool();

        //for(int i = 0; i < spawnPositionsRoot.childCount; i++)
        //{
        //    spawnPositions.Add(spawnPositionsRoot.GetChild(i));   
        //}
    }

    // 나무와 상호작용
    void IInteractable.Interact(bool isDeepPressed, bool isPressed)
    {
        if(isDeepPressed || isPressed)
        {
            return;
            // pool지우기
        }
    }

    public void SetReward() // Lobby, Stage클리어 -> GamaManager에서 판단 후 호출
    {
        //랜덤 돌려서 보상 SO - > // TODO : Lobby 신선도 level/stage 따라서

        MakeReward(weaponData);
    }

    // SO 데이터를 열메에 넣는다.
    private void MakeReward(WeaponSO weaponData)
    {
        // 열매생성
        // Reward에서 위치 잡아준거 여기로
        // Reward에서 Data설정한거 여기로
        rewardPrefab.SetRewardData(weaponData);
    }
}
