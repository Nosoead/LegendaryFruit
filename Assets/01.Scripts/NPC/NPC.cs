using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    public abstract void InitNPC();
    public virtual void SetReward()
    {
        return;
    }

    public virtual void ReleaseReward()
    {
        return;
    }
}
