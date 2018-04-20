using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class MPBarScaler : BarScaler
    {
        protected override float currentRelativeStat
        {
            get
            {
                return character.CurrentMana;
            }
        }

        protected override float maxRelativeStat
        {
            get
            {
                return character.MaxMana;
            }
        }

        protected override void ScaleBarEvent()
        {
            character.OnManaSpend += scaleBar;
        }
    }

}