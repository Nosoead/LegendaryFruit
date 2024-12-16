using UnityEngine;

public class PlayerGround : GroundDetector
{
    private LayerMask oneWayBlockLayer;
    private bool isOneWayBlock;

    protected override void OnDrawGizmos()
    {
        if (isGround) { Gizmos.color = isOneWayBlock ? Color.yellow : Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + colliderRightOffset, transform.position + colliderRightOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position + colliderLeftOffset, transform.position + colliderLeftOffset + Vector3.down * groundLength);
    }

    protected override void SetLayerMask()
    {
        base.SetLayerMask();
        oneWayBlockLayer = LayerMask.GetMask("Block");
    }

    protected override void CheckGround()
    {
        base.CheckGround();
        isOneWayBlock = Physics2D.Raycast(transform.position + colliderRightOffset, Vector2.down, groundLength, oneWayBlockLayer) || Physics2D.Raycast(transform.position + colliderLeftOffset, Vector2.down, groundLength, oneWayBlockLayer);
    }

    public bool GetOneWayBlock() { return isOneWayBlock; }
}