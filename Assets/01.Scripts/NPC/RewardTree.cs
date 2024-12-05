using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardTree : MonoBehaviour,IInteractable
{
    protected int rewardCount = 1; // ���߿� ��ȭ�Ǹ� �ö� ����

    protected float rewardGrade;
    [SerializeField] private Reward rewardPrefab;
    [SerializeField] private Transform spawnPositionsRoot;
    public List<Transform> spawnPositions = new List<Transform>();
    private List<Reward> rewards = new List<Reward>();

    //�ϴ� �׽�Ʈ������ weaponData�� �ӽ������� ����
    //TODO :���ҽ��Ŵ������� SO������ �������� ����
    [SerializeField] private WeaponSO weaponData = null;


    private void Awake()
    {   
        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));
        }
        PoolManager.Instance.CreatePool<Reward>();
        rewards = PoolManager.Instance.rewards;
    }

    // ������ ��ȣ�ۿ�
    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            
        }
    }

    // Tag : Player�� ��ü�� ��������� UI���
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //UI�� ����
        }
    }

    // �ݶ��̴� ����� UI����
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //UI�� ���ش�
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
        // Ǯ�� ������ Reard�� �������� rewardCount������ �°� SetActive�� True
        for (int i = 0; i < rewardCount; i++)
        {
            int randomCount = UnityEngine.Random.Range(1, rewards.Count - 1);
            Reward randomReward = rewards[randomCount];
            randomReward.gameObject.SetActive(true);
            rewards.Remove(randomReward);
        }

        // Reward���� ��ġ ����ذ� �����

        // Reward���� Data�����Ѱ� �����
        rewardPrefab.SetRewardData(weaponData);
    }
}
