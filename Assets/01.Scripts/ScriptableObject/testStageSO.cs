using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "testStageSO", menuName ="ScriptableObject/testStageSO", order = 4)]
public class testStageSO : ScriptableObject
{
    public GameObject stagePrefab;
    public string stageKey;
    public int clearMonsterKillCount;
    public int creatMonsterCount;
    public bool isCreatMonster;
    public bool isCreatRewardTree;

}
