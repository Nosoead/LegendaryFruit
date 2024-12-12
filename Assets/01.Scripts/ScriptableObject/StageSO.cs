using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSO", menuName ="ScriptableObject/StageSO", order = 4)]
public class StageSO : ScriptableObject
{
    public GameObject stagePrefab;
    public string stageKey;
    public int clearMonsterKillCount;
    public int creatMonsterCount;
    public bool isCreatMonster;
    public bool isCreatRewardTree;

}
