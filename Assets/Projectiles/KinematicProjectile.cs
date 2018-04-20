using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Portfolio
{


    public class KinematicProjectile : Projectile
    {

 
        protected override void ApplyForce()
        {
        
        }
        protected override void Start()
        {
            base.Start();
            if (!rigidBody.isKinematic)
            {
                Debug.Log("KINEMATIC PROJECTILE IS ATTACHED TO NON-KINEMATIC OBJECT");
                Destroy(gameObject);
            }
        }

        protected override void FlipRender()
        {
            float deegre = Mathf.Acos(Direction.x) * Mathf.Rad2Deg;
            if (Direction.y < 0.0f)
                deegre += (180 - deegre) * 2; 
            Quaternion _temp = new Quaternion();
            _temp.eulerAngles = new Vector3(0.0f, 0.0f, deegre);
            transform.rotation = _temp;
        }

        protected override Vector2 SetDirection()
        {
            return Vector2.zero;
        }

        protected override Vector2 SetDirection(Character owner)
        { 
            Vector2 _temp = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - owner.transform.position).normalized;
            return _temp /= _temp.magnitude;
        }

        protected override void FixedUpdate()
        {
            transform.Translate(Vector2.right * Speed * 0.001f);
        }
    }
}