using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSO", menuName = "ScriptableObject/StageSO", order = 5)]
public class StageSO : ScriptableObject
{
    //   AB0CD -> A : 보상종류 B : 스테이지 C : 단계 D : 버전
    //ex)13012 -> 무기보상 3-1스테이지 2버전 / 현재는 그냥 1스테이지에 전부 1버전으로
    public int stageID;
    public bool isCombatStage;
    public bool isBossStage;
    public bool canReceiveReward;
    public int monsterCount;

    //TODO 스테이지에 따른 보상확률 조절하는 필드추가~
}