using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : UIBase
{
    
    [SerializeField] private TextMeshProUGUI playerStat;
    [SerializeField] private TextMeshProUGUI itemDescription1;
    [SerializeField] private TextMeshProUGUI itemDescription2;
    [SerializeField] private TextMeshProUGUI item1;
    [SerializeField] private TextMeshProUGUI item2;

    public override void Open()
    {
        base.Open();

        /*if ()
        {
            itemDescription1.text = "무기 SO에 있는 설명";
        }
        else
        {
            itemDescription2.text = "무기 SO에 있는 설명..";
        }
*/
    }
}
