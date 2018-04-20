using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class HPBarScaler : BarScaler
    {
        protected override float currentRelativeStat
        {
            get
            {
                return character.CurrentHealth;

            }
        }
    
        protected override float maxRelativeStat
        {
            get
            {
                return character.MaxHealth;
            }
        }

        protected override void ScaleBarEvent()
        {
            character.OnDamageTake += scaleBar;
        }
    }
}