using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    private List<StageBase> stageList = new List<StageBase>();
    private StageBase currentStage = null;
    private Monster monster;
    private RewardTree rewardTree;
    private GameObject player;

    private int kiilMonsterCount;
    private int weaponDestoryCount;

    protected override void Awake()
    {
        base.Awake();
    }

    public void CreatStage()
    {
        StageBase[] array = ResourceManager.Instance.LoadAllResources<StageBase>("Stages");
        foreach(var stages  in array)
        {
            var obj = Instantiate(stages);
            stageList.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }

    public void StartStage(string key)
    {
        player = GameManager.Instance.player;
        for(int i = 0; i < stageList.Count; i++)
        {
            StageBase stage = stageList[i];
            if(stage.stageSO.stageKey == key)
            {
                stage.gameObject.SetActive(true);
                currentStage = stage;
                player.transform.position
                    = currentStage.playerSpawnPoint.position;
            }
        }
    }

    public void StageChange(string key)
    {
        monster = null;

        for (int i = 0; i < stageList.Count;i++)
        {
            StageBase stage = stageList[i];
            if(stage.stageSO.stageKey == key)
            {
                currentStage.gameObject.SetActive(false);
                currentStage = stage;
                currentStage.gameObject.SetActive(true);
                player.transform.position
                    = currentStage.playerSpawnPoint.position;
                CreatMonster();
                SetRewardPosition();    
            }
        }
    }

    public void CreatRewardTree()
    {
        for(int i = 0; i < rewardTree.spawnPositions.Count;i++)
        {

        }
    }

    public void CreatMonster()
    {
        if(currentStage.stageSO.isCreatMonster &&  monster == null)
        {
            monster = Instantiate(currentStage.monster);
            monster.gameObject.transform.position
                = currentStage.monsterSpawnPoint.position;
        }
        else
        {
            monster.gameObject.SetActive(false);
            return;
        }
    }

    public void SetRewardPosition()
    {
        for(int i = 0; i < rewardTree.spawnPositions.Count; i++)
        {
            var obj = rewardTree.rewards[i].transform.position;
            currentStage.rewardTree.rewards[i].transform.position = obj;
        }
    }

    public void MonsterDie()
    {
        if(!currentStage.monster.gameObject.activeSelf)
        {
            Debug.Log("죽다 몬스터");
            kiilMonsterCount++;
        }
        else
        {
            return;
        }
    }
    
    public bool StageClear()
    {
        if(currentStage.stageSO.clearMonsterKillCount == kiilMonsterCount)
        {
            GameManager.Instance.isClear = true;
        }
        return true;
    }

    public void WeaponDestory()
    {
        for(int i = 0; i < rewardTree.rewardWeapon.Count; i++)
        {
            weaponDestoryCount++;
            var obj = rewardTree.rewardWeapon[i].gameObject;
            Destroy(obj);
        }
    }
}
//{
//    [SerializeField] private GameObject player;

//    [SerializeField] private RewardTree tree;
//    [SerializeField] private Monster monster;

//    [SerializeField] private StageBase stagePrefab = null;

//    // 생성된 객체를 여기다가 담는다.
//    List<StageBase> stages = new List<StageBase>();

//    private string stageKey;
//    private int stageMonsterKillCount;

//    private Dictionary<string, StageBase> stageDictionary = new Dictionary<string, StageBase>();

//    protected override void Awake()
//    {
//        base.Awake();
//        SettingStage();
//        CreatStage();
//    }

//    private void Start()
//    {
//        tree = CreateRewardTree();
//        tree.gameObject.SetActive(false);
//    }
//    public void SettingStage()
//    {
//        StageBase[] stageArray = ResourceManager.Instance.LoadAllResources<StageBase>("Stages");
//        for(int i = 0; i < stageArray.Length; i++)
//        {
//            if(!stageDictionary.ContainsKey(stageArray[i].name))
//            {
//                stageDictionary.Add(stageArray[i].name, stageArray[i]);
//                //Debug.Log($"StageDictionary_Key :{stageArray[i].name}");
//            }
//        }
//    }

//    public RewardTree CreateRewardTree()
//    {
//        RewardTree[] tree = ResourceManager.Instance.LoadAllResources<RewardTree>("Stages/RewardTree");
//        var obj = Instantiate(tree[0]);
//        return obj;
//    }

//    public void CreatStage()
//    {
//        foreach(var item in stageDictionary.Values)
//        {
//            var obj = Instantiate(item);
//            stages.Add(obj);
//            obj.gameObject.SetActive(false);
//        }  
//    }

//    public void StartStage()
//    {
//        player = GameManager.Instance.player;
//        GameManager.Instance.isClear = true;
//        stagePrefab = stages.Find(n => n.stageKey == "1");
//        if (stagePrefab != null)
//        {
//            stagePrefab.gameObject.SetActive(true);
//            player.transform.position = stagePrefab.PlayerSpawnPoint();
//            Debug.Log($"Player : {player.name}");
//        }
//        else { return; }
//    }

//    public bool NextStage(string key)
//    {
//        for(int i = 0;  i < stages.Count; i++)
//        {
//            if(stages[i].stageKey == key)
//            {
//                GameObject go = Instantiate(monster.gameObject);
//                go.SetActive(false);
//                stagePrefab.gameObject.SetActive(false);
//                stagePrefab = stages[i];
//                stagePrefab.gameObject.SetActive(true);
//                player.transform.position = stagePrefab.PlayerSpawnPoint();

//                // RewardTree의 스폰포인트가 존재한다면 위치를 잡아준다.
//                if(stagePrefab.RewardTreeSpawnPoint() != null)
//                { 
//                    tree.gameObject.SetActive(true);
//                    tree.transform.position = stagePrefab.RewardTreeSpawnPoint();
//                    for(int j = 0; j < tree.spawnPositions.Count; j++)
//                    {   
//                        var rewardTree = tree.spawnPositions[j].position;
//                        tree.rewards[j].transform.position = rewardTree;
//                    }
//                }
//                if (stagePrefab.MonsterSpawnPoint() != null)
//                {
//                    // 임시적으로 풀링 말고 일단 생성 
//                    go.SetActive(true);
//                    go.gameObject.transform.position
//                        = stagePrefab.MonsterSpawnPoint();
//                }
//            }
//        }
//        GameManager.Instance.isGetWeapon = false;
//        WeaponDestroy();
//        return GameManager.Instance.isClear = false;
//    }

//    public void StageClear()
//    {   
//        GameManager.Instance.isClear = true;
//        tree.OpenReward();
//    }

//    public void WeaponDestroy()
//    {
//        if(!GameManager.Instance.isGetWeapon)
//        {
//            for (int i = 0; i < tree.getWeapon.Count; i++)
//            {
//                var obj = tree.getWeapon[i];
//                obj.gameObject.SetActive(false);
//            }
//        }
//    }

//    public void KillMonster(Monster monster)
//    {
//        Debug.Log($"몬스터를 잡았습니다.{monster.name}");
//        stageMonsterKillCount++;
//        if (stageMonsterKillCount >= stagePrefab.monsterCount)
//        {
//            Debug.Log("스테이지 클리어");
//            GameManager.Instance.isClear = true;
//        }      
//    }
//}
