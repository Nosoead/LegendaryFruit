using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    private List<WeaponSO> itemList = new List<WeaponSO>();
    private Dictionary<int, WeaponSO> itemDictionary = new Dictionary<int, WeaponSO>();
    private string path;

    private void Start()
    {
        path = "ItemSO";
        SetItemDictionary();
    }

    private void SetItemDictionary()
    {
        WeaponSO[] itemArray = ResourceManager.Instance.LoadAllResources<WeaponSO>($"{path}");
        itemList = itemArray.ToList();
        foreach (WeaponSO item in itemList)
        {
            if (!itemDictionary.ContainsKey(item.ID))
            {
                itemDictionary.Add(item.ID, item);
            }
        }
    }

    public WeaponSO[] GetItemData(int selectNum)
    {
        WeaponSO[] selectedItem = new WeaponSO[selectNum];
        int[] selectedNumber = RandomNumber(selectNum, itemList.Count);
        for (int i = 0; i < selectedNumber.Length; i++)
        {
            selectedItem[i] = itemList[selectedNumber[i]];
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
        WeaponSO upgradeItem = itemDictionary[ID];
        return upgradeItem;
    }
}