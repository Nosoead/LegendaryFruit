using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StatHandler
{
    public void Add(ref float currentValue, float someValue)
    { 
        currentValue = currentValue + someValue;
    }

    public float Substract(ref float currentValue, float someValue)
    {
        float result = currentValue - someValue;
        return result;
    }
}
