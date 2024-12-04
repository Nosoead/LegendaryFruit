using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttributeLogics 
{
    //각 상태이상에 대한 추상클래스
    public virtual bool CanPenetrate => false;
    public abstract void ApplyAttackLogic(GameObject target, float damage);
    //각 상태이상에 대한 정의

}
