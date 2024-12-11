using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    private GameObject player;

    private RewardTree tree;

    [SerializeField] private StageBase stagePrefab = null;

    // 생성된 객체를 여기다가 담는다.
    List<StageBase> stages = new List<StageBase>();

    private string stageKey;

    private Dictionary<string, StageBase> stageDictionary = new Dictionary<string, StageBase>();
    private void Start()
    {
        player = GameManager.Instance.player;
        tree = CreateRewardTree();
        tree.gameObject.SetActive(false);

        SettingStage();
        CreatStage();
    }

    public void SettingStage()
    {
        StageBase[] stageArray = ResourceManager.Instance.LoadAllResources<StageBase>("Stages");
        for(int i = 0; i < stageArray.Length; i++)
        {
            if(!stageDictionary.ContainsKey(stageArray[i].name))
            {
                stageDictionary.Add(stageArray[i].name, stageArray[i]);
                Debug.Log($"StageDictionary_Key :{stageArray[i].name}");
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
        stagePrefab = stages.Find(n => n.stageKey == "1");
        if (stagePrefab != null)
        {
            stagePrefab.gameObject.SetActive(true);
            player.transform.position = stagePrefab.PlayerSpawnPoint();
        }
        else { return; }
    }

    public bool NextStage(string key)
    {
        for(int i = 0;  i < stages.Count; i++)
        {
            if(stages[i].stageKey == key)
            {
                stagePrefab.gameObject.SetActive(false);
                stagePrefab = stages[i];
                stagePrefab.gameObject.SetActive(true);
                player.transform.position = stagePrefab.PlayerSpawnPoint();
                // RewardTree의 스폰포인트가 존재한다면 위치를 잡아준다.
                if(stagePrefab.RewardTreeSpawnPoint() != null)
                { 
                    tree.gameObject.SetActive(true);    
                    tree.gameObject.transform.position
                        = stagePrefab.RewardTreeSpawnPoint();
                }
            }
        }
        return true;
    }
}
