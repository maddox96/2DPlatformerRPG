using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Portfolio
{


    public class speedEffect : TickEffect  {

        protected override void Action()
        {
            target.RunSpeed += statsValue;
        }
    }
}