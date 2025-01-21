using UnityEngine;

public class StatHandler
{
    public float Add(float currentValue, float deltaValue)
    {
        return currentValue + deltaValue;
    }

    public float Add(float currentValue, float deltaVlue, float MaxValue)
    {
        return Mathf.Min(currentValue + deltaVlue, MaxValue);
    }

    public float Substract(float currentValue, float deltaValue)
    {
        return Mathf.Max(currentValue - deltaValue, 0f);
    }
}
