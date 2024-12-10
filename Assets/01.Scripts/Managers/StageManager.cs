using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    private TestPlayerScript player;
    //private Monster monster;
    private RewardTree rewardTree;

    [SerializeField] public StageBase currentStage = null;
    [SerializeField] private GameObject stagePrefab;

    private Dictionary<string, StageBase> stageDictionary = new Dictionary<string, StageBase>();
    private void Start()
    {
        player = GameManager.Instance.player;
        SetStageDictionary();
    }

    public void SetStageDictionary()
    {
        StageBase[] stageArray = ResourceManager.Instance.LoadAllResources<StageBase>("Stages");
        foreach (var stage in stageArray)
        {
            string name = stage.nextStageKey;
            if(!stageDictionary.ContainsKey(name))
            {
                stageDictionary.Add(name, stage);    
            }    
        }
    }

    //public StageBase GetStage<T>()
    //{
    //    stageDictionary.TryGetValue(typeof(T).Name, out var stage);
    //    return stage;
    //}

    public StageBase GetStage(string key)
    {
        stageDictionary.TryGetValue(key, out var stage);
        currentStage = stage;
        var obj = Instantiate(currentStage.gameObject);
        player.transform.position = currentStage.PlayerSpawnPoint();
        return stage;
    }

    public bool NextStage(string id)
    {  
        if(currentStage == null || currentStage.nextStageKey == string.Empty)
        {
            return false;
        }
        var obj = GetStage(id);
        switch(obj.nextStageKey)
        {
            case "1-1":
                currentStage = obj;
                stagePrefab.SetActive(false);
                stagePrefab = Instantiate(currentStage.gameObject);
                player.transform.position = currentStage.PlayerSpawnPoint();
                break;

            case "1-2":
                currentStage = obj;
                stagePrefab = Instantiate(currentStage.gameObject);
                player.transform.position = currentStage.PlayerSpawnPoint();               
                break;

            case "0-1":
                currentStage = obj;
                stagePrefab = Instantiate(currentStage.gameObject);
                player.transform.position = currentStage.PlayerSpawnPoint();               
                break;
        }
        return true;
    }

    //public void NextStage2()
    //{
    //    var obj = GetStage<Stage2>();
    //    currentStage = obj;
    //    stagePrefab.SetActive(false);
    //    stagePrefab = Instantiate(currentStage.gameObject);
    //    player.transform.position = currentStage.PlayerSpawnPoint();
    //}

    //public void NextShelterStage()
    //{
    //    var obj = GetStage<ShelterStage>();
    //    currentStage = obj;
    //    stagePrefab.SetActive(false);
    //    stagePrefab = Instantiate(currentStage.gameObject);
    //    player.transform.position = currentStage.PlayerSpawnPoint();
    //}
}
