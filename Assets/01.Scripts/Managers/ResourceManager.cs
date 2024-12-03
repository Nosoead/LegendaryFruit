using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    //경로 저장
    public Dictionary<string, string> resourcePaths = new Dictionary<string, string>();

    protected override void Awake()
    {
        base.Awake();
        SetResourcePaths();
    }

    private void SetResourcePaths()
    {
        //경로 자동 탐색해서 Dicitonary에 등록해서 빼서 쓰는 것 고려...
    }

    //Resource 하나 들고오는 함수
    public T LoadResource<T>(string path) where T : Object
    {
        T resource = Resources.Load<T>(path);
        return resource;
    }

    //Resource 전부 들고오는 함수
    public T[] LoadAllResources<T>(string path) where T : Object
    {
        T[] resources = Resources.LoadAll<T>(path);
        return resources;
    }
}
