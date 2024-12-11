using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEquipment : MonoBehaviour
{
    //TODO 최대 2개 보관
    public UnityAction OnEquipWeaponEvent;
    private void Start()
    {
        OnEquipWeaponEvent?.Invoke();
    }
}
