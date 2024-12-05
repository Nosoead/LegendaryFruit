/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttributeLogicsDictionary, IDamageable
{
    private Dictionary<AttributeType, AttributeLogics> attributeLogicMap;

    public void Initialize()
    {
        attributeLogicMap = new Dictionary<AttributeType, AttributeLogics>
        {
            { AttributeType.Normal, new TakeDamage() },
            { AttributeType.Burn, new BurnDamage() },
            { AttributeType.SlowDown, new SlowDown() },

        };
    }

    public AttributeLogics GetAttributeLogic(AttributeType type)
    {
        if (attributeLogicMap.TryGetValue(type, out AttributeLogics logic))
        {
            return logic;
        }

        return null;
    }
}
*/
