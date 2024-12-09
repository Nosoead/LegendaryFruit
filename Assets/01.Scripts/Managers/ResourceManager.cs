using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    // 파일 저장
    public Dictionary<string, string> resourcePaths = new Dictionary<string, string>();

    protected override void Awake()
    {
        base.Awake();
        SetResourcePaths();
    }

    private void SetResourcePaths()
    {
        // 파일경로를 읽어와 Dictionary에 저장하여 데이터 관리 용도로 사용
    }

    // Resource 하나를 반환하는 함수
    public T LoadResource<T>(string path) where T : Object
    {
        T resource = Resources.Load<T>(path);
        return resource;
    }

    // Resource 전체를 반환하는 함수
    public T[] LoadAllResources<T>(string path) where T : Object
    {
        T[] resources = Resources.LoadAll<T>(path);
        return resources;
    }
}
