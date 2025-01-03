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


    public void Save()
    {
        CallSaveData();
        DataManager.Instance.SaveData(saveDataContainer);
    }

    public void Load()
    {
        saveDataContainer = DataManager.Instance.LoadData<SaveDataContainer>();
        statManager.LoadStatManagerData(saveDataContainer.playerStatData);
        statManager.LoadConsumeData(saveDataContainer.weaponData.eatWeaponDataList);
        foreach (var dataear in saveDataContainer.weaponData.eatWeaponDataList)
        {
            Debug.Log($"섭취이름 {dataear.weaponName}, 타입 {dataear.type}");
        }
        equipment.LoadEquipmentData(saveDataContainer.weaponData.equippedWeapons, saveDataContainer.weaponData.currentEquipWeaponIndex);
        Debug.Log("중간끊어보기");
        foreach (var data in saveDataContainer.weaponData.equippedWeapons)
        {
            Debug.Log($"장착이름 {data.weaponName}, 타입 {data.type}");
        }


    }

    public void Delete()
    {
        statManager.DeleteStatManagerData();
        statManager.DeleteConsumeData();
    }

    private void CallSaveData()
    {
        saveDataContainer.playerStatData = statManager.SaveStatManagerData();
        saveDataContainer.weaponData.eatWeaponDataList = statManager.SaveConsumeData();
        var result = equipment.SaveEquipmentData();
        saveDataContainer.weaponData.equippedWeapons = result.Item1;
        saveDataContainer.weaponData.currentEquipWeaponIndex = result.Item2;
        saveDataContainer.currentStage = StageManager.Instance.GetCurrentStage();
    }

    public SaveDataContainer GetSaveData()
    {
        CallSaveData();
        return saveDataContainer;
    }

    public StageType GetStageID()
    {
        return saveDataContainer.currentStage;
    }
}

[System.Serializable]
public class SaveDataContainer
{
    public PlayerStatData playerStatData = new PlayerStatData();
    public WeaponData weaponData = new WeaponData();
    public StageType currentStage;
    public CurrencyData currencyData = new CurrencyData();
}

[System.Serializable]
public class WeaponData
{
    public List<WeaponSO> eatWeaponDataList = new List<WeaponSO>();
    public List<WeaponSO> equippedWeapons = new List<WeaponSO>();
    public int currentEquipWeaponIndex;
}

[System.Serializable]
public class CurrencyData
{
    public float inGameCurrency;
    public float globalCurrency;
}