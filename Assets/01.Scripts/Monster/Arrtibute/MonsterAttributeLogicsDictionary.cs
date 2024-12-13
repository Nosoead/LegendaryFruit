using System.Collections.Generic;
using UnityEngine;

public class MonsterAttributeLogicsDictionary
{
    private Dictionary<AttributeType, MonsterAttributeLogics> attributeLogicMap;

    public void Initialize()
    {
        attributeLogicMap = new Dictionary<AttributeType, MonsterAttributeLogics>
        {
            { AttributeType.Normal, new MonsterNormal() },
            { AttributeType.Burn, new MonsterBurn() },
            { AttributeType.SlowDown, new MonsterSlowDown() },

        };
    }

    // 속성 타입 가져옴
    public MonsterAttributeLogics GetAttributeLogic(AttributeType type)
    {
        if (attributeLogicMap.TryGetValue(type, out MonsterAttributeLogics logic))
        {
            return logic;
        }

        return null;
    }
}
