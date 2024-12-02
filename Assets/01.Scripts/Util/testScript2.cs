using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class testScript2 : MonoBehaviour
{
    private IObjectPool<testScript2> objectPool;
    public IObjectPool<testScript2> ObjectPool { set => objectPool = value; }

    public void ReleaseObject()
    {
        objectPool.Release(this);
    }
}
