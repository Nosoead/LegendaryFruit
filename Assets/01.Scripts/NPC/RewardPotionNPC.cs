using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPotionNPC : RewardNPC, IInteractable
{


    public override void InitNPC()
    {
        base.InitNPC();
        

    }

    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            //GetWeaponFromReward();
            
        }
    }

    public override void SetReward()
    {

    }

    public override void ReleaseReward()
    {
        base.ReleaseReward();
        
    }
}