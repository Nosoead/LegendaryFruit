using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardTree : MonoBehaviour,IInteractable
{
    protected int rewardCount = 1; // 나중에 강화되면 올라갈 변수

    protected float rewardGrade;
    [SerializeField] private Reward rewardPrefab;
    [SerializeField] private Transform spawnPositionsRoot;
    public List<Transform> spawnPositions = new List<Transform>();


    private List<Reward> rewards = new List<Reward>();

    //TODO :리소스매니저에서 SO형태인 무기정보 들고옴
    [SerializeField] public WeaponSO weaponData1 = null;

    //임의적으로 리스트로 SO담아옴
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

    // 나무와 상호작용
    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            
        }
    }

    // Tag : Player인 객체와 닿고있으면 UI띄움
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //UI를 띄운다
        }
    }

    // 콜라이더 벗어나면 UI꺼줌
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //UI를 없앤다
        }
    }

    public void SetReward() // Lobby, Stage클리어 -> GamaManager에서 판단 후 호출
    {
        //랜덤 돌려서 보상 SO - > // TODO : Lobby 신선도 level/stage 따라서
        MakeReward(weaponData1);
    }

    // SO 데이터를 열메에 넣는다.
    private void MakeReward(WeaponSO weaponData)
    {
        // 풀에 생성된 Reard를 랜덤으로 rewardCount갯수에 맞게 SetActive를 True
        for (int i = 0; i < rewardCount; i++)
        {
            int randomCount = UnityEngine.Random.Range(1, rewards.Count - 1);
            Reward randomReward = rewards[randomCount];
            weaponData = randomReward.weaponData;
            rewardPrefab.SetRewardData(weaponData);
            randomReward.gameObject.SetActive(true);

            rewards.Remove(randomReward);
        }
        // Reward에서 Data설정한거 여기로
    }


    // 랜덤으로 집어넣는데 까지는 성공 
    // 리펙토링 필요(만약에 SO를 각자 다른 SO가 필요하다면 필요)
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
