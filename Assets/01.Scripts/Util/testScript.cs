using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class testScript : MonoBehaviour
{
    private IObjectPool<testScript> objectPool;
    public IObjectPool<testScript> ObjectPool { set => objectPool = value; }

    public void ReleaseObject()
    {
        objectPool.Release(this);
    }
}
