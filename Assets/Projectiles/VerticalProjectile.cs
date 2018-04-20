using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{

    public class VerticalProjectile : Projectile
    {
        [Tooltip("Should it move UP or DOWN?")]
        public bool ShouldMoveUp;

        #region OVERRIDEN MECHANICS
        protected override void FlipRender()
        {
            if (ShouldFlipDirection)
            {
                transform.Rotate(0.0f, 0.0f, Direction.y * 90);
            }
        }

        protected override Vector2 SetDirection(Character owner)
        {
            return SetDirection();

        }

        protected override Vector2 SetDirection()
        {
            if (ShouldMoveUp) return Vector2.up;
            else return Vector2.down;
        }

        protected override void ApplyForce()
        {
            float y = Direction.y * Speed;
            Rigidbody2D body = GetComponent<Rigidbody2D>();
            body.constraints = RigidbodyConstraints2D.FreezePositionX;
            if (body != null)
            {
                body.AddForce(new Vector2(0, y));
            }
        }

        #endregion
    }

} 