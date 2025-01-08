using UnityEngine;

public class MonsterGround : GroundDetector
{
    protected override void CheckGround()
    {
        {
            if (isGround == false && playerRigidbody.velocity.y > 0.01f)
            {
                return;
            }
            isGround = Physics2D.Raycast(transform.position + colliderRightOffset, Vector2.down, groundLength, groundLayer)
                               && Physics2D.Raycast(transform.position + colliderLeftOffset, Vector2.down, groundLength, groundLayer);
        }
    }
}