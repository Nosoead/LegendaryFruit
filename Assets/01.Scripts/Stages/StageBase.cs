using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class StageBase : MonoBehaviour
{
    [SerializeField] private TestPlayerScript player;
    [SerializeField] private Monster monster;
    [SerializeField] private Potal potal;
    [SerializeField] private Transform spawnPointRot;
    Dictionary<string, Vector2> keyValuePairs = new Dictionary<string, Vector2>();

    private void Awake()
    {
        //for(int i = 0; i <spawnPointRot.childCount; i++)
        //{
        //    spawnPointRot.c
        //}
        //PlayerSpawn()
    }

    //private Vector2 PlayerSpawn()
    //{
    //    // 딕셔너리 
    //    return player.transform.position = Vector2 as;
    //}
}
