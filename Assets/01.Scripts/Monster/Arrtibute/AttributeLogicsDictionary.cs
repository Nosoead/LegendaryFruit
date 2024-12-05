using System.Collections.Generic;

public abstract class AttributeLogicsDictionary
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
        if (attributeLogicMap.TryGetValue(type, out AttributeLogics logic))
        {
            return logic;
        }

        return null;
    }
}
