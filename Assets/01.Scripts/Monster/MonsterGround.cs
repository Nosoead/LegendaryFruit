using UnityEngine;

public class MosterGround : GroundDetector
{
    /// <summary>
    /// 원하시는 위치에 조절할 수 있습니다.
    /// </summary>
    protected override void SetColliderOffset()
    {
        Vector3 offsetValue = new Vector3(characterCollider.bounds.extents.x, 0, 0);
        colliderOffset = offsetValue;
    }
}