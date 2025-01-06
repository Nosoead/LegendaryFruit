using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Stage : MonoBehaviour
{
    public StageSO stageData;
    public List<Parallax> parallaxList = new List<Parallax>();
    public PolygonCollider2D boundaryCollider;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private List<Transform> monsterSpawnPoints = new List<Transform>();
    [SerializeField] private Potal firstPotal;
    [SerializeField] private Potal secondPotal;
    [SerializeField] private List<NPC> stageNPC = new List<NPC>();

    public void SetStage(GameObject player, IObjectPool<PooledMonster> monster, CinemachineConfiner2D confiner)
    {
        SetPlayer(player);
        SetMonster(monster);
        SetObject();
        SetCameraBoundary(confiner);
    }

    public void SetStage(GameObject player, IObjectPool<PooledBossMonster> bossMonster, CinemachineConfiner2D confiner)
    {
        SetPlayer(player);
        SetMonster(bossMonster);
        SetObject();
        SetCameraBoundary(confiner);
    }

    private void SetPlayer(GameObject player)
    {
        if (player == null) return;
        player.transform.position = playerSpawnPoint.position;
    }

    #region /SetMonster
    private void SetMonster(IObjectPool<PooledMonster> monster)
    {
        if (!stageData.isCombatStage)
        {
            return;
        }
        if (monsterSpawnPoints.Count != stageData.TotalMonsterCount())
        {
            Debug.Log("몬스터 숫자와 리스폰 갯수가 일치하지 않습니다.");
            return;
        }
        //TODO : 각스테이지마다 몬스터 포인트와 몬스터 종류는 어떻게 할 것인가?
        //아이디어1 : a. StageSO에서 셋팅 순서 설정 // b. 순서에 맞춰 Stage포인트에 소환 
        //foreach (var setMonsterPosition in monsterSpawnPoints)
        //{
        //    var randomMonster = stageData.monsters[Random.Range(0, stageData.monsters.Count - 1)];
        //    PooledMonster monsterObj = monster.Get();
        //    monsterObj.gameObject.transform.position = setMonsterPosition.position;
        //    monsterObj.monsterManager.GetMonsterData(randomMonster.monsterID);
        //}
        int positionIndex = 0;
        for (int i = 0; i < stageData.monstersToSummon.Count; i++)
        {
            for (int j = 0; j < stageData.monstersToSummon[i].monsterCount; j++)
            {
                MonsterSO summonMonsterData = stageData.monstersToSummon[i].monsterData;
                PooledMonster monsterObj = monster.Get();
                monsterObj.gameObject.transform.position = monsterSpawnPoints[positionIndex].position;
                monsterObj.monsterManager.GetMonsterData(summonMonsterData.monsterID);
                positionIndex++;
            }
        }
    }

    private void SetMonster(IObjectPool<PooledBossMonster> bossMonster)
    {
        if (!stageData.isCombatStage)
        {
            return;
        }

        MonsterSO summonMonsterData = stageData.monstersToSummon[0].monsterData;
        PooledBossMonster monsterObj = bossMonster.Get();
        monsterObj.gameObject.transform.position = monsterSpawnPoints[0].position;
        monsterObj.monsterManager.GetMonsterData(summonMonsterData.monsterID);
        monsterObj.name = "111";
        Debug.Log(monsterObj.name);

        foreach (var setMonsterPosition in monsterSpawnPoints)
        {
            // 보스는 하나라 랜덤으로 뽑아도 의미 없음 그래도 일단 진행
            //var randomBoss = stageData.monsters[Random.Range(0,stageData.monsters.Count - 1)];
            //PooledBossMonster monsterObj = bossMonster.Get();
            //monsterObj.gameObject.transform.position = setMonsterPosition.position;
            //monsterObj.monsterManager.GetMonsterData(randomBoss.monsterID);
        }
    }
    #endregion

    #region /SetObject
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
        if (stageNPC != null)
        {
            foreach (var npc in stageNPC)
            {
                npc.InitNPC();
            }
        }
        if (parallaxList != null)
        {
            foreach (var parallax in parallaxList)
            {
                parallax.InitParallax();
            }
        }
    }

    private void SetCameraBoundary(CinemachineConfiner2D confiner)
    {
        confiner.m_BoundingShape2D = boundaryCollider;
    }

    public void SetReward()
    {
        if (!stageData.canReceiveReward)
        {
            return;
        }

        foreach (var npc in stageNPC)
        {
            npc.SetReward();
        }
    }
    #endregion
}