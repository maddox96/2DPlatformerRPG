using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class Spikes : Trap
    {
        protected override void Action(Character c)
        {
            c.ApplyDamage(damage);
        }

    }

}
