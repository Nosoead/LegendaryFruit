using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    //��� ����
    public Dictionary<string, string> resourcePaths = new Dictionary<string, string>();

    protected override void Awake()
    {
        base.Awake();
        SetResourcePaths();
    }

    private void SetResourcePaths()
    {
        //��� �ڵ� Ž���ؼ� Dicitonary�� ����ؼ� ���� ���� �� ���...
    }

    //Resource �ϳ� ������ �Լ�
    public T LoadResource<T>(string path) where T : Object
    {
        T resource = Resources.Load<T>(path);
        return resource;
    }

    //Resource ���� ������ �Լ�
    public T[] LoadAllResources<T>(string path) where T : Object
    {
        T[] resources = Resources.LoadAll<T>(path);
        return resources;
    }
}
