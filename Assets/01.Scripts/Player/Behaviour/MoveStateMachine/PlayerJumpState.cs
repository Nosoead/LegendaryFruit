//using System.Collections;
//using Unity.VisualScripting;
//using UnityEngine;

//public class PlayerJumpState : IState
//{

//    private PlayerMovementHandler player;
//    private float jumpSpeed;
//    private float jumpingCoolDown = 0.2f;

//    public PlayerJumpState(PlayerMovementHandler player)
//    {
//        this.player = player;
//    }

//    public void Enter()
//    {
//        //TODO : 애니메이션
//        Debug.Log("Enter Jump State");
//        if (player.CanJump)
//        {
//            if (player.IsDashing)//대쉬 중 점프
//            {
//                player.SetVelocity(Vector2.zero);
//                ApplyJump();
//                return;
//            }
//            else if (!player.IsDashing && player.IsJumping)//점프중에 점프
//            {
//                ApplyJump();
//                return;
//            }
//            else if (!player.IsDashing && !player.IsJumping)//걷다가 뛰는 상황
//            {
//                ApplyJump();
//                return;
//            }
//        }
//    }

//    public void Execute()
//    {
//        if (!player.IsGround)
//        {
//            player.StateMachine.TransitionTo(player.StateMachine.airborneState);
//            return;
//        }
//    }

//    public void Exit()
//    {
//        player.SetIsJumpKeyPressed(false);
//    }

//    private void ApplyJump()
//    {
//        Vector2 currentVelocity = player.GetVelocity();
//        jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * player.GetGravityScale() * player.JumpHeight);
//        Vector2 velocity = player.GetVelocity();
//        if (velocity.y > 0f)
//        {
//            jumpSpeed = Mathf.Max(jumpSpeed = velocity.y, 0f);
//        }
//        else if (velocity.y < 0f)
//        {
//            jumpSpeed += Mathf.Abs(velocity.y);
//        }
//        //player.SetVelocity(new Vector2(velocity.x, 0));
//        velocity.y += jumpSpeed;
//        player.SetIsJumping(true);
//        player.DecreaseDashCount();
//        //player.AddForce(new Vector2(currentVelocity.x, player.JumpHeight), ForceMode2D.Impulse);
//        //coJumpRoutine = player.EnterCoroutine(JumpCooldownRoutine());
//    }

//    //private IEnumerator JumpCooldownRoutine()
//    //{
//    //    player.SetIsJumping(true);
//    //    player.SetCanJump(false);
//    //    player.DecreaseJumpCount();
//    //    yield return new WaitForSeconds(jumpingCoolDown);
//    //    player.SetVelocity(Vector2.zero);
//    //    if (player.CurrentJumpCount != 0)
//    //    {
//    //        player.SetCanJump(true);
//    //    }
//    //}
//}
