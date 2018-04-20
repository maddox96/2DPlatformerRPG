using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class SpellHolder : OnClickHolder<Spell>
    {
        
        protected override void OnClickEvent()
        {
            SpellsUpgradeGUI.UpgradeGUI.GenerateUpdatesGUI(holdedObject);
        }
    }
}