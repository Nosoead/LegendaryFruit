using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��� UI�������� �ڱ��ڽ����̸�.cs:UIBase�� ��ӽ��Ѿ� �Ѵ�.
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
