using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 UI클래스는 자신의 이름.cs: UIBase를 상속받는다.
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
