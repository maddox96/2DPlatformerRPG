using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimatedPixelPack
{


    public class BubbleShield : Spell
    {
        protected override void Start()
        {
            base.Start();
            transform.position = new Vector3(target.x, 0.0f, 1.0f);
        }
    }
}