using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testManager : Singleton<testManager>
{
    [SerializeField] private TextMeshProUGUI stateTxt;

    public void ShowState(string txt)
    {
        stateTxt.text = txt;
    }
}
