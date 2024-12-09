using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    public float Substract(float currentValue, float deltaVlue)
    {
        return Mathf.Max(currentValue - deltaVlue, 0f);
    }
}
