using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfoManager : Singleton<PlayerInfoManager>
{
    public UnityAction<SaveDataContainer> OnLoadEvent;
    [SerializeField] private PlayerStatManager statManager;
    [SerializeField] private PlayerEquipment equipment;
    [SerializeField] private CurrencySystem currency;
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
        if (currency == null)
        {
            currency = GetComponent<CurrencySystem>();
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
        equipment.LoadEquipmentData(saveDataContainer.weaponData.equippedWeapons, saveDataContainer.weaponData.currentEquipWeaponIndex);
        currency.LoadCurrencyData(saveDataContainer.currencyData);
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
        saveDataContainer.currencyData = currency.SaveCurrencyData();
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

    public void SetCurrency()
    {
        currency.GetCurrency(0, isGlobalCurrency: true);
        currency.GetCurrency(0, isGlobalCurrency: false);
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
    public int inGameCurrency;
    public int globalCurrency;
}