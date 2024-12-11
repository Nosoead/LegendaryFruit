using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private GameObject player;

    [SerializeField] private RewardTree tree;
    [SerializeField] private Monster monster;

    [SerializeField] private StageBase stagePrefab = null;

    // 생성된 객체를 여기다가 담는다.
    List<StageBase> stages = new List<StageBase>();

    private string stageKey;
    private int stageMonsterKillCount;

    private Dictionary<string, StageBase> stageDictionary = new Dictionary<string, StageBase>();

    protected override void Awake()
    {
        base.Awake();
        SettingStage();
        CreatStage();
    }

    private void Start()
    {
        tree = CreateRewardTree();
        tree.gameObject.SetActive(false);
    }
    public void SettingStage()
    {
        StageBase[] stageArray = ResourceManager.Instance.LoadAllResources<StageBase>("Stages");
        for(int i = 0; i < stageArray.Length; i++)
        {
            if(!stageDictionary.ContainsKey(stageArray[i].name))
            {
                stageDictionary.Add(stageArray[i].name, stageArray[i]);
                //Debug.Log($"StageDictionary_Key :{stageArray[i].name}");
            }
        }
    }

    public RewardTree CreateRewardTree()
    {
        RewardTree[] tree = ResourceManager.Instance.LoadAllResources<RewardTree>("Stages/RewardTree");
        var obj = Instantiate(tree[0]);
        return obj;
    }
    
    public void CreatStage()
    {
        foreach(var item in stageDictionary.Values)
        {
            var obj = Instantiate(item);
            stages.Add(obj);
            obj.gameObject.SetActive(false);
        }  
    }

    public void StartStage()
    {
        player = GameManager.Instance.player;
        GameManager.Instance.isClear = true;
        stagePrefab = stages.Find(n => n.stageKey == "1");
        if (stagePrefab != null)
        {
            stagePrefab.gameObject.SetActive(true);
            player.transform.position = stagePrefab.PlayerSpawnPoint();
            Debug.Log($"Player : {player.name}");
        }
        else { return; }
    }

    public bool NextStage(string key)
    {
        for(int i = 0;  i < stages.Count; i++)
        {
            if(stages[i].stageKey == key)
            {
                GameObject go = Instantiate(monster.gameObject);
                go.SetActive(false);
                stagePrefab.gameObject.SetActive(false);
                stagePrefab = stages[i];
                stagePrefab.gameObject.SetActive(true);
                player.transform.position = stagePrefab.PlayerSpawnPoint();

                // RewardTree의 스폰포인트가 존재한다면 위치를 잡아준다.
                if(stagePrefab.RewardTreeSpawnPoint() != null)
                { 
                    tree.gameObject.SetActive(true);
                    tree.transform.position = stagePrefab.RewardTreeSpawnPoint();
                    for(int j = 0; j < tree.spawnPositions.Count; j++)
                    {   
                        var rewardTree = tree.spawnPositions[j].position;
                        tree.rewards[j].transform.position = rewardTree;
                    }
                }
                if (stagePrefab.MonsterSpawnPoint() != null)
                {
                    // 임시적으로 풀링 말고 일단 생성 
                    go.SetActive(true);
                    go.gameObject.transform.position
                        = stagePrefab.MonsterSpawnPoint();
                }
            }
        }
        GameManager.Instance.isGetWeapon = false;
        return GameManager.Instance.isClear = false;
    }

    public void StageClear()
    {   
        GameManager.Instance.isClear = true;
        tree.OpenReward();
    }

    public void WeaponDestroy(GameObject weapon)
    {
        if(!GameManager.Instance.isGetWeapon)
        {
            weapon.SetActive(false);
        }
        else { return; }
    }

    public void KillMonster(Monster monster)
    {
        Debug.Log($"몬스터를 잡았습니다.{monster.name}");
        stageMonsterKillCount++;
        if (stageMonsterKillCount >= stagePrefab.monsterCount)
        {
            Debug.Log("스테이지 클리어");
            GameManager.Instance.isClear = true;
        }      
    }
}
