using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] protected BoxCollider2D characterCollider;
    [SerializeField] protected Rigidbody2D playerRigidbody;
    [SerializeField] protected float groundLength = 0.6f;
    protected Vector3 colliderOffset;
    protected LayerMask groundLayer;
    private bool onGround;

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

    private void Update()
    {
        CheckGround();
    }

    private void OnDrawGizmos()
    {
        if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Debug.Log(onGround);
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

    protected virtual void SetLayerMask()
    {
        groundLayer = LayerMask.GetMask("Ground") | LayerMask.GetMask("Block");
    }

    protected virtual void SetColliderOffset()
    {
        Vector3 offsetValue = new Vector3(characterCollider.bounds.extents.x, 0, 0);
        colliderOffset = offsetValue;
    }

    protected virtual void CheckGround()
    {
        if (onGround == false && playerRigidbody.velocity.y > 0.01f)
        {
            return;
        }
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
    }
    public bool GetOnGround() { return onGround; }
}
