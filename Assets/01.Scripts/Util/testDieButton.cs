using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testDieButton : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        IDamageable test = GameManager.Instance.player.gameObject.GetComponent<IDamageable>();
        button.onClick.AddListener(()=> test.TakeDamage(200));
    }
}
