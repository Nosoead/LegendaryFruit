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
        //TODO : SaveManager 생성 후 LoadData에 대한 전역접근 될 때,
        //       Load 파일 null 여부에 따라 초기화 할 것
        stat.InitStat(playerData);
    }
}
