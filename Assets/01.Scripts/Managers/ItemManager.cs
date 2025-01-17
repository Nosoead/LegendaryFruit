using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    //List of Choises for Random Selection
    private Dictionary<ItemType, List<ItemSO>> itemDictionary = new Dictionary<ItemType, List<ItemSO>>();
    private List<ItemSO> weaponList = new List<ItemSO>();
    private List<ItemSO> potionList = new List<ItemSO>();
    private List<ItemSO> currencyList = new List<ItemSO>();

    //Prevent duplicates during random selection
    private List<WeaponSO> OwnedWeaponList = new List<WeaponSO>();

    //For Upgrade
    private Dictionary<int, WeaponSO> weaponDictionary = new Dictionary<int, WeaponSO>();

    //Path
    private string weaponItemPath;
    private string potionItemPath;
    private string currencyItemPath;

    public void Init()
    {
        if (itemDictionary.Count == 0)
        {
            weaponItemPath = "ItemSO/WeaponSO";
            potionItemPath = "ItemSO/PotionSO";
            currencyItemPath = "ItemSO/CurrencySO";
            SetItemDictionary();
        }
        else
        {
            ResetWeaponList();
        }
    }

    #region /SetCollection
    private void SetItemDictionary()
    {
        SetWeapon();
        SetPotion();
        SetCurrency();
    }

    private void SetWeapon()
    {
        ItemSO[] weaponArray = ResourceManager.Instance.LoadAllResources<ItemSO>($"{weaponItemPath}");
        foreach (WeaponSO item in weaponArray)
        {
            if (!weaponDictionary.ContainsKey(item.ID))
            {
                weaponDictionary.Add(item.ID, item);
            }
            if (item.gradeType == GradeType.Normal)
            {
                weaponList.Add(item);
            }
        }
        itemDictionary.Add(ItemType.Weapon, weaponList);
    }

    private void SetPotion()
    {
        potionList = ResourceManager.Instance.LoadAllResources<ItemSO>($"{potionItemPath}").ToList();
        itemDictionary.Add(ItemType.Potion, potionList);
    }

    private void SetCurrency()
    {
        currencyList = ResourceManager.Instance.LoadAllResources<ItemSO>($"{currencyItemPath}").ToList();
        itemDictionary.Add(ItemType.Currency, currencyList);
    }

    private void ResetWeaponList()
    {
        foreach (WeaponSO weaponData in OwnedWeaponList)
        {
            weaponList.Add(weaponData);
        }
        OwnedWeaponList.Clear();
    }
    #endregion

    public ItemSO[] GetItemData(int selectNum, ItemType itemType)
    {
        ItemSO[] selectedItem = new ItemSO[selectNum];
        List<ItemSO> itemList = itemDictionary[itemType];
        int[] selectedNumber = RandomNumber(selectNum, itemList.Count);
        for (int i = 0; i < selectedNumber.Length; i++)
        {
            selectedItem[i] = itemList[selectedNumber[i]];
            if (itemType == ItemType.Weapon)
            {
                selectedItem[i] = RandomGrade(selectedItem[i]);
            }
        }
        return selectedItem;
    }

    private int[] RandomNumber(int selectNum, int maxNum)
    {
        int[] resultNum = new int[selectNum];
        int[] numberArray = new int[maxNum];
        for (int i = 0; i < numberArray.Length; i++)
        {
            numberArray[i] = i;
        }

        for (int i = 0; i < selectNum; i++)
        {
            int randomNum = Random.Range(0, maxNum);
            resultNum[i] = numberArray[randomNum];
            numberArray[randomNum] = numberArray[numberArray.Length - i - 1];
        }
        return resultNum;
    }

    //TODO 랜덤으로 뽑힌 숫자를 ID값에 맞도록 바꾸는 함수 필요.
    //TODO 이미 나온 아이템은 딕셔너리에서 제외 혹은 먹거나 반납한 아이템 다시 등록
    //TODO Init으로 기존 먹은 데이터 받아서 리스트에서 제외
    public WeaponSO GetUpgradeItemData(int ID)
    {
        WeaponSO upgradeItem = weaponDictionary[ID];
        return upgradeItem;
    }

    private ItemSO RandomGrade(ItemSO itemData)
    {
        if (itemData is WeaponSO weaponData)
        {
            int probability = Random.Range(0, 100);
            if (probability < 65)
            {
                return weaponData;
            }
            else if (probability < 90)
            {
                weaponData = weaponDictionary[weaponData.ID + 100000];
                return weaponData;
            }
            else
            {
                weaponData = weaponDictionary[weaponData.ID + 200000];
                return weaponData;
            }
        }
        else
        {
            return itemData;
        }
    }

    public void IncludeInSelectList(WeaponSO weaponData)
    {
        if (weaponData.type == AttributeType.Normal)
        {
            return;
        }
        weaponData = CheckItemGrade(weaponData);
        OwnedWeaponList.Remove(weaponData);
        weaponList.Add(weaponData);
    }

    public void ExcludeFromSelectList(WeaponSO weaponData)
    {
        if (weaponData.type == AttributeType.Normal)
        {
            return;
        }
        weaponData = CheckItemGrade(weaponData);
        OwnedWeaponList.Add(weaponData);
        weaponList.Remove(weaponData);
    }

    private WeaponSO CheckItemGrade(WeaponSO weaponData)
    {
        if (weaponData.gradeType != GradeType.Normal)
        {
            weaponData = weaponDictionary[weaponData.ID % 100000 + 100000];
        }
        return weaponData;
    }
}