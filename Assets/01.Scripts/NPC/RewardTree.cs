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

    //�ϴ� �׽�Ʈ������ weaponData�� �ӽ������� ����
    [SerializeField] private WeaponSO weaponData = null;

    // ���ҽ��Ŵ������� SO������ �������� ����

    private void Awake()
    {
        SetReward();
        //PoolManager.Instance.CreatePool();

        //for(int i = 0; i < spawnPositionsRoot.childCount; i++)
        //{
        //    spawnPositions.Add(spawnPositionsRoot.GetChild(i));   
        //}
    }

    // ������ ��ȣ�ۿ�
    void IInteractable.Interact(bool isDeepPressed, bool isPressed)
    {
        if(isDeepPressed || isPressed)
        {
            return;
            // pool�����
        }
    }

    public void SetReward() // Lobby, StageŬ���� -> GamaManager���� �Ǵ� �� ȣ��
    {
        //���� ������ ���� SO - > // TODO : Lobby �ż��� level/stage ����

        MakeReward(weaponData);
    }

    // SO �����͸� ���޿� �ִ´�.
    private void MakeReward(WeaponSO weaponData)
    {
        // ���Ż���
        // Reward���� ��ġ ����ذ� �����
        // Reward���� Data�����Ѱ� �����
        rewardPrefab.SetRewardData(weaponData);
    }
}
