using UnityEngine;

public class MosterGround : GroundDetector
{
    /// <summary>
    /// ���Ͻô� ��ġ�� ������ �� �ֽ��ϴ�.
    /// </summary>
    protected override void SetColliderOffset()
    {
        Vector3 offsetValue = new Vector3(characterCollider.bounds.extents.x, 0, 0);
        colliderOffset = offsetValue;
    }
}