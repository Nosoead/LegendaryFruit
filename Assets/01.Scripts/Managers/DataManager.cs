using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private string savePath;

    protected override void Awake()
    {
        base.Awake();
        savePath = Application.persistentDataPath;
    }

    public void SaveData<T>(T data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath + $"/{typeof(T).ToString()}.txt", json);
        Debug.Log(savePath + $"/{typeof(T).ToString()}.txt");
    }

    public T LoadData<T>() //where T : class
    {
        Debug.Log(savePath + $"/{typeof(T).ToString()}.txt");
        if (File.Exists(savePath + $"/{typeof(T).ToString()}.txt"))
        {
            string loadJson = File.ReadAllText(savePath + $"/{typeof(T).ToString()}.txt");
            return JsonUtility.FromJson<T>(loadJson);
        }
        else
        {
            return default(T);
        }
    }

    public void DeleteData<T>()
    {
        if (File.Exists(savePath + $"/{typeof(T).ToString()}.txt"))
        {
            File.Delete(savePath + $"/{typeof(T).ToString()}.txt");
        }
    }

    public bool GetCanLoad<T>()
    {
        if (File.Exists(savePath + $"/{typeof(T).ToString()}.txt"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}