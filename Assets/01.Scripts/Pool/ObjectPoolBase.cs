using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolBase : MonoBehaviour
{
    protected IObjectPool<ObjectPoolBase> objectPool;
    public IObjectPool<ObjectPoolBase> ObjectPool { set => objectPool = value; }
}
