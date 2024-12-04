using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerSO playerData;
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerAttack attack;
    [SerializeField] private PlayerInteraction interaction;
    [SerializeField] private PlayerCondition condition;
    [SerializeField] private PlayerToUI openUI;
    //TODO 무기 Equiop
    private PlayerStat stat;
    private StatHandler statHandler;

    private void Awake()
    {
        stat = new PlayerStat();
        statHandler = new StatHandler();
    }

    private void Start()
    {
        //TODO : SaveManager 생성 후 LoadData에 대한 전역접근 될 때,
        //       Load 파일 null 여부에 따라 초기화 할 것
        stat.InitStat(playerData);
    }
}
