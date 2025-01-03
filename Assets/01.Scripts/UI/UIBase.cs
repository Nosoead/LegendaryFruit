using UnityEngine;

// 각 UI클래스는 자신의 이름.cs: UIBase를 상속받는다.
public class UIBase : MonoBehaviour
{
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        Destroy(gameObject);
    }
}
