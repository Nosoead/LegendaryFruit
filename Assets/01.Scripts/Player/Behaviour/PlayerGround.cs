using UnityEngine;

public class PlayerGround : GroundDetector
{
    private LayerMask oneWayBlockLayer;
    private bool isOnewayBlock;
    private Collider2D onewayBlockCollider;

    protected override void FixedUpdate()
    {
        CheckGround();
    }
    protected override void OnDrawGizmos()
    {
        if (isGround) { Gizmos.color = isOnewayBlock ? Color.yellow : Color.green; } else { Gizmos.color = Color.red; }
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
        if (isGround == false && playerRigidbody.velocity.y > 0.01f)
        {
            return;
        }
        isOnewayBlock = Physics2D.Raycast(transform.position + colliderRightOffset, Vector2.down, groundLength, oneWayBlockLayer) || Physics2D.Raycast(transform.position + colliderLeftOffset, Vector2.down, groundLength, oneWayBlockLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + colliderRightOffset, Vector2.down, groundLength, oneWayBlockLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + colliderLeftOffset, Vector2.down, groundLength, oneWayBlockLayer);
        if (hitRight.collider != null && hitLeft.collider != null)
        {
            onewayBlockCollider = hitRight.collider;
        }
        else if (hitRight.collider != null && hitLeft.collider == null)
        {
            onewayBlockCollider = hitRight.collider;
        }
        else if (hitRight.collider == null && hitLeft.collider != null)
        {
            onewayBlockCollider = hitLeft.collider;
        }
    }

    public bool GetOnewayBlock() {return isOnewayBlock; }
    public Collider2D GetOnewayCollider() { return onewayBlockCollider; }
}