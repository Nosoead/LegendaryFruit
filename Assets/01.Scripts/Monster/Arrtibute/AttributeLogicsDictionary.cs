using System.Collections.Generic;
using UnityEngine;

public class AttributeLogicsDictionary
{
    private Dictionary<AttributeType, AttributeLogics> attributeLogicMap;

    public void Initialize()
    {
        attributeLogicMap = new Dictionary<AttributeType, AttributeLogics>
        {
            { AttributeType.Normal, new NormalLogic() },
            { AttributeType.Burn, new BurnLogic() },
            { AttributeType.SlowDown, new SlowDown() },

        };
    }

    // 속성 타입 가져옴
    public AttributeLogics GetAttributeLogic(AttributeType type)
    {
        Debug.Log("진입");
        if (attributeLogicMap.TryGetValue(type, out AttributeLogics logic))
        {
            Debug.Log(attributeLogicMap[type]);
            return logic;
        }

        return null;
    }
}
