using UnityEngine;

public class MonsterGround : GroundDetector
{
    private bool isGround;
    protected override void CheckGround()
    {
        {
            isGround = base.GetOnGround();
            if (isGround == false && playerRigidbody.velocity.y > 0.01f)
            {
                return;
            }
            isGround = Physics2D.Raycast(transform.position + colliderRightOffset, Vector2.down, groundLength, groundLayer)
                               && Physics2D.Raycast(transform.position + colliderLeftOffset, Vector2.down, groundLength, groundLayer);
            //Debug.Log($"isGround체크 {isGround}");
        }
    }

    public bool GetOnGround() { return isGround; }
}