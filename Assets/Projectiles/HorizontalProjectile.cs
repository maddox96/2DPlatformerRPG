using UnityEngine;
using System.Collections;
using System;

namespace Portfolio
{
    public class HorizontalProjectile : Projectile
    {
        [Tooltip("When projectile doesnt have a caster, should it move RIGHT or LEFT?")]
        public bool ShouldMoveRight;

        #region OVERRIDEN MECHANICS
        protected override void ApplyForce()
        {
            if (rigidBody != null)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY;
                rigidBody.AddForce(new Vector2(Direction.x * Speed, 0));
            }
        }

        protected override Vector2 SetDirection(Character owner)
        {
            return owner.CurrentDirection == Character.Direction.Right ? Vector2.right : Vector2.left;  
        }

        protected override Vector2 SetDirection()
        {
            return ShouldMoveRight ? Vector2.right : Vector2.left;
        }

        protected override void FlipRender()
        {
            if (ShouldFlipDirection && Direction.x < 0)
            {
                transform.Rotate(0.0f, 0.0f, Direction.x * 180);
            }
        }

        #endregion
    }
}
 