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

    //TODO :���ҽ��Ŵ������� SO������ �������� ����
    [SerializeField] public WeaponSO weaponData1 = null;

    //���������� ����Ʈ�� SO��ƿ�
    [SerializeField] private List<WeaponSO> weaponList = new List<WeaponSO>();


    private void Start()
    {
        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));
        }
        PoolManager.Instance.CreatePool<Reward>();
        rewards = PoolManager.Instance.rewards;
        RandomSO();
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
        MakeReward(weaponData1);
    }

    // SO �����͸� ���޿� �ִ´�.
    private void MakeReward(WeaponSO weaponData)
    {
        // Ǯ�� ������ Reard�� �������� rewardCount������ �°� SetActive�� True
        for (int i = 0; i < rewardCount; i++)
        {
            int randomCount = UnityEngine.Random.Range(1, rewards.Count - 1);
            Reward randomReward = rewards[randomCount];
            weaponData = randomReward.weaponData;
            rewardPrefab.SetRewardData(weaponData);
            randomReward.gameObject.SetActive(true);

            rewards.Remove(randomReward);
        }
        // Reward���� Data�����Ѱ� �����
    }


    // �������� ����ִµ� ������ ���� 
    // �����丵 �ʿ�(���࿡ SO�� ���� �ٸ� SO�� �ʿ��ϴٸ� �ʿ�)
    private void RandomSO()
    {
        for (int i = 0; i < rewards.Count;i++)
        {
            rewards[i].weaponData = weaponList[i];
        }
        //weaponList.Remove(rewards[i].weaponData);
        //var randomWeapon = UnityEngine.Random.Range(0, weaponList.Count - 1);
        //WeaponSO randomWeaponData = weaponList[randomWeapon];
        //rewards[i].weaponData = randomWeaponData;
        //weaponList.Remove(randomWeaponData);
    }

    public void GetWeaponButton()
    {
        rewardPrefab.GetWeapon(); 
    }
}
