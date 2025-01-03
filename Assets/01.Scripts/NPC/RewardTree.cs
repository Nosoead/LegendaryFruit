using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardTree : MonoBehaviour,IInteractable
{
    protected int rewardCount = 1; // 나중에 추가하면 코드 수정
    protected float rewardGrade;

    public PooledReward rewardPrefab;
    private List<PooledReward> randomReward = new List<PooledReward>(); 

    [SerializeField] private Transform spawnPositionsRoot;
    public List<Transform> spawnPositions = new List<Transform>();

    // 나중에 Dictionary에 데이터 저장 기능 추가 (리소스 개선)
    public List<PooledReward> rewards = new List<PooledReward>();
    public List<GameObject> rewardWeapon = new List<GameObject>();


    public event Action<PooledReward> OnReward;

    // TODO: 유저데이터를 관리하는 SO클래스를 생성하여 적용
    [SerializeField] public WeaponSO weaponData = null;

    //데이터베이스의 리스트를 SO로 옮김
    [SerializeField] private List<WeaponSO> weaponList = new List<WeaponSO>();

    private void Awake()
    {
        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));
        }
        //for(int i = 0; i < spawnPositions.Count; i++)
        //{
        //    Debug.Log($"CreateReward SpawnPoint : {spawnPositions[i].position}");
        //}
        CreatReward();
    }

    public void CreatReward()
    {
        testPoolManager.Instance.CreatePool<PooledReward>(rewardPrefab,false, 5, 50);

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            var reward =  testPoolManager.Instance.GetObject<PooledReward>();
            reward.SetPosition(spawnPositions[i].position);
            reward.weaponData = weaponList[i];
            reward.gameObject.SetActive(false);
            rewards.Add(reward);
        }
    }

    // 플레이어가 보호막을 얻음 -> 상태 업데이트
    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            RewardToGetWeapon();
        }
    }

    public void GetReward() // Lobby, Stage 클래스 -> GameManager에서 호출 또는 연결
    {
        // 기존 데이터를 관리하는 SO -> // TODO: Lobby 진입 시 level/stage 초기화
        MakeReward(weaponData);
    }

    /// <summary>
    /// 보상을 줄 때 WeaponData를 추가하는 작업
    /// </summary>
    /// <param name="weaponData"> 보상에 추가할 WeaponData</param>
    private void MakeReward(WeaponSO weaponData)
    {
        // 풀에서 꺼낸 Reward를 초기화하고 rewardCount에 따라 SetActive를 True로 설정
        for (int i = 0; i < rewardCount; i++)
        {
            randomReward.Add(rewards[RandomCount()]);
            weaponData = randomReward[i].weaponData;
            rewardPrefab.SetRewardData(weaponData);
            randomReward[i].gameObject.SetActive(true);
            OnReward += DisableReward;  
        }
        GameManager.Instance.isCreatReward = true;
        // Reward에서 Data를 받아 초기화
    }

    public int RandomCount()
    {
        int randomCount = UnityEngine.Random.Range(1, rewards.Count - 1);
        return randomCount;
    }

    // 열매 따고 무기 지급
    public void RewardToGetWeapon()
    {
        for(int i = 0; i < randomReward.Count; i++)
        {
            ////var obj = randomReward[i].GetWeapon();
            //OnReward?.Invoke(randomReward[i]);
            //rewardWeapon.Add(obj);
            //GameManager.Instance.isGetWeapon = true;
        }
    }

    /// <summary>
    /// 보상에서 사용할 특정 데이터를 포함한 Reward
    /// </summary>
    /// <param name="reward">보상에서 사용할 특정 데이터를 포함한 Reward</param>
    public void DisableReward(PooledReward reward)
    {
        reward.gameObject.SetActive(false);
    }
}
