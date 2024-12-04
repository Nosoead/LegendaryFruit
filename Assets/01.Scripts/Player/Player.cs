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
    //TODO ���� Equiop
    private PlayerStat stat;
    private StatHandler statHandler;

    private void Awake()
    {
        stat = new PlayerStat();
        statHandler = new StatHandler();
    }

    private void Start()
    {
        //TODO : SaveManager ���� �� LoadData�� ���� �������� �� ��,
        //       Load ���� null ���ο� ���� �ʱ�ȭ �� ��
        stat.InitStat(playerData);
    }
}
