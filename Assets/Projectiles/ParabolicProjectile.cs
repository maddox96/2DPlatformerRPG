using UnityEngine;


namespace Portfolio
{
    public class ParabolicProjectile : Projectile
    {

        #region OVERRIDEN MECHANICS
        protected override void ApplyRotation()
        {
            if (rigidBody)
            {
                Vector3 vel = rigidBody.velocity;
                transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg);
            }
        }

        protected override void ApplyForce()
        {
            if (rigidBody != null && Direction != Vector2.zero)
            {
               rigidBody.AddForce(Direction * Speed / (Direction.magnitude), ForceMode2D.Impulse);
            }
        }

        protected override Vector2 SetDirection()
        {
            return Vector2.zero;
        }

        protected override Vector2 SetDirection(Character owner)
        {
            return (Camera.main.ScreenToWorldPoint(Input.mousePosition) - owner.transform.position).normalized;
        }

        protected override void FlipRender()
        {
            return;
        }

        #endregion

    }
}