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

    private void SetPlayer(GameObject player)
    {
        if (player == null) return;
        player.transform.position = playerSpawnPoint.position;
    }

    #region // Objects
    private void SetObject()
    {
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
    public void SetReward()
    {
        if (!stageData.canReceiveReward)
        {
            return;
        }
        rewardNPC.SetReward();
    }
    #endregion

    #region // RegularMonsters
    public void SetStage(GameObject player, IObjectPool<PooledMonster> monster)
    {
        SetPlayer(player);
        SetMonster(monster);
        SetObject();
    }

    private void SetMonster(IObjectPool<PooledMonster> monster)
    {
        if (!stageData.isCombatStage)
        {
            return;
        }

        foreach (var setMonsterPosition in monsterSpawnPoints)
        {
            var randomMonster = stageData.monsters[Random.Range(0, stageData.monsters.Count - 1)];
            PooledMonster monsterObj = monster.Get();
            monsterObj.gameObject.transform.position = setMonsterPosition.position;
            monsterObj.monsterManager.GetMonsterData(randomMonster.monsterID);
        }
    }
    #endregion

    #region // BossMonsters
    // Boss
    public void SetStage(GameObject player, IObjectPool<PooledBossMonster> bossMonster)
    {
        SetPlayer(player);
        SetMonster(bossMonster);
        SetObject();
    }

    private void SetMonster(IObjectPool<PooledBossMonster> bossMonster)
    {
        if (!stageData.isCombatStage)
        {
            return;
        }

        foreach (var setMonsterPosition in monsterSpawnPoints)
        {
            // 보스는 하나라 랜덤으로 뽑아도 의미 없음 그래도 일단 진행
            var randomBoss = stageData.monsters[Random.Range(0,stageData.monsters.Count - 1)];
            PooledBossMonster monsterObj = bossMonster.Get();
            monsterObj.gameObject.transform.position = setMonsterPosition.position;
            monsterObj.monsterManager.GetMonsterData(randomBoss.monsterID);
        }
    }
    #endregion
}