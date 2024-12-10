using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    private Dictionary<string, StageBase> stageDictionary = new Dictionary<string, StageBase>();
    private void Start()
    {
        SetDictionary();
    }

    public void SetDictionary()
    {
        StageBase[] stageArray = ResourceManager.Instance.LoadAllResources<StageBase>("Stages");
        foreach (var stage in stageArray)
        {
            string name = stage.GetType().Name;
            if(!stageDictionary.ContainsKey(name))
            {
                stageDictionary.Add(name, stage);    
            }    
        }
    }

    public StageBase GetStage<T>()
    {
        stageDictionary.TryGetValue(typeof(T).Name, out var stage);
        return stage;
    }


}
