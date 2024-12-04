using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardTree : MonoBehaviour,IInteractable
{
    protected int rewardCount;
    protected float rewardGrade;
    [SerializeField] private Reward rewardPrefab;
    [SerializeField] private Transform spawnPositionsRoot;
    private List<Transform> spawnPositions = new List<Transform>();
    //private WeaponSO weaponData = null;

    //�ϴ� �׽�Ʈ������ weaponData�� �ӽ������� ����
    [SerializeField] private WeaponSO weaponData = null;

    // ��ųʸ��� Ű������ Reward ������(�ϴ� �Ⱦ�)
    private Dictionary<int, Reward> rewards;

    // ���ҽ��Ŵ������� SO������ �������� ����

    private void Awake()
    {   
        rewards = PoolManager.Instance.rewards;
        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));
        }
    }

    // ������ ��ȣ�ۿ�
    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
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
    // TODO : �̸� ��ġ�� �����ϰԲ� ���鿹�� �׷��� �����丵 �ʿ�
    private void MakeReward(WeaponSO weaponData)
    {
        // ���Ż���
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            var reward = PoolManager.Instance.CreatReward();
            Vector2 rewardSpawnPonint = spawnPositions[i].transform.position;
            reward.SetPosition(rewardSpawnPonint);
        }

        // Reward���� ��ġ ����ذ� �����

        // Reward���� Data�����Ѱ� �����
        rewardPrefab.SetRewardData(weaponData);
    }
}
