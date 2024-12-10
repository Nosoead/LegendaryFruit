using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInteractHold : MonoBehaviour, IInteractable
{
    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed)
        {
            Debug.Log("Hold 눌러짐");
        }
        if (isPressed)
        {
            Debug.Log("Tap 눌러짐");
        }
        Debug.Log("취소");
    }

}
