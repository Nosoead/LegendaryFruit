using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] protected BoxCollider2D characterCollider;
    [SerializeField] protected Rigidbody2D playerRigidbody;
    [SerializeField] protected float colliderY = 0;
    [SerializeField] protected float groundLength = 0.6f;
    protected Vector3 colliderRightOffset;
    protected Vector3 colliderLeftOffset;
    protected LayerMask groundLayer;
    protected bool isGround;

    private void Awake()
    {
        if (characterCollider == null)
        {
            characterCollider = GetComponent<BoxCollider2D>();
        }
        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
        }
    }

    private void Start()
    {
        SetLayerMask();
        SetColliderOffset();
    }

    protected virtual void FixedUpdate()
    {
        CheckGround();
    }

    protected virtual void OnDrawGizmos()
    {
        if (isGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + colliderRightOffset, transform.position + colliderRightOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position + colliderLeftOffset, transform.position + colliderLeftOffset + Vector3.down * groundLength);
    }

    protected virtual void SetLayerMask()
    {
        groundLayer = LayerMask.GetMask("Ground") | LayerMask.GetMask("Block");
    }

    protected virtual void SetColliderOffset()
    {
        Vector3 rightOffset = new Vector3(characterCollider.bounds.extents.x, colliderY, 0);
        Vector3 leftOffset = new Vector3(-characterCollider.bounds.extents.x, colliderY, 0);
        colliderRightOffset = rightOffset;
        colliderLeftOffset = leftOffset;

    }

    protected virtual void CheckGround()
    {
        if (isGround == false && playerRigidbody.velocity.y > 0.01f)
        {
            return;
        }
        isGround = Physics2D.Raycast(transform.position + colliderRightOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position + colliderLeftOffset, Vector2.down, groundLength, groundLayer);
    }
    public bool GetOnGround() { return isGround; }
}
