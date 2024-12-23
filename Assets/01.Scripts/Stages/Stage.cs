using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Stage : MonoBehaviour
{
    public StageSO stageData;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private List<Transform> monsterSpawnPoints = new List<Transform>();
    [SerializeField] private Potal firstPotal;
    [SerializeField] private Potal secondPotal;
    [SerializeField] private RewardNPC rewardNPC;

    public void SetStage(GameObject player, IObjectPool<PooledMonster> monster)
    {
        SetPlayer(player);
        SetMonster(monster);
        if (firstPotal != null)
        {
            firstPotal.InitPotal();
        }
        if (secondPotal != null)
        {
            secondPotal.InitPotal();
        }
        if (rewardNPC != null)
        {
            rewardNPC.InitRewardNPC();
        }
    }

    private void SetPlayer(GameObject player)
    {
        player.transform.position = playerSpawnPoint.position;
    }

    private void SetMonster(IObjectPool<PooledMonster> monster)
    {
        if (!stageData.isCombatStage)
        {
            return;
        }

        foreach (var setMonsterPosition in monsterSpawnPoints)
        {
            PooledMonster monsterObj = monster.Get();
            monsterObj.gameObject.transform.position = setMonsterPosition.position;
            monsterObj.statManager.SetInitStat();
        }
    }

    public void SetReward()
    {
        if (!stageData.canReceiveReward)
        {
            return;
        }
        rewardNPC.SetReward();
    }
}