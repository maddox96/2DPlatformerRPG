using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class SpikedBall : Trap
    {

        Rigidbody2D chainRb;

        protected override void Awake()
        {
            base.Awake();
            chainRb = Utility.GetComponentInChildrenWithoutParent<Rigidbody2D>(transform);
            chainRb.simulated = false;

        }
    
        protected override void OnBuild()
        {
            base.OnBuild();
            chainRb.simulated = true;
            chainRb.AddTorque(50.0f);
        }
    }
}