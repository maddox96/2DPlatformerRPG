using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{

    public class SpringTrap : Trap
    {

        HingeJoint2D hingeJoint2D;
        JointMotor2D jointMotor2D;

        protected override void Awake()
        {
            base.Awake();
            hingeJoint2D = GetComponent<HingeJoint2D>();
            jointMotor2D = hingeJoint2D.motor;
        }


        protected override void OnBuild()
        {
            rb.isKinematic = false;
            jointMotor2D = hingeJoint2D.motor;
            hingeJoint2D.connectedBody = groundTransform.GetComponent<Rigidbody2D>();
            //hingeJoint2D.connectedAnchor = transform.position + new Vector3(0.0f, hingeJoint2D.transform.localPosition.y, 0.0f);
            Wait();
            base.OnBuild();
        }

        void Wait()
        {
            jointMotor2D.motorSpeed = 0.0f;
        }

        void Shoot()
        {
            jointMotor2D.motorSpeed = 200.0f;
            hingeJoint2D.motor = jointMotor2D;
            Invoke("Reload", 3.0f);
        }

        void Reload()
        {
            jointMotor2D.motorSpeed = -25.0f;
            hingeJoint2D.motor = jointMotor2D;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
            if(collision.transform.tag == "Player")
            {
                Shoot();
            }
        }

    }
}