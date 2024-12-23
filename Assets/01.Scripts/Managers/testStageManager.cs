using System.Collections.Generic;
using UnityEngine;

public class testStageManager : Singleton<testStageManager>
{
    private Dictionary<string, StageBase> stageDictionary = new Dictionary<string, StageBase>();
    private StageBase currentStage = null;
    private Monster monster;
    private List<Monster> monsterList = new List<Monster>();
    private ShelterNPC shelterNPC;
    private GameObject shelterNPCObject;
    private RewardTree rewardTree;
    private GameObject rewardTreeObject;
    private GameObject player;

    private int monsterCount;
    private int weaponDestoryCount;

    protected override void Awake()
    {
        base.Awake();

    }

    public void CreatStage()
    {
        StageBase[] array = ResourceManager.Instance.LoadAllResources<StageBase>("Stages");
        foreach (var stages in array)
        {
            var obj = Instantiate(stages);
            stageDictionary.Add(obj.stageSO.stageKey, obj);
            obj.gameObject.SetActive(false);
            //Debug.Log($"key값 : {obj.stageSO.stageKey}");
        }
        CreatRewardTree();
        //CreatMonster();
    }

    private void CreatRewardTree()
    {
        rewardTree = ResourceManager.Instance.LoadResource<RewardTree>("NPC/RewardTree");
        rewardTreeObject = Instantiate(rewardTree.gameObject);
        rewardTreeObject.SetActive(false);
        //shelterNPC = ResourceManager.Instance.LoadResource<ShelterNPC>("NPC/ShelterNPC"); ;
        //shelterNPCObject = Instantiate(shelterNPC.gameObject);
        //shelterNPCObject.SetActive(false);
    }

    private void CreatMonster()
    {
        //TODO 오브젝트 풀 매니저 사용
        monster = ResourceManager.Instance.LoadResource<Monster>("NPC/Monster");
        //testPoolManager.Instance.CreatePool<Monster>(monster, false, 5, 50);
        var obj = testPoolManager.Instance.GetObject<Monster>();
        //for (int i = 0; i < 5; i++)
        //{
        //    var monsters = Instantiate(monster);
        //    monsterList.Add(monsters);
        //    monsterList[i].gameObject.SetActive(false);
        //}
    }


    public void StartStage(string key)
    {
        Debug.Log("dddddddddddd");
        player = GameManager.Instance.player;
        var stage = stageDictionary[key];
        stage.gameObject.SetActive(true);
        currentStage = stage;
        SetPlayerPosition();
        SetMonsterPosition(key);
        SetNPCPosition(key);
    }

    public void StageChange(string key)
    {
        GameManager.Instance.isClear = false;
        var stage = stageDictionary[key];
        stage.gameObject.SetActive(true);
        currentStage.gameObject.SetActive(false);
        currentStage = stage;
        SetPlayerPosition();
        SetMonsterPosition(key);
        SetNPCPosition(key);
    }

    private void SetPlayerPosition()
    {
        player.transform.position = currentStage.playerSpawnPoint.position;
    }

    private void SetMonsterPosition(string key)
    {
        if (key == "Stage1" || key == "Stage2" || key == "Boss")
        {
            if (!monsterList[0].gameObject.activeSelf)
            {
                monsterList[0].gameObject.SetActive(true);
                monsterCount++;
            }
            else
            {
                monsterList[1].gameObject.SetActive(true);
                monsterCount++;
            }

            monsterList[0].gameObject.transform.position = currentStage.monsterSpawnPoint.position;
        }
    }

    private void SetNPCPosition(string key)
    {
        if (key == "Stage1" || key == "Stage2")
        {
            rewardTreeObject.SetActive(true);
            rewardTree.transform.position = currentStage.rewardSpawnPoint.position;
        }
        else
        {
            rewardTreeObject.SetActive(false);
        }

        if (key == "Shelter")
        {
            shelterNPCObject.SetActive(true);
            shelterNPCObject.transform.position = currentStage.rewardSpawnPoint.position;
            GameManager.Instance.isClear = true;
        }
        else { shelterNPCObject.SetActive(false); }
    }


    public void MonsterDie()
    {
        monsterCount--;
        StageClear();
    }

    private void StageClear()
    {
        if (monsterCount == 0)
        {
            if (rewardTreeObject.TryGetComponent(out RewardTree tree))
            {
                tree.GetReward();
            }
            GameManager.Instance.isClear = true;
            if (currentStage.stageSO.stageKey == "Boss")
            {
                GameManager.Instance.GameEnd();
            }
        }
    }

    public void WeaponDestory()
    {
        for (int i = 0; i < rewardTree.rewardWeapon.Count; i++)
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
