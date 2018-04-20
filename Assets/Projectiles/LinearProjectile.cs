using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Portfolio
{
    public class LinearProjectile : Projectile
    {
        public Utility.Direction direction;

        protected override void ApplyForce()
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            GetComponent<Rigidbody2D>().AddForce(Direction * Speed);
        }

        protected override void FlipRender()
        {
           if(ShouldFlipDirection)
                transform.Rotate(0.0f, 0.0f, Utility.DirectionToZRotation(direction));

        }

        public LinearProjectile Shoot(int damage, Vector3 startPosition, Utility.Direction dir)
        {
            LinearProjectile projectile = Instantiate(this, startPosition, Quaternion.identity);
            projectile.direction = dir;
            projectile.SetLifeTime(projectile.lifeTime);
            projectile.Damage = damage;
            return projectile;
        }

        protected override Vector2 SetDirection()
        {
            return Utility.DirectionToVector(direction);
        }

        protected override Vector2 SetDirection(Character owner)
        {
            return SetDirection();
        }
    }
}