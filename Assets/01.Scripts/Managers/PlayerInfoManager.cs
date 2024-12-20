using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfoManager : Singleton<PlayerInfoManager>
{
    public UnityAction<SaveDataContainer> OnLoadEvent;
    [SerializeField] private PlayerStatManager statManager;
    [SerializeField] private PlayerEquipment equipment;
    private SaveDataContainer saveDataContainer;
    protected override void Awake()
    {
        EnsureComponents();
        saveDataContainer = new SaveDataContainer();
        //if (DataManager.Instance.LoadData<SaveDataContainer>() == null)
        //{
        //}
        //else
        //{
        //    saveDataContainer = DataManager.Instance.LoadData<SaveDataContainer>();
        //}
    }


    private void EnsureComponents()
    {
        if (statManager == null)
        {
            statManager = GetComponent<PlayerStatManager>();
        }
        if (equipment == null)
        {
            equipment = GetComponentInChildren<PlayerEquipment>();
        }
    }

    private void CallSaveData()
    {
        saveDataContainer.playerStatData = statManager.SaveStatManagerData();
    }

    public SaveDataContainer GetSaveData()
    {
        CallSaveData();
        return saveDataContainer;
    }

    public void Save()
    {
        CallSaveData();
        DataManager.Instance.SaveData(saveDataContainer);
    }

    public void Load()
    {
        saveDataContainer = DataManager.Instance.LoadData<SaveDataContainer>();
        statManager.LoadStatManagerData(saveDataContainer.playerStatData);
    }

    public void Delete()
    {
        statManager.DeleteStatManagerData();
    }
}

[System.Serializable]
public class SaveDataContainer
{
    public PlayerStatData playerStatData;//대충 세팅 완료
    public WeaponData weaponData;
    public int currentStage; //stageManager하면서
    public CurrencyData currencyData; //재화만들면
}

[System.Serializable]
public class WeaponData
{
    public List<WeaponSO> eatWeaponDataList;
    public List<WeaponSO> equippedWeapons;
}

[System.Serializable]
public class CurrencyData
{
    public float inGameCurrency;
    public float globalCurrency;
}