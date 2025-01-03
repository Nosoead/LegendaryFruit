using System.Collections.Generic;
using System.Linq;
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
}