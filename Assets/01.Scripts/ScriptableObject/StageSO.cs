using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSO", menuName ="ScriptableObject/StageSO", order = 4)]
public class StageSO : ScriptableObject
{
    public StageType stageType;
    public GameObject stagePrefab;
    public Transform playerStartPosition;
    public Transform monsterStartPositon;
}
