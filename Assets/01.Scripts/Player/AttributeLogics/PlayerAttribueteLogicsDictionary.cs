using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribueteLogicsDictionary
{
    private Dictionary<AttributeType, PlayerAttributeLogics> attributeLogicMap;

    public void Initialize()
    {
        attributeLogicMap = new Dictionary<AttributeType, PlayerAttributeLogics>
        {
            { AttributeType.Burn, new PlayerBurn() },
            { AttributeType.SlowDown, new PlayerSlowDown() },
            { AttributeType.Normal, new PlayerNormal() },

        };
    }

    // 속성 타입 가져옴
    public PlayerAttributeLogics GetAttributeLogic(AttributeType type)
    {
        if (attributeLogicMap.TryGetValue(type, out PlayerAttributeLogics logic))
        {
            Debug.Log(attributeLogicMap[type]);
            return logic;
        }

        return null;
    }
}
