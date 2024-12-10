using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInteractTap : MonoBehaviour, IInteractable
{
    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            Debug.Log("tap 눌러짐");
        }
    }
}
