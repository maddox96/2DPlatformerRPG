using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class poisonEffect : TickEffect
    {


        protected override void Action()
        {
            target.ApplyDamage((int)statsValue);
        }
     
    }
}