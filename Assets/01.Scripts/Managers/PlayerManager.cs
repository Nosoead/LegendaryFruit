using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerSO playerData;
    private PlayerStat stat;
    private StatHandler statHandler;
    [SerializeField] private PlayerCondition condition;

    private void Awake()
    {
        stat = new PlayerStat();
        statHandler = new StatHandler();
        if (condition == null)
        {
            condition = GetComponent<PlayerCondition>();
        }
    }

    private void Start()
    {
        //TODO : SaveManager ���� �� LoadData�� ���� �������� �� ��,
        //       Load ���� null ���ο� ���� �ʱ�ȭ �� ��
        stat.InitStat(playerData);
    }
}
