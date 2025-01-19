using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Stage : MonoBehaviour
{
    [SerializeField] private StageSO stageData;
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
        UIManager.Instance.ToggleUI<BossHPUI>(false,true);
        monsterObj.gameObject.transform.position = monsterSpawnPoints[0].position;
        monsterObj.monsterManager.GetMonsterData(summonMonsterData.monsterID);
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

    #region /GetData
    public int GetStageID()
    {
        return stageData.stageID;
    }

    public int GetMonsterCount()
    {
        return stageData.TotalMonsterCount();
    }

    public bool GetCombatData()
    {
        return stageData.isCombatStage;
    }

    public bool GetBossData()
    {
        return stageData.isBossStage;
    }
    #endregion

    public void RegisterWeapon(PooledFruitWeapon discardedObject)
    {
        foreach (var npc in stageNPC)
        {
            if (npc is RewardWeaponNPC rewardWeaponNPC)
            {
                rewardWeaponNPC.RegisterWeapon(discardedObject);
            }
        }
    }

    public void PoolRelease()
    {
        foreach (var npc in stageNPC)
        {
            npc.ReleaseReward();
        }
    }
}