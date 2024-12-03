using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 UI프리펩은 자기자신의이름.cs:UIBase를 상속시켜야 한다.
public class UIBase : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
