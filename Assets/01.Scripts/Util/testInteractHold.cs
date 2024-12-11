using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInteractHold : MonoBehaviour, IInteractable
{
    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed)
        {
            Debug.Log("2초이상 눌러서 Hold 눌러짐");
            return;
        }
        else if (isPressed)
        {
            Debug.Log("1초전에 풀어서 Tap 눌러짐");
            return;
        }
    }
}
