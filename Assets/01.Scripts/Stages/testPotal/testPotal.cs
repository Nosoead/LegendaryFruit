using UnityEngine;

public class testPotal : MonoBehaviour, IInteractable
{
    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if(isDeepPressed || isPressed)
        {
            ChagedStage();
        }
    }
    protected virtual void ChagedStage()
    {

    }
}
